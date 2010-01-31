using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;

namespace AgateLib.Data
{
	public class AgateRowList : IList<AgateRow> 
	{
		AgateTable mParentTable;
		List<AgateRow> mRows = new List<AgateRow>();

		private AgateRowList() { }
		internal AgateRowList(AgateTable parentTable)
		{
			mParentTable = parentTable;
		}
		internal AgateRowList(AgateTable parentTable, List<AgateRow> rows)
		{
			mParentTable = parentTable;
			mRows = rows;
		}

		internal AgateTable ParentTable
		{
			get { return mParentTable; }
			set { mParentTable = value; }
		}


		public void ForEach(Action<AgateRow> action)
		{
			mRows.ForEach(action);
		}
		public void SortDescending(AgateColumn col)
		{
			if (col.IsNumeric)
			{
				mRows.Sort((x, y) => -decimal.Parse(x[col]).CompareTo(decimal.Parse(y[col])));
			}
			else
			{
				mRows.Sort((x, y) => -x[col].CompareTo(y[col]));
			}
		}
		public void SortAscending(AgateColumn col)
		{
			if (col.IsNumeric)
			{
				mRows.Sort((x, y) => decimal.Parse(x[col]).CompareTo(decimal.Parse(y[col])));
			}
			else
			{
				mRows.Sort((x, y) => x[col].CompareTo(y[col]));
			}
		}

		public override string ToString()
		{
			return "Rows: " + mRows.Count;
		}

		#region IList<AgateRow> Members

		public int IndexOf(AgateRow item)
		{
			return mRows.IndexOf(item);
		}

		public void Insert(int index, AgateRow item)
		{
			item.ValidateData(mParentTable);
			item.ParentTable = mParentTable;

			mRows.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			this[index].ParentTable = null;
			mRows.RemoveAt(index);
		}

		public AgateRow this[int index]
		{
			get
			{
				return mRows[index];
			}
			set
			{
				AgateRow old = mRows[index];

				try
				{
					mRows[index] = null;

					value.ValidateData(mParentTable);
				}
				catch
				{
					mRows[index] = old;
					throw;
				}

				mRows[index] = value;
			}
		}

		#endregion

		#region ICollection<AgateRow> Members

		public void Add(AgateRow row)
		{
			row.ParentTable.Rows.Remove(row);
			row.ParentTable = mParentTable;

			mRows.Add(row);
		}

		public void Clear()
		{
			mRows.Clear();
		}

		public bool Contains(AgateRow item)
		{
			return mRows.Contains(item);
		}

		public void CopyTo(AgateRow[] array, int arrayIndex)
		{
			mRows.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return mRows.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(AgateRow item)
		{
			return mRows.Remove(item);
		}

		#endregion

		#region IEnumerable<AgateRow> Members

		public IEnumerator<AgateRow> GetEnumerator()
		{
			return mRows.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion


		internal void OnColumnNameChange(string oldName, string newName)
		{
			foreach (var row in this)
			{
				row.OnColumnNameChange(oldName, newName);
			}
		}
	}
}
