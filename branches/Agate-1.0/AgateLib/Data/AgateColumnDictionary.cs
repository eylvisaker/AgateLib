//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
