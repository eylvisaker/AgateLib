﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
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
				AgateLib.Utility.ZipFileProvider zip = FileProvider as AgateLib.Utility.ZipFileProvider;

				if (zip != null)
				{
					zip.Dispose();
				}
			}
		}

		internal IFileProvider FileProvider { get; set; }
		internal bool OwnFileProvider { get; set; }

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

				var result = mTables.First(x => x.Name == name);

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
		public AgateTable this[int index]
		{
			get { return mTables[index]; }
		}

		private void LoadTable(string name)
		{
			string filename = string.Format("data/{0}.txt", name);

			using (Stream r = FileProvider.OpenRead(filename))
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


		public void Add(AgateTable tbl)
		{
			if (tbl == null)
				throw new ArgumentNullException("tbl", "Passed table cannot be null.");

			if (mTables.Any(x => x.Name == tbl.Name))
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
		public override string ToString()
		{
			return "Tables: " + mTables.Count;
		}

		public bool ContainsTable(string name)
		{
			if (mUnloadedTables.Any(x => string.Compare(x, name, true) == 0))
				return true;

			if (mTables.Any(x => string.Compare(x.Name, name, true) == 0))
				return true;

			return false;
		}



		#region IEnumerable<Table> Members

		public IEnumerator<AgateTable> GetEnumerator()
		{
			LoadAllTables();

			return mTables.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region ICollection<AgateTable> Members


		public void Clear()
		{
			mTables.Clear();
		}

		public bool Contains(AgateTable item)
		{
			return mTables.Contains(item);
		}

		public void CopyTo(AgateTable[] array, int arrayIndex)
		{
			mTables.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return mTables.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(AgateTable table)
		{
			return mTables.Remove(table);
		}

		#endregion
	}
}
