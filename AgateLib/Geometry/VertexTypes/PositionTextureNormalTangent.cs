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
	/// Vertex structure with position, texture coordinates, normal and tangent.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PositionTextureNormalTangent
	{
		/// <summary>
		/// 
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// 
		/// </summary>
		public Vector2 Texture;
		/// <summary>
		/// 
		/// </summary>
		public Vector3 Normal;
		/// <summary>
		/// 
		/// </summary>
		public Vector3 Tangent;

		/// <summary>
		/// 
		/// </summary>
		public static VertexLayout VertexLayout
		{
			get
			{
				return new VertexLayout 
				{ 
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Position),
					new VertexElementDesc(VertexElementDataType.Float2, VertexElement.Texture),
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Normal),
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Tangent),
				};
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(
				"P:{0}  Tex:{1}  N:{2}  T:{3}", Position, Texture, Normal, Tangent);
		}
	}
}
