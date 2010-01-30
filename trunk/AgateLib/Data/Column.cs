using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;
using System.ComponentModel;

namespace AgateLib.Data
{
	public class Column : IXleSerializable
	{
		private string name;
		private string description;
		private string tableLookup;
		private string lookupField;
		private bool primaryKey;
		private int nextAutoIncrementValue = 1;
		private FieldType fieldType;

		public Column Clone()
		{
			Column retval = new Column();

			retval.name = name;
			retval.description = description;
			retval.tableLookup = tableLookup;
			retval.lookupField = lookupField;
			retval.primaryKey = primaryKey;
			retval.nextAutoIncrementValue = nextAutoIncrementValue;
			retval.fieldType = fieldType;

			return retval;
		}

		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Name", name, true);
			info.WriteEnum("FieldType", fieldType, true);

			if (fieldType == FieldType.AutoNumber)
				info.Write("NextValue", nextAutoIncrementValue, true);
			if (primaryKey)
				info.Write("PrimaryKey", primaryKey, true);

			info.Write("Description", description);

			if (string.IsNullOrEmpty(tableLookup) == false)
			{
				info.Write("TableLookup", tableLookup);
				info.Write("LookupField", lookupField);
			}
		}

		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			name = info.ReadString("Name");
			fieldType = info.ReadEnum<FieldType>("FieldType");
			nextAutoIncrementValue = info.ReadInt32("NextValue", 1);
			primaryKey = info.ReadBoolean("PrimaryKey", false);
			description = info.ReadString("Description");
			tableLookup = info.ReadString("TableLookup", string.Empty);
			lookupField = info.ReadString("LookupField", string.Empty);
		}

		#endregion

		[Browsable(false)]
		public int NextAutoIncrementValue
		{
			get { return nextAutoIncrementValue; }
		}

		public string DefaultValue
		{
			get
			{
				if (FieldType == FieldType.String)
					return string.Empty;

				return Activator.CreateInstance(FieldTypeDataType).ToString();
			}
		}

		internal void IncrementNextAutoIncrementValue()
		{
			nextAutoIncrementValue++;
		}

		public string Name
		{
			get { return name; }
			set
			{
				AssertIsValidName(value);

				name = value;
			}
		}
		public FieldType FieldType
		{
			get { return fieldType; }
			set { fieldType = value; }
		}
		[Browsable(false)]
		public Type FieldTypeDataType
		{
			get
			{
				return DataHelper.FromFieldType(FieldType);
			}
		}
		public bool IsPrimaryKey
		{
			get { return primaryKey; }
			set { primaryKey = value; }
		}
		public string TableLookup
		{
			get { return tableLookup; }
			set { tableLookup = value; }
		}
		public string TableDisplayField
		{
			get { return lookupField; }
			set { lookupField = value; }
		}
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		public override string ToString()
		{
			return "Column: " + Name;
		}

		private void AssertIsValidName(string value)
		{
			if (IsValidColumnName(value))
				return;

			throw new ArgumentException(string.Format(
				"Invalid name \"{0}\" supplied.  Column name should be a valid C# or VB identifier.", value));
		}

		public static bool IsValidColumnName(string value)
		{
			return DataHelper.IsValidIdentifier(value);
		}


	}
}
