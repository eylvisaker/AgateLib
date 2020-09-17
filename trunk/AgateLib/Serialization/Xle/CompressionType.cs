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
