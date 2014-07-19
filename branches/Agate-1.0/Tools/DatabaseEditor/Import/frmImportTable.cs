﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib.Data;

namespace AgateDatabaseEditor
{
	public partial class frmImportTable : Form
	{
		string fileContents;

		public frmImportTable()
		{
			InitializeComponent();

			UpdateDelimiters();
			textQualifier = cboTextQualifier.Text;
		}

		public AgateDatabase Database { get; set; }
		char[] Delimiters;
		string textQualifier;
		bool firstRowFieldNames = true;
		bool mergeDelimiters = false;
		AgateTable importedTable;

		public string Filename { get; set; }

		private void frmImportData_Load(object sender, EventArgs e)
		{
			fileContents = System.IO.File.ReadAllText(Filename);

			txtName.Text = System.IO.Path.GetFileNameWithoutExtension(Filename);
			txtFileContents.Text = fileContents;

			RedoImport();
		}

		private void DelimiterCheck_CheckedChanged(object sender, EventArgs e)
		{
			UpdateDelimiters();
			RedoImport();
		}

		private void UpdateDelimiters()
		{
			List<char> delim = new List<char>();

			if (chkComma.Checked) delim.Add(',');
			if (chkSemicolon.Checked) delim.Add(';');
			if (chkSpace.Checked) delim.Add(' ');
			if (chkTab.Checked) delim.Add('\t');
			if (chkOther.Checked && txtOther.Text.Length > 0)
				delim.Add(txtOther.Text[0]);

			Delimiters = delim.ToArray();
		}


		private void chkMergeDelimiters_CheckedChanged(object sender, EventArgs e)
		{
			mergeDelimiters = chkMergeDelimiters.Checked;
			RedoImport();
		}

		private void comboBox1_TextChanged(object sender, EventArgs e)
		{
			if (cboTextQualifier.Text != "{none}")
			{
				textQualifier = cboTextQualifier.Text;
			}
			else
				textQualifier = null;

			RedoImport();
		}

		private void chkFirstRow_CheckedChanged(object sender, EventArgs e)
		{
			firstRowFieldNames = chkFirstRow.Checked;

			RedoImport();
		}
		private void RedoImport()
		{
			btnOK.Enabled = false;
			lstColumns.Enabled = false;
			propColumns.Enabled = false;
			lstColumns.Items.Clear();

			if (backgroundWorker1.IsBusy)
				backgroundWorker1.CancelAsync();

			backgroundWorker1.RunWorkerAsync();
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			importedTable = null;

			string[] lines = SplitFileContents();

			List<AgateColumn> cols = new List<AgateColumn>();
			DetectColumnTypes(lines, cols);

			if (firstRowFieldNames)
				SetColumnNames(lines[0], cols);
			else
				SetDefaultColumnNames(cols);


			
			AgateTable tbl = ImportTable(lines, cols);

			importedTable = tbl;
		}

