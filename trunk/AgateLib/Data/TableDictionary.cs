using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
	public class TableDictionary : IEnumerable<Table>, IDisposable 
	{
		List<Table> mTables = new List<Table>();
		AgateDatabase mParentDatabase;
		List<string> mUnloadedTables = new List<string>();

		internal TableDictionary(AgateDatabase parentDatabase)
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

		public Table this[string name]
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

		private void LoadTable(string name)
		{
			string filename = string.Format("data/{0}.txt", name);

			using (Stream r = FileProvider.OpenRead(filename))
			{
				Table tbl = Table.FromStream(r);

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

		public Table this[int index]
		{
			get { return mTables[index]; }
		}

		public void Add(Table tbl)
		{
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


		#region IEnumerable<Table> Members

		public IEnumerator<Table> GetEnumerator()
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


	}
}
