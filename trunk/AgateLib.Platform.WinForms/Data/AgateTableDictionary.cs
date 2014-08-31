using AgateLib.Platform.WinForms.IO;
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
using System.IO;
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
	/// <summary>
	/// Class which contains all the tables in the database.
	/// </summary>
	public class AgateTableDictionary : ICollection<AgateTable>, IDisposable
	{
		List<AgateTable> mTables = new List<AgateTable>();
		AgateDatabase mParentDatabase;
		List<string> mUnloadedTables = new List<string>();

		internal AgateTableDictionary(AgateDatabase parentDatabase)
		{
			mParentDatabase = parentDatabase;
		}

		void IDisposable.Dispose()
		{
			DisposeFileHandle();
		}


		internal void DisposeFileHandle()
		{
			if (OwnFileProvider)
			{
				var zip = FileProvider as ZipFileProvider;

				if (zip != null)
				{
					zip.Dispose();
				}
			}
		}

		internal IReadFileProvider FileProvider { get; set; }
		internal bool OwnFileProvider { get; set; }

		/// <summary>
		/// Gets a table in the database.
		/// </summary>
		/// <param name="name">The name of the table to load.  This is case-insensitive.</param>
		/// <returns></returns>
		public AgateTable this[string name]
		{
			get
			{
				bool loaded = false;

				if (mUnloadedTables.Contains(name))
				{
					loaded = true;
					LoadTable(name);
					mUnloadedTables.Remove(name);
				}

				var result = mTables.First(x => string.Compare(x.Name, name, true) == 0);

				if (result == null)
				{
					if (loaded)
						throw new AgateException(
							"BUG: Table was loaded but did not exist in database." +
							"Probably, the table is corrupt, but an exception should have been thrown.");

					throw new ArgumentException("Table does not exist.");
				}

				return result;
			}
		}
		/// <summary>
		/// Gets a table by its index in the database.
		/// </summary>
		/// <param name="index">Numerical index of the table.  Should be between
		/// 0 and Count-1.</param>
		/// <returns></returns>
		public AgateTable this[int index]
		{
			get { return mTables[index]; }
		}

		private void LoadTable(string name)
		{
			string filename = string.Format("data/{0}.txt", name);

			using (Stream r = FileProvider.OpenRead(filename).Result)
			{
				AgateTable tbl = AgateTable.FromStream(r);

				mTables.Add(tbl);
			}
		}


		internal void LoadAllTables()
		{
			foreach (var table in mUnloadedTables)
			{
				LoadTable(table);
			}

			mUnloadedTables.Clear();
		}

		/// <summary>
		/// Adds a table to the database.  The name of the added table
		/// must not conflict with a table already in the database.
		/// Table names are case-insensitive.
		/// </summary>
		/// <param name="tbl">The table to add.</param>
		public void Add(AgateTable tbl)
		{
			if (tbl == null)
				throw new ArgumentNullException("tbl", "Passed table cannot be null.");

			if (mTables.Any(x => 0 == string.Compare(x.Name, tbl.Name, true)))
				throw new ArgumentException("Table " + tbl.Name + " already exists.");

			mTables.Add(tbl);
		}

		internal void AddUnloadedTable(string name)
		{
			mUnloadedTables.Add(name);
		}
		internal void AddUnloadedTable(IEnumerable<string> tables)
		{
			mUnloadedTables.AddRange(tables);
		}
		/// <summary>
		/// Constructs a string representation of the table dictionary.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "Tables: " + mTables.Count;
		}

		/// <summary>
		/// Returns true if there is a table of the specified name in the database.
		/// Table names are case-insensitive.
		/// </summary>
		/// <param name="name">The name of the table to search for.</param>
		/// <returns></returns>
		public bool ContainsTable(string name)
		{
			if (mUnloadedTables.Any(x => string.Compare(x, name, true) == 0))
				return true;

			if (mTables.Any(x => string.Compare(x.Name, name, true) == 0))
				return true;

			return false;
		}



		#region --- IEnumerable<Table> Members ---

		/// <summary>
		/// Enumerates the tables.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<AgateTable> GetEnumerator()
		{
			LoadAllTables();

			return mTables.GetEnumerator();
		}

		#endregion
		#region --- IEnumerable Members ---

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
		#region --- ICollection<AgateTable> Members ---

		/// <summary>
		/// Removes all the tables.
		/// </summary>
		public void Clear()
		{
			mTables.Clear();
		}

		/// <summary>
		/// Returns true if the specified table is in the database.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(AgateTable item)
		{
			return mTables.Contains(item);
		}

		void ICollection<AgateTable>.CopyTo(AgateTable[] array, int arrayIndex)
		{
			mTables.CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Returns the number of tables in the database.
		/// </summary>
		public int Count
		{
			get { return mTables.Count; }
		}

		bool ICollection<AgateTable>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes the specified table from the database.
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		public bool Remove(AgateTable table)
		{
			return mTables.Remove(table);
		}

		#endregion
	}
}
