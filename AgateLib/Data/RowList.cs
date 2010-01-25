using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;

namespace AgateLib.Data
{
	public class RowList : IList<Row> 
	{
		Table mParentTable;
		List<Row> mRows = new List<Row>();

		private RowList() { }
		internal RowList(Table parentTable)
		{
			mParentTable = parentTable;
		}
		internal RowList(Table parentTable, List<Row> rows)
		{
			mParentTable = parentTable;
			mRows = rows;
		}

		internal Table ParentTable
		{
			get { return mParentTable; }
			set { mParentTable = value; }
		}


		public void ForEach(Action<Row> action)
		{
			mRows.ForEach(action);
		}

		public override string ToString()
		{
			return "Rows: " + mRows.Count;
		}

		#region IList<AgateRow> Members

		public int IndexOf(Row item)
		{
			return mRows.IndexOf(item);
		}

		public void Insert(int index, Row item)
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

		public Row this[int index]
		{
			get
			{
				return mRows[index];
			}
			set
			{
				Row old = mRows[index];

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

		public void Add(Row row)
		{
			row.ParentTable.Rows.Remove(row);
			row.ParentTable = mParentTable;

			mRows.Add(row);
		}

		public void Clear()
		{
			mRows.Clear();
		}

		public bool Contains(Row item)
		{
			return mRows.Contains(item);
		}

		public void CopyTo(Row[] array, int arrayIndex)
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

		public bool Remove(Row item)
		{
			return mRows.Remove(item);
		}

		#endregion

		#region IEnumerable<AgateRow> Members

		public IEnumerator<Row> GetEnumerator()
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

	}
}
