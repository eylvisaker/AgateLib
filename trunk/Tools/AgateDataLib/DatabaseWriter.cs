using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib.Data;
using AgateLib.Serialization.Xle;
using Ionic.Zip;

namespace AgateDataLib
{
	public class DatabaseWriter
	{
		public DatabaseWriter()
		{
		}

		public DatabaseWriter(AgateDatabase database)
		{
			this.Database = database;
		}
		public AgateDatabase Database { get; set; }
 
		/// <summary>
		/// Writes the database to the specified file.
		/// The Database property must not be null, and its validate
		/// property must return without throwing an exception.
		/// </summary>
		/// <param name="filename">The output file to write to.</param>
		public void WriteData(string filename)
		{
			if (Database == null)
			{
				throw new InvalidOperationException("The database to write is empty.");
			}

			Database.LoadAllTables();
			Database.Validate();

			bool overwriting = File.Exists(filename);
			
			string tempfilename = Path.GetTempFileName();
					
			try
			{
				if (overwriting)
				{
					File.Copy(filename, tempfilename, true);
					File.Delete(filename);
				}

				using (ZipFile zip = new ZipFile(filename))
				{
					// serialize the catalog first.
					XleSerializer baseSer = new XleSerializer(typeof(AgateDatabase));
					MemoryStream ms = new MemoryStream();

					baseSer.Serialize(ms, Database);

					ms.Position = 0;

					ZipEntry catalog = zip.AddEntry("catalog.txt", ms);

					// now do each table
					XleSerializer tableSer = new XleSerializer(typeof(Table));

					foreach (var table in Database.Tables)
					{
						string name = string.Format("data/{0}.txt", table.Name);

						ms = new MemoryStream();
						tableSer.Serialize(ms, table);
						ms.Position = 0;

						ZipEntry f = zip.AddEntry(name, ms);
					}

					zip.Save();
				}
			}
			catch
			{
				if (overwriting)
				{
					File.Delete(filename);
					File.Copy(tempfilename, filename);
					File.Delete(tempfilename);
				}

				throw;
			}
		}
	}
}
