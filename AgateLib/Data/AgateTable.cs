using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;

namespace AgateLib.Data
{
	public class AgateTable : IXleSerializable 
	{
		string mName;
		AgateRowList mRows;
		AgateColumnDictionary mColumns;

		#region --- Construction and Serialization ---

		public AgateTable()
		{
			mColumns = new AgateColumnDictionary(this);
			mRows = new AgateRowList(this);
		}


		public AgateTable Clone()
		{
			XleSerializer ser = new XleSerializer(typeof(AgateTable));

			MemoryStream ms = new MemoryStream();

			ser.Serialize(ms, this);

			ms.Position = 0;

			return FromStream(ms);
		}

		internal static AgateTable FromStream(Stream stream)
		{
			XleSerializer ser = new XleSerializer(typeof(AgateTable));

			return (AgateTable)ser.Deserialize(stream);
		}

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			mColumns.SortByDisplayIndex();

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
				mColumns = new AgateColumnDictionary(this, info.ReadList<AgateColumn>("Columns"));
				mRows = new AgateRowList(this, ReadRows(info.ReadString("Rows")));
			}
			else
				throw new AgateDatabaseException("Unsupported database version.");

			for (int i = 0; i < mColumns.Count; i++)
				mColumns[i].DisplayIndex = i;
		}

		private string RowString()
		{
			StringBuilder b = new StringBuilder();

			mRows.ForEach(x => b.AppendLine(x.ToString()));

			return b.ToString();
		}

		static readonly char[] lineSplitChars = new char[] { '\n', '\r' };

		private List<AgateRow> ReadRows(string rows)
		{
			List<AgateRow> retval = new List<AgateRow>();

			string[] lines = rows.Split(lineSplitChars, StringSplitOptions.RemoveEmptyEntries);

			foreach (string line in lines)
			{
				string[] data = AgateDataHelper.Split(line);

				AgateRow row = new AgateRow(this);

				int i = 0;
				foreach (var column in Columns)
				{
					string val = data[i];

					if (val.StartsWith("\"") && val.EndsWith("\""))
					{
						val = val.Substring(1, val.Length - 2);
					}

					row.WriteWithoutValidation(column, val);
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
			return AgateDataHelper.IsValidIdentifier(value);
		}

		public AgateColumnDictionary Columns
		{
			get { return mColumns; }
		}
		public AgateRowList Rows
		{
			get { return mRows; }
		}

		
		internal void Validate()
		{
			foreach (var row in mRows)
				row.ValidateData(this);
		}

		public void AddColumn(AgateColumn col)
		{
			mColumns.Add(col);

			if (col.FieldType == FieldType.AutoNumber)
			{

			}
			mRows.ForEach(x => x.ValidateData(this));
		}

		public void RemoveColumn(int index)
		{
			string text = mColumns[index].Name;

			foreach (var row in Rows)
			{
				row.OnDeleteColumn(text);
			}

			mColumns.Remove(index);
		}

		public void OverwriteColumn(int index, AgateColumn newColumn)
		{
			AgateColumn old = Columns[index];

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

					if (newColumn.FieldType == FieldType.AutoNumber)
					{
						int max = Rows.Max(x => int.Parse(x[newColumn]));

						newColumn.SetNextAutoIncrementValue(max + 1);
					}
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


		public void MoveColumn(int oldIndex, int newIndex)
		{
			AgateColumn col = mColumns[oldIndex];

			mColumns.Remove(oldIndex);
			mColumns.Insert(newIndex, col);
		}
	}
}
