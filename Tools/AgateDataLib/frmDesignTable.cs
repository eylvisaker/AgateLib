using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AgateLib.Data;

namespace AgateDataLib
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

		Table mTable;
		Column mColumnInEdit;
		int mRowInEdit = -1;

		public Table TheTable
		{
			get { return mTable; }
			set
			{
				mTable = value;

				gridColumns.RowCount = mTable.Columns.Count+1;
			}
		}

		internal static void EditColumns(AgateLib.Data.Table mTable)
		{
			frmDesignTable d = new frmDesignTable();

			d.TheTable = mTable;

			d.ShowDialog();
		}

		private void gridColumns_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			if (e.RowIndex == gridColumns.RowCount - 1) return;

			Column col = null;

			if (e.RowIndex == mRowInEdit)
				col = mColumnInEdit;
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
			Column col = null;

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
			this.mColumnInEdit = new Column();
			this.mRowInEdit = gridColumns.Rows.Count - 1;
		}

		private void gridColumns_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			// Save row changes if any were made and release the edited 
			// Column object if there is one.
			if (e.RowIndex >= mTable.Columns.Count &&
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
				mColumnInEdit = new Column();
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
	}
}
