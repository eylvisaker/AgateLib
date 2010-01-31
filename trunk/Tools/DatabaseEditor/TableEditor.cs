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
			set
			{
				mDatabase = value;
				Table = null;
			}
		}
		public AgateTable Table
		{
			get { return mTable; }
			set
			{
				mTable = value;
				TableRefresh();
			}
		}

		internal void FinalizeData()
		{
			gridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
			gridView.EndEdit();

			if (gridView.CurrentCell != null)
			{
				DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(gridView.CurrentCell.ColumnIndex, gridView.CurrentCell.RowIndex);

				gridView_RowValidated(gridView, e);
			}
		}

		public void TableRefresh()
		{
			gridView.SuspendLayout();
			gridView.Columns.Clear();

			if (mTable == null)
			{
				gridView.ResumeLayout();

				return;
			}

			mTable.Columns.SortByDisplayIndex();

			int index = 0;
			foreach (var column in mTable.Columns)
			{
				DataGridViewColumn col = new DataGridViewColumn();

				col.Tag = column;
				col.Name = column.Name;
				col.ReadOnly = column.FieldType == FieldType.AutoNumber;

				if (column.FieldType == FieldType.Boolean)
				{
					col.CellTemplate = new DataGridViewCheckBoxCell();
				}
				else if (string.IsNullOrEmpty(column.TableLookup) || string.IsNullOrEmpty(column.TableDisplayField))
				{
					col.CellTemplate = new DataGridViewTextBoxCell();
				}
				else
				{
					var cbo = new DataGridViewComboBoxCell();
					var table = Database.Tables[column.TableLookup];

					if (table == null)
					{
						OnStatusText(StatusTextIcon.Warning, "The lookup table " + table.Name + " does not exist.");

						col.CellTemplate = new DataGridViewTextBoxCell();
					}
					else
					{
						var primaryKey = table.Columns.PrimaryKeyColumn;
						if (primaryKey != null)
						{
							for (int i = 0; i < table.Rows.Count; i++)
							{
								cbo.Items.Add(new { ID = table.Rows[i][primaryKey], Name = table.Rows[i][column.TableDisplayField] });
							}
							cbo.DisplayMember = "Name";
							cbo.ValueMember = "ID";

							col.CellTemplate = cbo;
						}
						else
						{
							OnStatusText(StatusTextIcon.Warning, "The lookup table " + table.Name + " needs a primary key field.");
							col.CellTemplate = new DataGridViewTextBoxCell();
						}
					}
				}

				if (column.ColumnWidth > 10)
					col.Width = column.ColumnWidth;

				gridView.Columns.Add(col);

				index++;
			}

			UpdateGridViewRowCount();
			gridView.ResumeLayout();
		}

		private void OnStatusText(StatusTextIcon icon, string text)
		{
			if (StatusText != null)
				StatusText(this, new StatusTextEventArgs(icon, text));
		}
		public event EventHandler<StatusTextEventArgs> StatusText;

		private void OnSetDirtyFlag()
		{
			if (SetDirtyFlag != null)
				SetDirtyFlag(this, EventArgs.Empty);
		}
		public event EventHandler SetDirtyFlag;

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

		private void gridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			// swallow unnecessary error?
		}
		private void gridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			AgateRow row = null;

			if (e.RowIndex == mEditingRowIndex)
				row = mEditingRow;
			else if (e.RowIndex >= mTable.Rows.Count)
				return;
			else 
				row = mTable.Rows[e.RowIndex];

			string value = row[ColumnName(e.ColumnIndex)];

			if (mTable.Columns[ColumnName(e.ColumnIndex)].FieldType == FieldType.Boolean)
			{
				e.Value = bool.Parse(value);
			}
			else
			{
				e.Value = value;
			}
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
				string value = string.Empty;
				if (e.Value != null) value = e.Value.ToString();

				row[ColumnName(e.ColumnIndex)] = value;
			}
			catch (FormatException)
			{
				MessageBox.Show(this,
					"The value you entered for this field is not valid." + Environment.NewLine +
					"The field data type is " + mTable.Columns[e.ColumnIndex].FieldType.ToString() + ".",
					"Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			OnSetDirtyFlag();
		}
		private void gridView_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
		{
			mEditingRow = new AgateRow(mTable);
			mEditingRowIndex = gridView.Rows.Count - 1;
		}
		private void gridView_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			if (mEditingRow != null && 
				e.RowIndex >= mTable.Rows.Count &&
				e.RowIndex != gridView.Rows.Count - 1)
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

			OnSetDirtyFlag();
		}

		private void editColumnsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			frmDesignTable.EditColumns(Database, mTable);

			TableRefresh();

			OnSetDirtyFlag();
		}

		private void TableEditor_Load(object sender, EventArgs e)
		{
		}




		public void EditColumns()
		{
			frmDesignTable.EditColumns(Database, Table);

			TableRefresh();

			OnSetDirtyFlag();
		}

		internal void SortAscending()
		{
			if (CurrentColumn == null)
				return;

			Table.Rows.SortAscending(CurrentColumn);
			gridView.Invalidate();

			OnSetDirtyFlag();
		}

		internal void SortDescending()
		{
			if (CurrentColumn == null)
				return;

			Table.Rows.SortDescending(CurrentColumn);
			gridView.Invalidate();

			OnSetDirtyFlag();
		}

		AgateColumn CurrentColumn
		{
			get
			{
				if (gridView.CurrentCell.ColumnIndex > -1)
					return Table.Columns[gridView.CurrentCell.ColumnIndex];

				return null;
			}
		}

		private void gridView_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
		{
			AgateColumn col = e.Column.Tag as AgateColumn;

			col.DisplayIndex = e.Column.DisplayIndex;

			OnSetDirtyFlag();
		}
	}

	public class StatusTextEventArgs : EventArgs 
	{
		public string Text { get; private set; }
		public StatusTextIcon StatusTextIcon { get; private set; }

		public StatusTextEventArgs(StatusTextIcon icon, string text)
		{
			StatusTextIcon = icon;
			Text = text;
		}
	}

	public enum StatusTextIcon
	{
		Information,
		Warning,
		Error,
	}
}
