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
using System.Runtime.InteropServices;
using System.Text;

namespace AgateLib.Geometry.VertexTypes
{
	/// <summary>
	/// Vertex layout which only contains position and color information.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PositionColor
	{
		/// <summary>
		/// Vertex position
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Vertex color
		/// </summary>
		public int Color;

		/// <summary>
		/// Constructs vertex.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="color"></param>
		public PositionColor(float x, float y, float z, Color color)
			: this(x, y, z, color.ToArgb())
		{ }
		/// <summary>
		/// Constructs vertex.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="color"></param>
		public PositionColor(float x, float y, float z, int color)
		{
			Position = new Vector3(x, y, z);
			this.Color = color;
		}

		/// <summary>
		/// Layout for the PositionColor vertex type.
		/// </summary>
		public static VertexLayout VertexLayout
		{
			get
			{
				return new VertexLayout 
				{ 
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Position),
					new VertexElementDesc(VertexElementDataType.Int, VertexElement.DiffuseColor),
				};
			}
		}
	}
}
