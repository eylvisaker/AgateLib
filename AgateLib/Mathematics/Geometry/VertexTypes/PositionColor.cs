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
using AgateLib.DisplayLib;

namespace AgateLib.Mathematics.Geometry.VertexTypes
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
		public Vector3f Position;
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
			Position = new Vector3f(x, y, z);
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
