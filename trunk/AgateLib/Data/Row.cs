using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
	public class Row
	{
		Dictionary<string, string> values = new Dictionary<string, string>();
		Table parentTable;

		public Row(Table parentTable)
		{
			this.parentTable = parentTable;

			foreach (var column in parentTable.Columns)
			{
				values[column.Name] = null;
			}
		}

		public Row Clone()
		{
			Row retval = new Row(parentTable);

			foreach (var value in values)
				retval.values[value.Key] = value.Value;

			return retval;
		}
		public Table ParentTable
		{
			get { return parentTable; }
			internal set
			{
				parentTable = value;

				if (parentTable != null)
				{
					ValidateData(parentTable);
				}
			}
		}

		/// <summary>
		/// Shortcut for this[column.Name].
		/// </summary>
		/// <param name="column"></param>
		/// <returns></returns>
		public string this[Column column]
		{
			get { return values[column.Name]; }
			set
			{
				ValidateTypeOrThrow(column.Name, value);

				if (column.FieldType == FieldType.AutoNumber)
					throw new AgateDatabaseException("Cannot write to autonumber field.");

				values[column.Name] = value;
			}
		}
		public string this[string key]
		{
			get { return values[key]; }
			set
			{
				this[parentTable.Columns[key]] = value;
				
				values[key] = value;
			}
		}

		internal void WriteWithoutValidation(Column column, string value)
		{
			values[column.Name] = value;
		}

		public override string ToString()
		{
			StringBuilder b = new StringBuilder();
			int count = 0;

			foreach (var column in parentTable.Columns)
			{
				string value = DataHelper.FixString(this[column.Name]);
				
				if (count > 0)
					b.Append(",");
				
				b.Append(value);

				count++;
			}

			return b.ToString();
		}

		private void ValidateTypeOrThrow(string key, string value)
		{
			Convert.ChangeType(value, DataHelper.GetType(parentTable.Columns[key].FieldType));
		}

		internal void ValidateData(Table agateTable)
		{
			foreach (var column in agateTable.Columns)
			{
				if (column.FieldType == FieldType.AutoNumber &&
					(values.ContainsKey(column.Name) == false ||
					values[column.Name] == null))
				{
					int value = column.NextAutoIncrementValue;
					column.IncrementNextAutoIncrementValue();

					values[column.Name] = value.ToString();
				}

				if (values.ContainsKey(column.Name))
				{
					ValidateTypeOrThrow(column.Name, values[column.Name]);
				}
				else 
				{
					values.Add(column.Name, null);
				}

				if (column.IsPrimaryKey)
				{
					var matches = from x in agateTable.Rows
								  where x != null && x[column] == this[column]
								  select x;

					List<Row> l = matches.ToList();
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
			string value = values[oldName];
			values[newName] = value;

			values.Remove(oldName);
		}
	}
}
