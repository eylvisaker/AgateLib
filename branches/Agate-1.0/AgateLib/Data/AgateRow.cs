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
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
	/// <summary>
	/// Class which contains a row of data in a table of an AgateDatabase object.
	/// Data added to this class is validated against the data types for the columns
	/// in the table.
	/// </summary>
	public class AgateRow
	{
		Dictionary<string, string> mValues = new Dictionary<string, string>();
		AgateTable mParentTable;

		/// <summary>
		/// Constructs a new AgateRow object representing a single instance
		/// of the data.
		/// </summary>
		/// <param name="parentTable">The table into which the row will be added.
		/// This is required to validate data that gets added to the row.</param>
		public AgateRow(AgateTable parentTable)
		{
			this.mParentTable = parentTable;

			foreach (var column in parentTable.Columns)
			{
				mValues[column.Name] = null;
			}
		}

		/// <summary>
		/// Creates a deep copy of the AgateRow object.
		/// </summary>
		/// <returns></returns>
		public AgateRow Clone()
		{
			AgateRow retval = new AgateRow(mParentTable);

			foreach (var value in mValues)
				retval.mValues[value.Key] = value.Value;

			return retval;
		}
		/// <summary>
		/// Gets the AgateTable object that this row belongs to or will be added to.
		/// </summary>
		public AgateTable ParentTable
		{
			get { return mParentTable; }
			internal set
			{
				mParentTable = value;

				if (mParentTable != null)
				{
					ValidateData(mParentTable);
				}
			}
		}

		/// <summary>
		/// Gets or sets the field value for this row for the specified column.
		/// </summary>
		/// <param name="column">The column which indexes the data.</param>
		/// <returns></returns>
		public string this[AgateColumn column]
		{
			get { return mValues[column.Name]; }
			set
			{
				if (column.FieldType == FieldType.AutoNumber)
					throw new AgateDatabaseException("Cannot write to autonumber field.");

				string oldValue = mValues[column.Name];
				mValues[column.Name] = value;

				try
				{
					ValidateTypeOrThrow(column);
				}
				catch
				{
					mValues[column.Name] = oldValue;
					throw;
				}
			}
		}
		/// <summary>
		/// Gets or sets the field value for this row for the specified column.
		/// </summary>
		/// <param name="column">The name of the column which indexes the data.</param>
		/// <returns></returns>
		public string this[string column]
		{
			get { return mValues[column]; }
			set
			{
				this[mParentTable.Columns[column]] = value;
				
				mValues[column] = value;
			}
		}

		internal void WriteWithoutValidation(AgateColumn column, string value)
		{
			mValues[column.Name] = value;
		}

		/// <summary>
		/// Gets a string representation of the row.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder b = new StringBuilder();
			int count = 0;

			foreach (var column in mParentTable.Columns)
			{
				string value = AgateDataHelper.FixString(this[column.Name]);
				
				if (count > 0)
					b.Append(",");
				
				b.Append(value);

				count++;
			}

			return b.ToString();
		}

		private void ValidateTypeOrThrow(AgateColumn column)
		{
			if (mValues.ContainsKey(column.Name) == false ||
				string.IsNullOrEmpty(this[column]))
			{
				switch (column.FieldType)
				{
					case FieldType.Int16:
					case FieldType.Int32:
					case FieldType.SByte:
					case FieldType.Single:
					case FieldType.Decimal:
					case FieldType.Boolean:
					case FieldType.Byte:
					case FieldType.DateTime:
					case FieldType.Double:
					case FieldType.UInt16:
					case FieldType.UInt32:
					case FieldType.String:
						mValues[column.Name] = column.DefaultValue;
						break;
				}
			}

			Convert.ChangeType(mValues[column.Name], column.FieldTypeDataType);
		}

		internal void ValidateData(AgateTable agateTable)
		{
			foreach (var column in agateTable.Columns)
			{
				if (column.FieldType == FieldType.AutoNumber &&
					(mValues.ContainsKey(column.Name) == false ||
					mValues[column.Name] == null))
				{
					int value = column.NextAutoIncrementValue;
					column.IncrementNextAutoIncrementValue();

					mValues[column.Name] = value.ToString();
				}

				if (mValues.ContainsKey(column.Name))
				{
					ValidateTypeOrThrow(column);
				}
				else 
				{
					mValues.Add(column.Name, null);
				}

				if (column.PrimaryKey)
				{
					var matches = from x in agateTable.Rows
								  where x != null && x[column] == this[column]
								  select x;

					List<AgateRow> l = matches.ToList();
					l.Remove(this);

					if (l.Count > 0)
					{
						throw new AgateDatabaseException("The primary key is alread present.");
					}
				}
			}
		}


		internal void OnColumnNameChange(string oldName, string newName)
		{
			string value = mValues[oldName];
			mValues[newName] = value;

			mValues.Remove(oldName);
		}
		internal void OnDeleteColumn(string text)
		{
			mValues.Remove(text);
		}
	}
}
