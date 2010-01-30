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
	public partial class TableEditor : UserControl
	{
		AgateDatabase mDatabase;
		AgateTable mTable;

		AgateRow mEditingRow;
		int mEditingRowIndex = -1;

		public TableEditor()
		{
			InitializeComponent();
		}

		public AgateDatabase Database
		{
			get { return mDatabase; }
			set { mDatabase = value; 
				AgateTable = null;
			}
		}
		public AgateTable AgateTable
		{
			get { return mTable; }
			set
			{
				mTable = value;
				TableReset();
			}
		}

		internal void FinalizeData()
		{
			gridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
			gridView.EndEdit();

			DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(gridView.CurrentCell.ColumnIndex, gridView.CurrentCell.RowIndex);

			gridView_RowValidated(gridView, e);
		}

		private void TableReset()
		{
			gridView.SuspendLayout();
			gridView.Columns.Clear();

			if (mTable == null)
			{
				gridView.ResumeLayout();

				return;
			}
			
			int index = 0;
			foreach (var column in mTable.Columns)
			{
				DataGridViewColumn col = new DataGridViewColumn();

				col.Name = column.Name;
				col.ReadOnly = column.FieldType == FieldType.AutoNumber;

				if (string.IsNullOrEmpty(column.TableLookup))
				{
					col.CellTemplate = new DataGridViewTextBoxCell();
				}

				if (column.ColumnWidth > 10)
					col.Width = column.ColumnWidth;

				gridView.Columns.Add(col);

				index++;
			}

			UpdateGridViewRowCount();
			gridView.ResumeLayout();
		}

		private void UpdateGridViewRowCount()
		{
			gridView.RowCount = mTable.Rows.Count + 1;
		}

		AgateColumn GetColumn(int columnIndex)
		{
			return mTable.Columns[columnIndex];
		}
		string ColumnName(int columnIndex)
		{
			return GetColumn(columnIndex).Name;
		}

		private void gridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex == gridView.RowCount - 1)
				return;

			AgateRow row = null;

			if (e.RowIndex == mEditingRowIndex)
				row = mEditingRow;
			else
				row =  mTable.Rows[e.RowIndex];

			e.Value = row[ColumnName(e.ColumnIndex)];
		}
		private void gridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
		{
			AgateRow row = null;

			// Store the reference to the row being edited.
			if (e.RowIndex < mTable.Rows.Count)
			{
				// the user is editing an existing row, so make a copy of
				// it in case they want to cancel.
				if (this.mEditingRow == null)
				{
					this.mEditingRow = mTable.Rows[e.RowIndex].Clone();
				}

				mEditingRowIndex = e.RowIndex;
			}
			
			row = this.mEditingRow;

			try
			{
				row[ColumnName(e.ColumnIndex)] = e.Value.ToString();
			}
			catch (FormatException)
			{
				MessageBox.Show(this,
					"The value you entered for this field is not valid." + Environment.NewLine +
					"The field data type is " + mTable.Columns[e.ColumnIndex].FieldType.ToString() + ".",
					"Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		private void gridView_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
		{
			mEditingRow = new AgateRow(mTable);
			mEditingRowIndex = gridView.Rows.Count - 1;
		}
		private void gridView_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= mTable.Rows.Count &&
				e.RowIndex != gridView.Rows.Count-1)
			{
				mTable.Rows.Add(mEditingRow);
				mEditingRow = null;
				mEditingRowIndex = -1;
			}
			else if (mEditingRow != null &&
				e.RowIndex < mTable.Rows.Count)
			{
				mTable.Rows[e.RowIndex] = mEditingRow;
				mEditingRow = null;
				mEditingRowIndex = -1;
			}
			else if (gridView.ContainsFocus)
			{
				mEditingRow = null;
				mEditingRowIndex = -1;
			}
		}
		private void gridView_RowDirtyStateNeeded(object sender, QuestionEventArgs e)
		{
		}
		private void gridView_CancelRowEdit(object sender, QuestionEventArgs e)
		{
			if (mEditingRowIndex == gridView.Rows.Count - 2 &&
				mEditingRowIndex == mTable.Rows.Count)
			{
				// If the user has canceled the edit of a newly created row, 
				// replace the corresponding Customer object with a new, empty one.
				this.mEditingRow = new AgateRow(mTable);
			}
			else
			{
				// If the user has canceled the edit of an existing row, 
				// release the corresponding Customer object.
				this.mEditingRow = null;
				this.mEditingRowIndex = -1;
			}

		}
		private void gridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			if (e.Row.Index < mTable.Rows.Count)
			{
				mTable.Rows.RemoveAt(e.Row.Index);
			}

			if (e.Row.Index == mEditingRowIndex)
			{
				// user has deleted a newly created row.
				mEditingRowIndex = -1;
				mEditingRow = null;
			}
		}
		private void gridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}
		private void gridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
		{
			mTable.Columns[e.Column.Index].ColumnWidth = e.Column.Width;
		}

		private void editColumnsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmDesignTable.EditColumns(mTable);

			TableReset();
		}


	}
}
