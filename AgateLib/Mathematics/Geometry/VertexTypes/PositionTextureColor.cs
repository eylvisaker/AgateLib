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
	/// Vertex structure with position, texture and color values
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PositionTextureColor
	{
		/// <summary>
		/// Position of vertex.
		/// </summary>
		public Vector3f Position;
		/// <summary>
		/// Texture coordinates of vertex.
		/// </summary>
		public Vector2f TexCoord;
		/// <summary>
		/// Color value of vertex.
		/// </summary>
		public int Color;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="color"></param>
		/// <param name="tu"></param>
		/// <param name="tv"></param>
		public PositionTextureColor(float x, float y, float z, Color color, float tu, float tv)
			: this(x, y, z, color.ToAbgr(), tu, tv)
		{ }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="color"></param>
		/// <param name="tu"></param>
		/// <param name="tv"></param>
		public PositionTextureColor(float x, float y, float z, int color, float tu, float tv)
		{
			Position = new Vector3f(x, y, z);
			TexCoord = new Vector2f(tu, tv);
			this.Color = color;
		}

		/// <summary>
		/// X Position 
		/// </summary>
		public float X { get { return Position.X; } set { Position.X = value; } }
		/// <summary>
		/// Y Position 
		/// </summary>
		public float Y { get { return Position.Y; } set { Position.Y = value; } }
		/// <summary>
		/// Z Position 
		/// </summary>
		public float Z { get { return Position.Z; } set { Position.Z = value; } }

		/// <summary>
		/// Gets or sets the U texture coordinate.
		/// </summary>
		public float U { get { return TexCoord.X; } set { TexCoord.X = value; } }
		/// <summary>
		/// Gets or sets the V texture coordinate.
		/// </summary>
		public float V { get { return TexCoord.Y; } set { TexCoord.Y = value; } }

		/// <summary>
		/// ToString debugging information.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("X: {0} Y: {1} Z: {2} Color: {3} Tu: {4}, Tv: {5}",
				Position.X, Position.Y, Position.Z, Color, TexCoord.X, TexCoord.Y);
		}

		/// <summary>
		/// Gets the vertex layout for PositionTextureColor.
		/// </summary>
		public static VertexLayout VertexLayout
		{
			get
			{
				return new VertexLayout 
				{ 
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Position),
					new VertexElementDesc(VertexElementDataType.Float2, VertexElement.Texture),
					new VertexElementDesc(VertexElementDataType.Int, VertexElement.DiffuseColor),
				};
			}
		}
	}

}
