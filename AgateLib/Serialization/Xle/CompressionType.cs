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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Serialization.Xle
{
	/// <summary>
	/// Indicates the type of compression used when serializing binary data.
	/// </summary>
	public enum CompressionType
	{
		/// <summary>
		/// Use no compression.  Note that data will still be Base64 encoded,
		/// so this will result in an increase in the storage space required.
		/// </summary>
		None,
		/// <summary>
		/// The Deflate algorithm, as is commonly used by zip archiving programs.
		/// </summary>
		Deflate,
		/// <summary>
		/// The GZip algorithm, commonly used on unix systems to compress data.
		/// </summary>
		GZip,
	}
}
