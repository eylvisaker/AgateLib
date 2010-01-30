using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;
using System.ComponentModel;

namespace AgateLib.Data
{
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

		public AgateColumn Clone()
		{
			AgateColumn retval = new AgateColumn();

			retval.mName = mName;
			retval.mDescription = mDescription;
			retval.mTableLookup = mTableLookup;
			retval.mLookupField = mLookupField;
			retval.mPrimaryKey = mPrimaryKey;
			retval.mNextAutoIncrementValue = mNextAutoIncrementValue;
			retval.mFieldType = mFieldType;

			return retval;
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

		[Browsable(false)]
		public int NextAutoIncrementValue
		{
			get { return mNextAutoIncrementValue; }
		}
		[Browsable(false)]
		public int ColumnWidth
		{
			get { return mColumnWidth; }
			set { mColumnWidth = value; }
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

		public string Name
		{
			get { return mName; }
			set
			{
				AssertIsValidName(value);

				mName = value;
			}
		}
		public FieldType FieldType
		{
			get { return mFieldType; }
			set { mFieldType = value; }
		}
		[Browsable(false)]
		public Type FieldTypeDataType
		{
			get
			{
				return AgateDataHelper.FromFieldType(FieldType);
			}
		}
		public bool IsPrimaryKey
		{
			get { return mPrimaryKey; }
			set { mPrimaryKey = value; }
		}
		public string TableLookup
		{
			get { return mTableLookup; }
			set { mTableLookup = value; }
		}
		public string TableDisplayField
		{
			get { return mLookupField; }
			set { mLookupField = value; }
		}
		public string Description
		{
			get { return mDescription; }
			set { mDescription = value; }
		}

		#endregion

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
			return AgateDataHelper.IsValidIdentifier(value);
		}


		internal void IncrementNextAutoIncrementValue()
		{
			mNextAutoIncrementValue++;
		}
	}
}