		private string[] SplitFileContents()
		{
			string[] lines = fileContents.Split('\n');
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].EndsWith("\r"))
				{
					lines[i] = lines[i].Substring(0, lines[i].Length - 1);
				}
			}
			return lines;
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (importedTable == null)
				return;

			lstColumns.Items.AddRange(importedTable.Columns.ToArray());
			lstColumns.Enabled = true;
			propColumns.Enabled = true;

			if (importedTable == null)
				return;

			if (string.IsNullOrEmpty(txtName.Text))
				return;

			if (Database.Tables.ContainsTable(txtName.Text))
				return;

			SetOKEnabled();
		}

		private void SetOKEnabled()
		{
			bool value = true;

			if (backgroundWorker1.IsBusy)
				value = false;

			if (Database.Tables.ContainsTable(txtName.Text))
			{
				if (chkOverwrite.Checked == false)
					value = false;
			}

			btnOK.Enabled = value;
		}

		private void txtName_TextChanged(object sender, EventArgs e)
		{
			if (Database.Tables.ContainsTable(txtName.Text))
			{
				pnlTableWarning.Visible = true;
				chkOverwrite.Checked = false;
			}
			else
				pnlTableWarning.Visible = false;

			SetOKEnabled();
		}

		private void SetDefaultColumnNames(List<AgateColumn> cols)
		{
			for (int i = 0; i < cols.Count; i++)
			{
				cols[i].Name = "Column" + (i + 1).ToString();
			}
		}

		private AgateTable ImportTable(string[] lines, List<AgateColumn> cols)
		{
			AgateTable retval = new AgateTable();

			foreach (var col in cols)
			{
				retval.AddColumn(col);
			}

			int start = 0;

			if (firstRowFieldNames)
				start = 1;

			for (int i = start; i < lines.Length; i++)
			{
				if (lines[i].Trim().Length == 0)
					continue;

				string[] text = SplitLine(lines[i], mergeDelimiters);

				AgateRow r = new AgateRow(retval);

				for (int k = 0; k < text.Length; k++)
				{
					if (cols[k].FieldType == FieldType.Boolean)
					{
						if (text[k] == "0" || text[k] == "1")
						{
							r[cols[k]] = text[k] == "1" ? "true" : "false";
							continue;
						}
					}

					string value = StripTextQualifier(text[k]);

					if (string.IsNullOrEmpty(value))
						value = cols[k].DefaultValue;

					r[cols[k]] = value;
				}

				retval.Rows.Add(r);
			}

			return retval;
		}

		private void SetColumnNames(string line, List<AgateColumn> cols)
		{
			string[] text = SplitLine(line, mergeDelimiters);

			for (int i = 0; i < text.Length; i++)
			{
				if (i < cols.Count)
				{
					try
					{
						cols[i].Name = StripTextQualifier(text[i]);
					}
					catch
					{
						cols[i].Name = "Column" + (i + 1).ToString();
					}
				}
				else
				{
					cols.Add(new AgateColumn { Name = StripTextQualifier(text[i]) });
				}
			}
		}

		private string StripTextQualifier(string p)
		{
			if (textQualifier == null)
				return p;

			if (p.StartsWith(textQualifier) && p.EndsWith(textQualifier))
			{
				return p.Substring(textQualifier.Length, p.Length - textQualifier.Length * 2);
			}
			else if (p.StartsWith(textQualifier))
			{
				return p.Substring(textQualifier.Length);
			}

			return p;
		}

		private void DetectColumnTypes(string[] lines, List<AgateColumn> cols)
		{
			int start = 0;

			if (firstRowFieldNames)
			{
				start = 1;

				string[] text = SplitLine(lines[0], mergeDelimiters);

				for (int i = 0; i < text.Length; i++)
				{
					if (cols.Count == i)
						cols.Add(new AgateColumn());

					if (text[i].StartsWith("\"") && text[i].EndsWith("\""))
					{
						text[i] = text[i].Substring(1, text[i].Length - 2);
					}

					cols[i].Name = text[i];

					// set the most conservative field type.
					cols[i].FieldType = FieldType.Boolean;
				}
			}

			for (int i = start; i < lines.Length; i++)
			{
				int intTrial;
				double doubleTrial;
				decimal decimalTrial;
				DateTime dateTrial;

				if (lines[i].Trim().Length == 0)
					continue;

				string[] text = SplitLine(lines[i], mergeDelimiters);

				for (int j = 0; j < text.Length; j++)
				{
					if (text[j] == "")
						continue;

					if (j < cols.Count && cols[j].FieldType == FieldType.String)
						continue;

					if (IsBoolCompatible(text[j]))
					{
						ColumnType(FieldType.Boolean, cols, j);
					}
					else if (int.TryParse(text[j], out intTrial))
					{
						ColumnType(FieldType.Int32, cols, j);
					}
					else if (double.TryParse(text[j], out doubleTrial))
					{
						ColumnType(FieldType.Double, cols, j);
					}
					else if (decimal.TryParse(text[j], out decimalTrial))
					{
						ColumnType(FieldType.Decimal, cols, j);
					}
					else if (DateTime.TryParse(text[j], out dateTrial))
					{
						ColumnType(FieldType.DateTime, cols, j);
					}
					else
					{
						ColumnType(FieldType.String, cols, j);
					}
				}
			}

			for (int i = 0; i < cols.Count; i++)
			{
				cols[i].DisplayIndex = i;
			}
		}

		private static bool IsBoolCompatible(string text)
		{
			if (text == "0" || text == "1")
				return true;

			bool trial;
			return bool.TryParse(text, out trial);

		}

		private void ColumnType(FieldType fieldType, List<AgateColumn> cols, int j)
		{
			if (j < cols.Count)
			{
				if (cols[j].FieldType == fieldType)
					return;

				// check to see if the data type can be promoted to what type is already in the field
				if (CanPromoteFieldType(fieldType, cols[j].FieldType))
					return;

				// check to see if the field type can be promoted to the data type of the current data being imported.
				if (CanPromoteFieldType(cols[j].FieldType, fieldType))
				{
					cols[j].FieldType = fieldType;
				}
				else
				{
					cols[j].FieldType = FieldType.String;
				}
			}
			else
			{
				AgateColumn newCol = new AgateColumn();

				newCol.FieldType = fieldType;

				cols.Add(newCol);
			}
		}

		private bool CanPromoteFieldType(FieldType currentType, FieldType toType)
		{
			if (toType == FieldType.DateTime)
				return false;

			switch (currentType)
			{
				case FieldType.Boolean:
					return true;

				case FieldType.Int32:
					switch (toType)
					{
						case FieldType.Double:
						case FieldType.Decimal:
							return true;
					}

					return false;

				case FieldType.Double:
					switch (toType)
					{
						case FieldType.Decimal:
							return true;
					}

					return false;
			}

			return false;
		}

		private string[] SplitLine(string line, bool removeEmpties)
		{
			List<string> retval = new List<string>();

			bool inString = false;

			int pos = 0;
			int start = pos;
			while (pos < line.Length)
			{
				if (Delimiters.Contains(line[pos]) && inString == false)
				{
					retval.Add(line.Substring(start, pos - start));

					start = pos + 1;
				}
				else if (textQualifier != null && line.Substring(pos, textQualifier.Length) == textQualifier)
				{
					inString = !inString;
				}

				pos++;
			}

			retval.Add(line.Substring(start));

			if (removeEmpties)
			{
				for (int i = 0; i < retval.Count; i++)
				{
					if (string.IsNullOrEmpty(retval[i]))
					{
						retval.RemoveAt(i);
						i--;
					}
				}
			}

			return retval.ToArray();
		}

		private void lstColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			propColumns.SelectedObject = lstColumns.SelectedItem;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			importedTable = ImportTable(SplitFileContents(), importedTable.Columns.ToList());

			try
			{
				importedTable.Name = txtName.Text;
			}
			catch(Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error creating table", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				DialogResult = DialogResult.None;

				return;
			}

			if (Database.Tables.ContainsTable(importedTable.Name) &&
				chkOverwrite.Checked == true)
			{
				Database.Tables.Remove(Database.Tables[importedTable.Name]);
			}

			Database.Tables.Add(importedTable);
		}

		private void propColumns_Click(object sender, EventArgs e)
		{

		}

		private void chkOverwrite_CheckedChanged(object sender, EventArgs e)
		{
			SetOKEnabled();
		}

	}
}
