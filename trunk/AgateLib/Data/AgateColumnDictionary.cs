using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;

namespace AgateLib.Data
{
	public class AgateColumnDictionary : IEnumerable<AgateColumn>
	{
		AgateTable mParentTable;
		List<AgateColumn> mColumns = new List<AgateColumn>();

		private AgateColumnDictionary() { }
		internal AgateColumnDictionary(AgateTable parentTable)
		{
			mParentTable = parentTable;
		}
		internal AgateColumnDictionary(AgateTable parentTable, List<AgateColumn> columns)
		{
			mParentTable = parentTable;
			mColumns = columns;
		}
		internal AgateTable ParentTable
		{
			get { return mParentTable; }
			set { mParentTable = value; }
		}

		public AgateColumn this[string name]
		{
			get
			{
				var result = mColumns.FirstOrDefault(x => x.Name == name);

				if (result == null)
					throw new ArgumentException("Column does not exist.");

				return result;
			}
		}
		public AgateColumn this[int index]
		{
			get { return mColumns[index]; }
			internal set
			{
				mColumns[index] = value; 
			}
		}

		internal void Add(AgateColumn col)
		{
			if (col == null)
				throw new ArgumentNullException("Cannot add a null column.");

			if (mColumns.Any(x => x.Name == col.Name))
				throw new ArgumentException("Column " + col.Name + " already exists.");

			mColumns.Add(col);
		}

		internal void Insert(int newIndex, AgateColumn col)
		{
			if (col == null)
				throw new ArgumentNullException("Cannot add a null column.");
			if (mColumns.Any(x => x.Name == col.Name))
				throw new ArgumentException("Column " + col.Name + " already exists.");

			mColumns.Insert(newIndex, col);
		}

		public override string ToString()
		{
			return "Columns: " + mColumns.Count;
		}

		internal List<AgateColumn> ColumnList
		{
			get { return mColumns; }
		}

		public int Count
		{
			get { return mColumns.Count; }
		}

		public AgateColumn PrimaryKeyColumn
		{
			get
			{
				return mColumns.FirstOrDefault(x => x.PrimaryKey); 
			}
		}

		internal void Remove(int index)
		{
			mColumns.RemoveAt(index);
		}

		#region IEnumerable<AgateColumn> Members

		public IEnumerator<AgateColumn> GetEnumerator()
		{
			return mColumns.GetEnumerator();
		}

		#endregion
		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion



		public void SortByDisplayIndex()
		{
			mColumns.Sort((x, y) => x.DisplayIndex.CompareTo(y.DisplayIndex));
		}
	}
}
