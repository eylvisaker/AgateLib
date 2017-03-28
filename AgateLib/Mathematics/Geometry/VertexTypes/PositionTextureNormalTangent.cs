//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System.Runtime.InteropServices;

namespace AgateLib.Mathematics.Geometry.VertexTypes
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
		public Vector3f Position;
		/// <summary>
		/// 
		/// </summary>
		public Vector2f Texture;
		/// <summary>
		/// 
		/// </summary>
		public Vector3f Normal;
		/// <summary>
		/// 
		/// </summary>
		public Vector3f Tangent;

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
