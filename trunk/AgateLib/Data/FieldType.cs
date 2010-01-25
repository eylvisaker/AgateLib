using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Data
{
	public enum FieldType
	{
		[DataType(typeof(String))]
		String,
		[DataType(typeof(Boolean))]
		Boolean,
		[DataType(typeof(Int16))]
		Int16,
		[DataType(typeof(Int32))]
		Int32,
		[DataType(typeof(UInt16))]
		UInt16,
		[DataType(typeof(UInt32))]
		UInt32,
		[DataType(typeof(Byte))]
		Byte,
		[DataType(typeof(SByte))]
		SByte,
		[DataType(typeof(Single))]
		Single,
		[DataType(typeof(Double))]
		Double,
		[DataType(typeof(Decimal))]
		Decimal,
		[DataType(typeof(DateTime))]
		DateTime,
		[DataType(typeof(Int32))]
		AutoNumber,
	}

	[global::System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
	sealed class DataTypeAttribute : Attribute
	{
		readonly Type dataType;

		public DataTypeAttribute(Type dataType)
		{
			this.dataType = dataType;
		}

		public Type DataType { get { return dataType; } }
	}
}
