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

namespace AgateLib.Data
{
	/// <summary>
	/// Enum which is used to indicate the type of data
	/// that a field in an AgateDatabase should contain.
	/// </summary>
	public enum FieldType
	{
		/// <summary>
		/// The field contains string data.
		/// </summary>
		[DataType(typeof(String))]
		String,

		/// <summary>
		/// The field contains a logical true/false value.
		/// </summary>
		[DataType(typeof(Boolean))]
		Boolean,
		
		/// <summary>
		/// The field contains a 16-bit integer.
		/// </summary>
		[DataType(typeof(Int16))]
		Int16,
		
		/// <summary>
		/// The field contains a 32-bit integer.
		/// </summary>
		[DataType(typeof(Int32))]
		Int32,

		/// <summary>
		/// The field contains an unsigned 16-bit integer.
		/// </summary>
		[DataType(typeof(UInt16))]
		UInt16,

		/// <summary>
		/// The field contains an unsigned 32-bit integer.
		/// </summary>
		[DataType(typeof(UInt32))]
		UInt32,

		/// <summary>
		/// The field contains an unsigned 8-bit integer.
		/// </summary>
		[DataType(typeof(Byte))]
		Byte,

		/// <summary>
		/// The field contains a signed 8-bit integer.
		/// </summary>
		[DataType(typeof(SByte))]
		SByte,


		/// <summary>
		/// The field contains a single precision floating point value.
		/// </summary>
		[DataType(typeof(Single))]
		Single,
		/// <summary>
		/// The field contains a double precision floating point value.
		/// </summary>
		[DataType(typeof(Double))]
		Double,

		/// <summary>
		/// The field contains the .NET System.Decimal type.
		/// </summary>
		[DataType(typeof(Decimal))]
		Decimal,

		/// <summary>
		/// The field contains a DateTime structure.
		/// </summary>
		[DataType(typeof(DateTime))]
		DateTime,

		/// <summary>
		/// The field is automatically numbered as rows are added.
		/// This implies a 32-bit integer is used.
		/// </summary>
		[DataType(typeof(Int32))]
		AutoNumber,
	}

	/// <summary>
	/// Class which is used for the FieldType enum in order to 
	/// associate actual type objects with the enum values.
	/// </summary>
	[global::System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
	sealed class DataTypeAttribute : Attribute
	{
		readonly Type mDataType;

		public DataTypeAttribute(Type dataType)
		{
			this.mDataType = dataType;
		}

		public Type DataType { get { return mDataType; } }
	}
}
