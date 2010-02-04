using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;

namespace AgateLib.Data
{
	/// <summary>
	/// Container class for columns in an AgateTable object.
	/// </summary>
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

		/// <summary>
		/// Gets a column by the specified name.
		/// </summary>
		/// <param name="name">Name of the column to get.</param>
		/// <returns></returns>
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
		/// <summary>
		/// Gets a column by its numerical index.
		/// </summary>
		/// <param name="index">Index of the column to get.</param>
		/// <returns></returns>
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

		/// <summary>
		/// Returns a string representation of the AgateColumnDictionary object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "Columns: " + mColumns.Count;
		}

		internal List<AgateColumn> ColumnList
		{
			get { return mColumns; }
		}

		/// <summary>
		/// Returns the number of columns in the AgateColumnDictionary.
		/// </summary>
		public int Count
		{
			get { return mColumns.Count; }
		}

		/// <summary>
		/// Gets the column which acts as the primary key for the table.
		/// If no column is marked as the primary key, null is returned.
		/// </summary>
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

		#region --- IEnumerable<AgateColumn> Members ---

		/// <summary>
		/// Enumerates the columns.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<AgateColumn> GetEnumerator()
		{
			return mColumns.GetEnumerator();
		}

		#endregion
		#region --- IEnumerable Members ---

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion


		/// <summary>
		/// Sorts the columns by their display index.
		/// </summary>
		public void SortByDisplayIndex()
		{
			mColumns.Sort((x, y) => x.DisplayIndex.CompareTo(y.DisplayIndex));
		}
	}
}
