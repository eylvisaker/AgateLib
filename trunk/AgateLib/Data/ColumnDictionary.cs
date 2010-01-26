using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;

namespace AgateLib.Data
{
	public class ColumnDictionary : IEnumerable<Column>
	{
		Table mParentTable;
		List<Column> mColumns = new List<Column>();

		private ColumnDictionary() { }
		internal ColumnDictionary(Table parentTable)
		{
			mParentTable = parentTable;
		}
		internal ColumnDictionary(Table parentTable, List<Column> columns)
		{
			mParentTable = parentTable;
			mColumns = columns;
		}
		internal Table ParentTable
		{
			get { return mParentTable; }
			set { mParentTable = value; }
		}

		public Column this[string name]
		{
			get
			{
				var result = mColumns.First(x => x.Name == name);

				if (result == null)
					throw new ArgumentException("Column does not exist.");

				return result;
			}
		}
		public Column this[int index]
		{
			get { return mColumns[index]; }
			internal set
			{
				mColumns[index] = value; 
			}
		}

		internal void Add(Column col)
		{
			if (mColumns.Any(x => x.Name == col.Name))
				throw new ArgumentException("Column " + col.Name + " already exists.");

			mColumns.Add(col);
		}

		public override string ToString()
		{
			return "Columns: " + mColumns.Count;
		}

		internal List<Column> ColumnList
		{
			get { return mColumns; }
		}

		public int Count
		{
			get { return mColumns.Count; }
		}

		#region IEnumerable<AgateColumn> Members

		public IEnumerator<Column> GetEnumerator()
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


	}
}
