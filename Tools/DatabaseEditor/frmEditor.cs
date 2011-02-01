using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateDataLib;

namespace AgateDatabaseEditor
{
	public partial class frmEditor : Form
	{
		string filename;
		string title;

		public frmEditor()
		{
			InitializeComponent();

			statusLabel.Text = "";

			title = Text;
		}

		#region --- Form Events ---

		private void frmEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (CheckSave() == false)
				e.Cancel = true;

		}

		#endregion

		#region --- Basic database operations ---

		private void NewDatabase()
		{
			if (CheckSave() == false)
				return;

			databaseEditor1.Visible = true;
			databaseEditor1.Database = new AgateLib.Data.AgateDatabase();

			Text = "New Database - " + title;
		}
		
		private void OpenDatabase()
		{
			if (CheckSave() == false)
				return;
			if (openDatabase.ShowDialog() == DialogResult.Cancel)
				return;

			databaseEditor1.Visible = true;
			databaseEditor1.Database = AgateLib.Data.AgateDatabase.FromFile(openDatabase.FileName);

			filename = openDatabase.FileName;

			Text = System.IO.Path.GetFileName(filename) + " - " + title;
		}

		
		/// <summary>
		/// Asks the user if they want to save before continuing the next operation.
		/// Returns false if the next operation should be canceled.
		/// </summary>
		/// <returns></returns>
		private bool CheckSave()
		{
			if (databaseEditor1.Database == null)
				return true;
			if (databaseEditor1.DirtyState == false)
				return true;

			string name = System.IO.Path.GetFileName(filename);

			if (string.IsNullOrEmpty(filename))
				name = "untitled database";

			var result = MessageBox.Show(this,
				"Do you want to save " + name + " first?",
				"Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

			if (result == DialogResult.Yes)
			{
				return Save();
			}
			else if (result == DialogResult.No)
			{
				return true;
			}
			else
				return false;
		}
		/// <summary>
		/// Saves the file, asking the user if there is no filename.
		/// Returns false if the file couldn't be saved.
		/// </summary>
		/// <returns></returns>
		private bool Save()
		{
			if (string.IsNullOrEmpty(filename))
				return SaveAs();

			AgateDataLib.DatabaseWriter w = new AgateDataLib.DatabaseWriter();
			w.Database = databaseEditor1.Database;

			try
			{
				w.WriteData(filename);
			}
			catch (Exception e)
			{
				MessageBox.Show("Failed to save file." + Environment.NewLine + e.Message);
				return false;
			}

			databaseEditor1.DirtyState = false;

			return true;
		}
		/// <summary>
		/// Saves the file, asking the user for a filename.
		/// Returns false if the file couldn't be saved.
		/// </summary>
		/// <returns></returns>
		private bool SaveAs()
		{
			saveDatabase.FileName = filename;

			if (saveDatabase.ShowDialog() == DialogResult.OK)
			{
				filename = saveDatabase.FileName;

				return Save();
			}
			else
				return false;
		}

		#endregion
		#region --- Toolstrip Menu Items ---

		private void newDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewDatabase();
		}
		private void openDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenDatabase();
		}

		private void saveDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Save();
		}
		private void saveDatabaseAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveAs();
		}
		private void generateCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GenerateCode();
		}
		private void importDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Import.DatabaseImporter importer = new Import.DatabaseImporter();

			importer.Database = databaseEditor1.Database;

			importer.Run();

			databaseEditor1.DatabaseRefresh();

		}
		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		#endregion
		#region --- Tool strip button events ---

		private void btnNew_Click(object sender, EventArgs e)
		{
			NewDatabase();
		}
		private void btnOpen_Click(object sender, EventArgs e)
		{
			OpenDatabase();
		}
		private void btnSave_Click(object sender, EventArgs e)
		{
			Save();
		}

		private void btnDesignTable_Click(object sender, EventArgs e)
		{
			databaseEditor1.DesignCurrentTable();
		}
		private void btnSortAscending_Click(object sender, EventArgs e)
		{
			databaseEditor1.CurrentTableSortAscending();
		}
		private void btnSortDescending_Click(object sender, EventArgs e)
		{
			databaseEditor1.CurrentTableSortDescending();
		}

		private void btnCodeGen_Click(object sender, EventArgs e)
		{
			GenerateCode();
		}

		#endregion

		#region --- Code generation ---

		private void GenerateCode()
		{
			frmCodeGenerator frm = new frmCodeGenerator();

			frm.Namespace = databaseEditor1.Database.CodeNamespace;
			frm.StartDirectory = System.IO.Path.GetDirectoryName(filename);

			if (frm.ShowDialog() == DialogResult.OK)
			{
				databaseEditor1.Database.CodeNamespace = frm.Namespace;

				GenerateCode(frm.CodeDomProviderType, frm.Directory, frm.Namespace);

				MessageBox.Show("Code generation completed.",
					"Code Generation", MessageBoxButtons.OK,
					MessageBoxIcon.Information);
			}

			databaseEditor1.DirtyState = true;
		}
		private void GenerateCode(Type type, string directory, string theNamespace)
		{
			GenerateCode((System.CodeDom.Compiler.CodeDomProvider)
				Activator.CreateInstance(type), directory, theNamespace);
		}
		private void GenerateCode(System.CodeDom.Compiler.CodeDomProvider provider, string directory, string theNamespace)
		{
			CodeGenerator gen = new CodeGenerator();

			gen.Provider = provider;
			gen.Directory = directory;
			gen.Namespace = theNamespace;

			gen.Run(databaseEditor1.Database);

		}

		#endregion

		#region --- Database editor events ---

		private void databaseEditor1_TableActiveStatusChanged(object sender, EventArgs e)
		{
			tableToolStrip.Enabled = databaseEditor1.IsTableActive;
		}
		private void databaseEditor1_DirtyStateChanged(object sender, EventArgs e)
		{
			if (databaseEditor1.DirtyState)
			{
				if (this.Text.StartsWith("* ") == false)
				{
					this.Text = "* " + this.Text;
				}
			}
			else
			{
				if (this.Text.StartsWith("* ") == true)
				{
					this.Text = this.Text.Substring(2);
				}
			}
		}
		private void databaseEditor1_StatusText(object sender, StatusTextEventArgs e)
		{
			switch (e.StatusTextIcon)
			{
				case StatusTextIcon.Information:
					statusLabel.Image = Properties.Resources.infoBubble;
					break;

				case StatusTextIcon.Warning:
					statusLabel.Image = Properties.Resources.warning;
					break;

				case StatusTextIcon.Error:
					statusLabel.Image = Properties.Resources.Error;
					break;
			}

			statusLabel.Text = e.Text;
		}

		#endregion



	}
}
