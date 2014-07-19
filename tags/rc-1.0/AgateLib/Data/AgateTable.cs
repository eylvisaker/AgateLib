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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
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

namespace AgateLib.Data
{
	/// <summary>
	/// Class which represents a table in a database.
	/// </summary>
	public class AgateTable : IXleSerializable 
	{
		string mName;
		AgateRowList mRows;
		AgateColumnDictionary mColumns;

		#region --- Construction and Serialization ---

		/// <summary>
		/// Constructs an AgateTable object.
		/// </summary>
		public AgateTable()
		{
			mColumns = new AgateColumnDictionary(this);
			mRows = new AgateRowList(this);
		}

		/// <summary>
		/// Creates a deep copy of an AgateTable object.
		/// </summary>
		/// <returns></returns>
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
			info.Write("Version", "0.3.2");
			info.Write("Columns", mColumns.ColumnList);
			info.Write("Rows", RowString());
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mName = info.ReadString("Name");

			string version = info.ReadString("Version");

			if (version == "0.3.2")
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

		static readonly char[] LineSplitChars = new char[] { '\n', '\r' };

		private List<AgateRow> ReadRows(string rows)
		{
			List<AgateRow> retval = new List<AgateRow>();

			string[] lines = rows.Split(LineSplitChars, StringSplitOptions.RemoveEmptyEntries);

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

		/// <summary>
		/// Gets or sets the name of the table.  This must be a valid
		/// C# identifier, so it must start with a letter or underscore 
		/// and can contain only letters, numbers or underscores.
		/// </summary>
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

		/// <summary>
		/// Returns true if the specified string is valid as a table name.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsValidTableName(string value)
		{
			return AgateDataHelper.IsValidIdentifier(value);
		}

		/// <summary>
		/// Gets the list of columns in the table.
		/// </summary>
		public AgateColumnDictionary Columns
		{
			get { return mColumns; }
		}
		/// <summary>
		/// Gets the list of rows in the table.
		/// </summary>
		public AgateRowList Rows
		{
			get { return mRows; }
		}

		
		internal void Validate()
		{
			foreach (var row in mRows)
				row.ValidateData(this);
		}

		/// <summary>
		/// Adds a column to the table.
		/// </summary>
		/// <param name="col"></param>
		public void AddColumn(AgateColumn col)
		{
			mColumns.Add(col);

			if (col.FieldType == FieldType.AutoNumber)
			{

			}
			mRows.ForEach(x => x.ValidateData(this));
		}
		/// <summary>
		/// Removes a column from the table.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveColumn(int index)
		{
			string text = mColumns[index].Name;

			foreach (var row in Rows)
			{
				row.OnDeleteColumn(text);
			}

			mColumns.Remove(index);
		}
		/// <summary>
		/// Overwrites a column in the table.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="newColumn"></param>
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

					if (newColumn.FieldType == FieldType.AutoNumber && Rows.Count > 0)
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
		/// <summary>
		/// Moves a column in the table to a new index.
		/// </summary>
		/// <param name="oldIndex"></param>
		/// <param name="newIndex"></param>
		public void MoveColumn(int oldIndex, int newIndex)
		{
			AgateColumn col = mColumns[oldIndex];

			mColumns.Remove(oldIndex);
			mColumns.Insert(newIndex, col);
		}
	}
}
