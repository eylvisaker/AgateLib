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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
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
	/// Container class for AgateRow objects in a table.
	/// </summary>
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

		/// <summary>
		/// Performs the specified action for each row in the list.
		/// </summary>
		/// <param name="action">The System.Action&lt;T&gt; delegate to perform on 
		/// each row.</param>
		public void ForEach(Action<AgateRow> action)
		{
			mRows.ForEach(action);
		}
		/// <summary>
		/// Sorts rows in descending order for the specified column.
		/// </summary>
		/// <param name="col">The column whose data is to be sorted on.</param>
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
		/// <summary>
		/// Sorts rows in ascending order for the specified column.
		/// </summary>
		/// <param name="col">The column whose data is to be sorted on.</param>
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

		/// <summary>
		/// Returns a string representation of the AgateRowList object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "Rows: " + mRows.Count;
		}

		#region	--- IList<AgateRow> Members ---

		/// <summary>
		/// Gets the index of the specified row.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(AgateRow item)
		{
			return mRows.IndexOf(item);
		}
		/// <summary>
		/// Inserts a new row into the table.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, AgateRow item)
		{
			item.ValidateData(mParentTable);
			item.ParentTable = mParentTable;

			mRows.Insert(index, item);
		}
		/// <summary>
		/// Removes a row by its index.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			this[index].ParentTable = null;
			mRows.RemoveAt(index);
		}
		/// <summary>
		/// Gets or sets a row by its index.
		/// The data in the row is validated when setting.  An exception
		/// is thrown if the data validation fails.
		/// </summary>
		/// <param name="index">Index of the row.</param>
		/// <returns></returns>
		public AgateRow this[int index]
		{
			get { return mRows[index]; }
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

		#region --- ICollection<AgateRow> Members ---

		/// <summary>
		/// Adds a row to the AgateRowList.
		/// </summary>
		/// <param name="row"></param>
		public void Add(AgateRow row)
		{
			if (row == null)
				throw new ArgumentNullException("Cannot add a null row.");

			row.ParentTable.Rows.Remove(row);
			row.ParentTable = mParentTable;

			mRows.Add(row);
		}
		/// <summary>
		/// Removes all the rows.
		/// </summary>
		public void Clear()
		{
			mRows.Clear();
		}
		/// <summary>
		/// Returns true if the specified row is in the table.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(AgateRow item)
		{
			return mRows.Contains(item);
		}

		void ICollection<AgateRow>.CopyTo(AgateRow[] array, int arrayIndex)
		{
			mRows.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of rows in the table.
		/// </summary>
		public int Count
		{
			get { return mRows.Count; }
		}

		bool ICollection<AgateRow>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes a row from the table.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(AgateRow item)
		{
			return mRows.Remove(item);
		}

		#endregion
		#region --- IEnumerable<AgateRow> Members ---

		/// <summary>
		/// Enumerates the rows.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<AgateRow> GetEnumerator()
		{
			return mRows.GetEnumerator();
		}

		#endregion
		#region --- IEnumerable Members ---

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
