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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Enum which describes different pixel formats.
	/// Order of the characters in the constant name specifies the
	/// ordering of the bytes for the pixel data, from least to most significant.
	/// See remarks for more information.
	/// </summary>
	/// <remarks>
	/// Order of the characters in the constant name specifies the
	/// ordering of the bytes for the pixel data, from least to most significant on 
	/// a little-endian architecture.  In other words, the first character indicates
	/// the meaning of the first byte or bits in memory.
	/// 
	/// For example, ARGB8888 indicates that the alpha channel is the least significant,
	/// the blue channel is most significant, and each channel is eight bits long.
	/// The alpha channel is stored first in memory, followed by red, green and blue.
	/// </remarks>
	public enum PixelFormat
	{
		/// <summary>
		/// Format specifying the Agate should choose what pixel format 
		/// to use, where appropriate.
		/// </summary>
		Any,

		#region --- 32 bit formats ---

		/// <summary>
		/// 
		/// </summary>
		ARGB8888,
		/// <summary>
		/// 
		/// </summary>
		ABGR8888,
		/// <summary>
		/// 
		/// </summary>
		BGRA8888,
		/// <summary>
		/// 
		/// </summary>
		RGBA8888,

		/// <summary>
		/// 
		/// </summary>
		XRGB8888,

		/// <summary>
		/// 
		/// </summary>
		XBGR8888,

		#endregion

		#region --- 24 bit formats ---

		/// <summary>
		/// 
		/// </summary>
		RGB888,
		/// <summary>
		/// 
		/// </summary>
		BGR888,

		#endregion

		#region --- 16 bit formats ---

		/// <summary>
		/// 
		/// </summary>
		RGB565,
		/// <summary>
		/// 
		/// </summary>
		XRGB1555,
		/// <summary>
		/// 
		/// </summary>
		XBGR1555,
		/// <summary>
		/// 
		/// </summary>
		BGR565,

		#endregion

	}
}
