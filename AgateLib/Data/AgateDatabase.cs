using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;

namespace AgateLib.Data
{
	/// <summary>
	/// AgateDatabase implements a basic cross-platform relational database.
	/// </summary>
	/// <remarks>
	/// This class is meant to meet not terribly complex needs to 
	/// retrieve type safe data stored in a relational database format at runtime.
	/// The data is entirely loaded into memory when a database is loaded, thus
	/// you would not want to use this to parse through a gigabyte of data.
	/// It also does not handle any of the "usual" things you get with a real database system,
	/// for instance there is no support for concurrent access.
	/// There is no SQL query engine, however LINQ should be adequate
	/// for any query needs.
	/// </remarks>
	public class AgateDatabase : IDisposable , IXleSerializable 
	{
		private TableDictionary mTables;

		public AgateDatabase()
		{
			mTables = new TableDictionary(this);
		}
		public static AgateDatabase FromFile(string filename)
		{
			AgateDatabase db = ReadDatabase(new AgateLib.Utility.ZipFileProvider(filename));
			db.mTables.OwnFileProvider = true;

			return db;
		}
		public static AgateDatabase FromProvider(IFileProvider provider)
		{
			return ReadDatabase(provider);
		}

		public void Dispose()
		{
			((IDisposable)mTables).Dispose();
		}


		private static AgateDatabase ReadDatabase(IFileProvider provider)
		{
			XleSerializer ser = new XleSerializer(typeof(AgateDatabase));

			using (Stream x = provider.OpenRead("catalog.txt"))
			{
				AgateDatabase retval = (AgateDatabase)ser.Deserialize(x);

				retval.mTables.FileProvider = provider;

				return retval;
			}
		}


		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Version", "0.4.0");
			info.Write("CodeNamespace", CodeNamespace);

			info.Write("Tables", TableList.ToList());
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			string version = info.ReadString("Version");

			if (version == "0.4.0")
			{
				List<string> tables = info.ReadList<string>("Tables");
				mTables.AddUnloadedTable(tables);

				CodeNamespace = info.ReadString("CodeNamespace", null);
			}
			else
				throw new AgateDatabaseException("Unsupported database version.");

		}

		#endregion

		public string CodeNamespace { get; set; }

		private IEnumerable<string> TableList
		{
			get
			{
				return from x in mTables select x.Name;
			}
		}

		/// <summary>
		/// Forces all tables to load into memory, rather than 
		/// lazily loading them when requested.
		/// </summary>
		public void LoadAllTables()
		{
			mTables.LoadAllTables();

			mTables.DisposeFileHandle();
		}


		/// <summary>
		/// Gets the dictionary of tables in the database.
		/// </summary>
		public TableDictionary Tables
		{
			get { return mTables; }
		}

		public string CatalogString()
		{
			StringBuilder b = new StringBuilder();

			b.AppendLine("Version:0.4.0");

			foreach (var table in mTables)
			{
				b.Append("Table:");
				b.Append(table.Name);
				b.AppendLine();
			}

			return b.ToString();
		}

		/// <summary>
		/// Validates the database to make sure it is correctly formed.
		/// An AgateDatabaseException is thrown if it is not.
		/// </summary>
		public void Validate()
		{
			int errorCount = 0;
			StringBuilder errors = new StringBuilder();

			foreach (var table in mTables)
			{
				
				try
				{
					table.Validate();
				}
				catch (AgateDatabaseException e)
				{
					errorCount += e.ErrorCount;
					errors.Append(e.Message);
				}
			}

			if (errorCount > 0)
			{
				throw new AgateDatabaseException(
					"There {0} {1} error{2} in validating the database:{3}{4}",
					errorCount == 1 ? "was" : "were",
					errorCount,
					errorCount == 1 ? "" : "s",
					Environment.NewLine,
					errors);
			}
		}
	}
}
