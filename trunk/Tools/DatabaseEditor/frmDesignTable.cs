using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib.Data;

namespace AgateDatabaseEditor
{
	public partial class frmDesignTable : Form
	{
		public frmDesignTable()
		{
			InitializeComponent();

			foreach (FieldType f in Enum.GetValues(typeof(FieldType)))
			{
				colDataType.Items.Add(f);
			}
		}

		AgateDatabase mDatabase;
		AgateTable mTable;
		AgateColumn mColumnInEdit;
		int mRowInEdit = -1;

		public AgateDatabase TheDatabase
		{
			get { return mDatabase; }
			set
			{
				mDatabase = value;

				cboTableLookup.Items.Clear();

				cboTableLookup.Items.Add(new { Name = "(none)" });

				foreach (var table in mDatabase.Tables)
				{
					cboTableLookup.Items.Add(table);
				}
			}
		}
		public AgateTable TheTable
		{
			get { return mTable; }
			set
			{
				if (mDatabase == null)
					throw new ArgumentNullException("TheDatabase should be set first.");

				mTable = value;

				cboTableLookup.Items.Remove(value);

				gridColumns.RowCount = mTable.Columns.Count + 1;
			}
		}

		internal static void EditColumns(AgateLib.Data.AgateDatabase dbase, AgateLib.Data.AgateTable mTable)
		{
			frmDesignTable d = new frmDesignTable();

			d.TheDatabase = dbase;
			d.TheTable = mTable;

			d.ShowDialog();
		}

		private void gridColumns_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex == gridColumns.RowCount - 1) return;

			AgateColumn col = null;

			if (e.RowIndex == mRowInEdit)
				col = mColumnInEdit;
			else if (e.RowIndex >= mTable.Columns.Count)
				return;
			else
				col = mTable.Columns[e.RowIndex];

			switch (e.ColumnIndex)
			{
				case 0:
					e.Value = col.Name;
					break;
				case 1:
					e.Value = col.FieldType;
					break;
				case 2:
					e.Value = col.Description;
					break;
			}
		}

		private void gridColumns_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			AgateColumn col = null;

			if (e.RowIndex < mTable.Columns.Count)
			{
				if (mColumnInEdit == null)
				{
					mColumnInEdit = mTable.Columns[e.RowIndex].Clone();
				}

				this.mRowInEdit = e.RowIndex;
			}

			col = mColumnInEdit;

			switch (e.ColumnIndex)
			{
				case 0:
					col.Name = e.Value as string;
					break;

				case 1:
					col.FieldType = (FieldType)Enum.Parse(typeof(FieldType), e.Value.ToString());
					break;

				case 2:
					col.Description = e.Value as string;
					break;
			}
		}

		private void gridColumns_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
		{
			this.mColumnInEdit = new AgateColumn();
			this.mRowInEdit = gridColumns.Rows.Count - 1;

			this.mColumnInEdit.DisplayIndex = gridColumns.Rows.Count - 1;
		}

		private void gridColumns_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			// Save row changes if any were made and release the edited 
			// Column object if there is one.
			if (mColumnInEdit != null && 
				e.RowIndex >= mTable.Columns.Count &&
				e.RowIndex != gridColumns.Rows.Count - 1)
			{
				// Add the new Column object to the data store.
				mTable.AddColumn(mColumnInEdit);
				mColumnInEdit = null;
				mRowInEdit = -1;
			}
			else if (mColumnInEdit != null &&
				e.RowIndex < mTable.Columns.Count)
			{
				// Save the modified Column object in the data store.
				mTable.OverwriteColumn(e.RowIndex, mColumnInEdit);
				mColumnInEdit = null;
				mRowInEdit = -1;
			}
			else if (gridColumns.ContainsFocus)
			{
				mColumnInEdit = null;
				mRowInEdit = -1;
			}
		}

		private void gridColumns_RowDirtyStateNeeded(object sender, QuestionEventArgs e)
		{

		}

		private void gridColumns_CancelRowEdit(object sender, QuestionEventArgs e)
		{
			if (mRowInEdit == gridColumns.Rows.Count - 2 &&
				mRowInEdit == mTable.Columns.Count)
			{
				// If the user has canceled the edit of a newly created row, 
				// replace the corresponding Column object with a new, empty one.
				mColumnInEdit = new AgateColumn();
			}
			else
			{
				// If the user has canceled the edit of an existing row, 
				// release the corresponding Column object.
				mColumnInEdit = null;
				mRowInEdit = -1;
			}

		}

		private void gridColumns_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			if (e.Row.Index < mTable.Columns.Count)
			{
				// If the user has deleted an existing row, remove the 
				// corresponding Column object from the data store.
				mTable.RemoveColumn(e.Row.Index);
			}

			if (e.Row.Index == mRowInEdit)
			{
				// If the user has deleted a newly created row, release
				// the corresponding Column object. 
				mRowInEdit = -1;
				mColumnInEdit = null;
			}

		}

		AgateColumn colProperties;

		private void gridColumns_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < mTable.Columns.Count)
				colProperties = mTable.Columns[e.RowIndex];
			else
			{
				colProperties = mColumnInEdit;
			}

			if (colProperties == null)
			{
				cboTableLookup.Enabled = false;
				cboDisplayField.Enabled = false;
			}
			else if (string.IsNullOrEmpty(colProperties.TableLookup))
			{
				cboTableLookup.Enabled = true;
				cboTableLookup.SelectedIndex = 0;
				cboDisplayField.Enabled = false;
			}
			else
			{
				cboTableLookup.Enabled = true;
				
				AgateTable selTable = mDatabase.Tables[colProperties.TableLookup];

				cboTableLookup.SelectedItem = selTable;
				cboDisplayField.SelectedItem = selTable.Columns[colProperties.TableDisplayField];
			}

			if (colProperties == null)
				chkPrimaryKey.Enabled = false;
			else
			{
				chkPrimaryKey.Enabled = true;
				chkPrimaryKey.Checked = colProperties.PrimaryKey;
			}
		}

		private void cboTableLookup_SelectedIndexChanged(object sender, EventArgs e)
		{
			AgateColumn col = colProperties;
			if (col == null)
				col = mColumnInEdit;

			if (col == null)
				return;

			cboDisplayField.Items.Clear();

			AgateTable table = cboTableLookup.SelectedItem as AgateTable;
			if (table == null)
			{
				col.TableLookup = "";
				cboDisplayField.Enabled = false;
				return;
			}

			col.TableLookup = table.Name;

			foreach (var column in table.Columns)
			{
				cboDisplayField.Items.Add(column);
			}

			cboDisplayField.Enabled = true;
		}

		private void cboDisplayField_SelectedIndexChanged(object sender, EventArgs e)
		{
			AgateColumn col = colProperties;
			if (col == null)
				col = mColumnInEdit;

			if (col == null)
				return;

			AgateColumn selectedField = cboDisplayField.SelectedItem as AgateColumn;

			if (selectedField == null)
				col.TableDisplayField = "";
			else
				col.TableDisplayField = selectedField.Name;
		}

		private void chkPrimaryKey_CheckedChanged(object sender, EventArgs e)
		{
			AgateColumn col = colProperties;
			if (col == null)
				col = mColumnInEdit;

			if (col == null)
				return;

			col.PrimaryKey = chkPrimaryKey.Checked;
		}
	}
}
