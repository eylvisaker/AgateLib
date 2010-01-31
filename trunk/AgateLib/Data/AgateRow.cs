using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
	public class AgateRow
	{
		Dictionary<string, string> mValues = new Dictionary<string, string>();
		AgateTable mParentTable;

		public AgateRow(AgateTable parentTable)
		{
			this.mParentTable = parentTable;

			foreach (var column in parentTable.Columns)
			{
				mValues[column.Name] = null;
			}
		}

		public AgateRow Clone()
		{
			AgateRow retval = new AgateRow(mParentTable);

			foreach (var value in mValues)
				retval.mValues[value.Key] = value.Value;

			return retval;
		}
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
		/// Shortcut for this[column.Name].
		/// </summary>
		/// <param name="column"></param>
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
		public string this[string key]
		{
			get { return mValues[key]; }
			set
			{
				this[mParentTable.Columns[key]] = value;
				
				mValues[key] = value;
			}
		}

		internal void WriteWithoutValidation(AgateColumn column, string value)
		{
			mValues[column.Name] = value;
		}

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
