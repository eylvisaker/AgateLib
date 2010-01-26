using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;

namespace AgateLib.Data
{
	public class Table : IXleSerializable 
	{
		string mName;
		RowList mRows;
		ColumnDictionary mColumns;

		#region --- Construction and Serialization ---

		public Table()
		{
			mColumns = new ColumnDictionary(this);
			mRows = new RowList(this);
		}

		internal static Table FromStream(Stream stream)
		{
			XleSerializer ser = new XleSerializer(typeof(Table));

			return (Table)ser.Deserialize(stream);
		}

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Name", mName);
			info.Write("Version", "0.4.0");
			info.Write("Columns", mColumns.ColumnList);
			info.Write("Rows", RowString());
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mName = info.ReadString("Name");

			string version = info.ReadString("Version");

			if (version == "0.4.0")
			{
				mColumns = new ColumnDictionary(this, info.ReadList<Column>("Columns"));
				mRows = new RowList(this, ReadRows(info.ReadString("Rows")));
			}
			else
				throw new AgateDatabaseException("Unsupported database version.");
		}

		private string RowString()
		{
			StringBuilder b = new StringBuilder();

			mRows.ForEach(x => b.AppendLine(x.ToString()));

			return b.ToString();
		}

		static readonly char[] lineSplitChars = new char[] { '\n', '\r' };

		private List<Row> ReadRows(string rows)
		{
			List<Row> retval = new List<Row>();

			string[] lines = rows.Split(lineSplitChars, StringSplitOptions.RemoveEmptyEntries);

			foreach (string line in lines)
			{
				string[] data = DataHelper.Split(line);

				Row row = new Row(this);

				int i = 0;
				foreach (var column in Columns)
				{
					row.WriteWithoutValidation(column, data[i]);
					i++;
				}

				retval.Add(row);
			}

			return retval;
		}

		#endregion

		public string Name
		{
			get { return mName; }
			set
			{
				AssertIsValidName(value);

				mName = value;
			}
		}

		private void AssertIsValidName(string value)
		{
			if (IsValidTableName(value))
				return;

			throw new ArgumentException("Invalid name.  Table name should be a valid C# or VB identifier.");
		}

		public static bool IsValidTableName(string value)
		{
			return DataHelper.IsValidIdentifier(value);
		}

		public ColumnDictionary Columns
		{
			get { return mColumns; }
		}
		public RowList Rows
		{
			get { return mRows; }
		}

		public void AddColumn(Column col)
		{
			mColumns.Add(col);

			mRows.ForEach(x => x.ValidateData(this));
		}

		public override string ToString()
		{
			StringBuilder b = new StringBuilder();

			b.Append("Name:");
			b.AppendLine(Name);

			foreach (var column in mColumns)
			{
				b.AppendLine(column.ToString());
			}

			b.AppendLine("Rows:");

			foreach (var row in mRows)
			{
				b.AppendLine(row.ToString());
			}

			return b.ToString();
		}

		internal void Validate()
		{
			foreach (var row in mRows)
				row.ValidateData(this);
		}


		private static void ReadRows(Table table, StreamReader r)
		{
			while (r.EndOfStream == false)
			{
				string line = r.ReadLine();

				string[] data = DataHelper.Split(line);

				Row row = new Row(table);

				int i = 0;
				foreach (var column in table.Columns)
				{
					row[column] = data[i];
					i++;
				}

				table.Rows.Add(row);
			}
		}

		public void RemoveColumn(int index)
		{
			throw new NotImplementedException();
		}

		public void OverwriteColumn(int index, Column newColumn)
		{
			Column old = Columns[index];

			Columns[index] = newColumn;

			if (old.Name != newColumn.Name)
			{
				Rows.OnColumnNameChange(old.Name, newColumn.Name);
			}

			if (old.FieldType != newColumn.FieldType)
			{
				try
				{
					Validate();
				}
				catch
				{
					// validation of the data failed, so
					// undo the column change and rethrow.
					Columns[index] = old;

					Rows.OnColumnNameChange(newColumn.Name, old.Name);

					throw;
				}
			}

		}
	}
}
