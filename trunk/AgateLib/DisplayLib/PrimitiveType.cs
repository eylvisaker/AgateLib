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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Primitive type for types in a vertex buffer.
	/// </summary>
	public enum PrimitiveType
	{
		/// <summary>
		/// Every three vertices is a separate triangle
		/// </summary>
		TriangleList,
		/// <summary>
		/// The vertices indicate a fan; the first point creates a triangle with 
		/// each pair of points after that.
		/// </summary>
		TriangleFan,
		/// <summary>
		/// The vertices make a strip of triangles, so each triangle shares a vertex
		/// with the previous one.
		/// </summary>
		TriangleStrip,
	}
}
