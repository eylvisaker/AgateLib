using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Data;

namespace DatabaseEditor
{
	class TestData
	{
		public void Run()
		{
			try
			{
				AgateDatabase dbase1 = AgateDatabase.FromFile("test.zip");
				dbase1.LoadAllTables();
				dbase1.Dispose();
			}
			catch (System.IO.FileNotFoundException)
			{ }

			Table tbl = new Table();
			tbl.Name = "TestTable";

			for (int i = 0; i < 10; i++)
			{
				Column c = new Column { Name = "Column" + i };
				tbl.AddColumn(c);

				if (i == 0)
				{
					c.IsPrimaryKey = true;
					c.FieldType = FieldType.AutoNumber;
				}
			}

			for (int j = 0; j < 10; j++)
			{
				Row row = new Row(tbl);
	
				for (int i = 0; i < 10; i++)
				{
					if (tbl.Columns[i].FieldType == FieldType.AutoNumber)
						continue;

					row[tbl.Columns[i]] = (j * 100 + i).ToString();
				}

				tbl.Rows.Add(row);
			}

			AgateDatabase dbase = new AgateDatabase();
			dbase.Tables.Add(tbl);

			AgateDataLib.DatabaseWriter writer = new AgateDataLib.DatabaseWriter();

			writer.Database = dbase;
			writer.WriteData("test.zip");
		}
	}
}
