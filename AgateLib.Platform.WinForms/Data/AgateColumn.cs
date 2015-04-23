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
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;
using System.ComponentModel;

namespace AgateLib.Data
{
	/// <summary>
	/// Represents a column in a table.
	/// </summary>
	public class AgateColumn : IXleSerializable
	{
		private string mName;
		private string mDescription;
		private string mTableLookup;
		private string mLookupField;
		private bool mPrimaryKey;
		private int mNextAutoIncrementValue = 1;
		private FieldType mFieldType;
		private int mColumnWidth;

		#region --- Construction and Serialization ---

		/// <summary>
		/// Creates a deep copy of the AgateColumn object.
		/// </summary>
		/// <returns></returns>
		public AgateColumn Clone()
		{
			AgateColumn result = new AgateColumn();

			result.mName = mName;
			result.mDescription = mDescription;
			result.mTableLookup = mTableLookup;
			result.mLookupField = mLookupField;
			result.mPrimaryKey = mPrimaryKey;
			result.mNextAutoIncrementValue = mNextAutoIncrementValue;
			result.mFieldType = mFieldType;

			return result;
		}

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("Name", mName, true);
			info.WriteEnum("FieldType", mFieldType, true);

			if (mFieldType == FieldType.AutoNumber)
				info.Write("NextValue", mNextAutoIncrementValue, true);
			if (mPrimaryKey)
				info.Write("PrimaryKey", mPrimaryKey, true);

			info.Write("Description", mDescription);

			if (string.IsNullOrEmpty(mTableLookup) == false)
			{
				info.Write("TableLookup", mTableLookup);
				info.Write("LookupField", mLookupField);
			}

			if (mColumnWidth > 0)
			{
				info.Write("ColumnWidth", mColumnWidth);
			}
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mName = info.ReadString("Name");
			mFieldType = info.ReadEnum<FieldType>("FieldType");
			mNextAutoIncrementValue = info.ReadInt32("NextValue", 1);
			mPrimaryKey = info.ReadBoolean("PrimaryKey", false);
			mDescription = info.ReadString("Description");
			mTableLookup = info.ReadString("TableLookup", string.Empty);
			mLookupField = info.ReadString("LookupField", string.Empty);
			mColumnWidth = info.ReadInt32("ColumnWidth", 0);
		}

		#endregion
		#region --- Properties ---

		/// <summary>
		/// Gets the next value that will be used if this is an 
		/// auto increment field.  If this is not an autoincrement
		/// field, the return value is undefined and meaningless.
		/// </summary>
		
		public int NextAutoIncrementValue
		{
			get { return mNextAutoIncrementValue; }
		}
		/// <summary>
		/// Gets the width of the column as displayed in the database editor.
		/// </summary>
		
		public int ColumnWidth
		{
			get { return mColumnWidth; }
			set { mColumnWidth = value; }
		}
		/// <summary>
		/// Gets or sets the display index of this column.
		/// When saved, columns are sorted by their display index.
		/// </summary>
		
		public int DisplayIndex { get; set; }

		/// <summary>
		/// Gets the default value for the datatype in this column.
		/// </summary>
		public string DefaultValue
		{
			get
			{
				if (FieldType == FieldType.String)
					return string.Empty;

				return Activator.CreateInstance(FieldTypeDataType).ToString();
			}
		}

		/// <summary>
		/// Gets the name of the column.
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

		/// <summary>
		/// Gets the data type for value in this column.
		/// </summary>
		public FieldType FieldType
		{
			get { return mFieldType; }
			set { mFieldType = value; }
		}

		/// <summary>
		/// Gets the actual Type object that corresponds to the
		/// FieldType enum.
		/// </summary>
		
		public Type FieldTypeDataType
		{
			get
			{
				return AgateDataHelper.FromFieldType(FieldType);
			}
		}
		/// <summary>
		/// Gets or sets whether or not this column is the primary key.
		/// </summary>
		public bool PrimaryKey
		{
			get { return mPrimaryKey; }
			set { mPrimaryKey = value; }
		}
		/// <summary>
		/// Gets or sets whether values entered in this column should be looked
		/// up in another table.
		/// </summary>
		public string TableLookup
		{
			get { return mTableLookup; }
			set { mTableLookup = value; }
		}
		/// <summary>
		/// Gets or sets what field is used to display data in another table
		/// when the table lookup is used.
		/// </summary>
		public string TableDisplayField
		{
			get { return mLookupField; }
			set { mLookupField = value; }
		}
		/// <summary>
		/// Gets or sets the description of this column.  This property is also used
		/// to comment properties when code is autogenerated.
		/// </summary>
		public string Description
		{
			get { return mDescription; }
			set { mDescription = value; }
		}
		/// <summary>
		/// Gets whether or not the data type for this column is a numeric type.
		/// </summary>
		public bool IsNumeric
		{
			get
			{
				switch (FieldType)
				{
					case FieldType.AutoNumber:
					case FieldType.Byte:
					case FieldType.Decimal:
					case FieldType.Double:
					case FieldType.Int16:
					case FieldType.Int32:
					case FieldType.SByte:
					case FieldType.Single:
					case FieldType.UInt16:
					case FieldType.UInt32:
						return true;

					default:
						return false;
				}
			}
		}
		#endregion

		/// <summary>
		/// Returns a string representation of the AgateColumn object.
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Checks to see if the name is a valid for a column.  It must be a valid C# identifier.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsValidColumnName(string value)
		{
			return AgateDataHelper.IsValidIdentifier(value);
		}


		internal void IncrementNextAutoIncrementValue()
		{
			mNextAutoIncrementValue++;
		}

		internal void SetNextAutoIncrementValue(int value)
		{
			mNextAutoIncrementValue = value;
		}
	}
}
