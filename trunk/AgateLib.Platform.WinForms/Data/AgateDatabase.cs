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
using AgateLib.Serialization.Xle;
using System.Threading.Tasks;

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
		private AgateTableDictionary mTables;

		/// <summary>
		/// Constructs a new AgateDatabase object.
		/// </summary>
		public AgateDatabase()
		{
			mTables = new AgateTableDictionary(this);
		}
		/// <summary>
		/// Loads an AgateDatabase object from a file on disk.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static AgateDatabase FromFile(string filename)
		{
			AgateDatabase db = ReadDatabase(new AgateLib.Platform.WinForms.IO.ZipFileProvider(filename)).Result;
			db.mTables.OwnFileProvider = true;

			return db;
		}
		/// <summary>
		/// Loads an AgateDatabase object from the specified file provider.
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public static AgateDatabase FromProvider(IReadFileProvider provider)
		{
			return ReadDatabase(provider).Result;
		}

		/// <summary>
		/// Destroys an AgateDatabase object.
		/// </summary>
		public void Dispose()
		{
			((IDisposable)mTables).Dispose();
		}

		private static async Task<AgateDatabase> ReadDatabase(IReadFileProvider provider)
		{
			XleSerializer ser = new XleSerializer(typeof(AgateDatabase));

			using (Stream x = await provider.OpenRead("catalog.txt"))
			{
				AgateDatabase retval = (AgateDatabase)ser.Deserialize(x);

				retval.mTables.FileProvider = provider;

				return retval;
			}
		}

		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Version", "0.3.2");
			info.Write("CodeNamespace", CodeNamespace);

			info.Write("Tables", TableList.ToList());
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			string version = info.ReadString("Version");

			if (version == "0.3.2")
			{
				List<string> tables = info.ReadList<string>("Tables");
				mTables.AddUnloadedTable(tables);

				CodeNamespace = info.ReadString("CodeNamespace", null);
			}
			else
				throw new AgateDatabaseException("Unsupported database version.");

		}

		#endregion

		/// <summary>
		/// Gets or sets the namespace that is used when code is generated
		/// from the AgateDatabase.
		/// </summary>
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
		public AgateTableDictionary Tables
		{
			get { return mTables; }
		}

		/// <summary>
		/// Gets the text that goes into catalog.txt in the database archive.
		/// </summary>
		/// <returns></returns>
		public string CatalogString()
		{
			StringBuilder b = new StringBuilder();

			b.AppendLine("Version:0.3.2");

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
