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
	/// Vertex structure with position, texture and normal values.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct PositionTextureColorNormal
	{
		/// <summary>
		/// Position
		/// </summary>
		public Vector3f Position;
		/// <summary>
		/// Texture coordinates
		/// </summary>
		public Vector2f Texture;
		/// <summary>
		/// Color value.
		/// </summary>
		public int Color;
		/// <summary>
		/// Normal value
		/// </summary>
		public Vector3f Normal;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <param name="color"></param>
		/// <param name="tu"></param>
		/// <param name="tv"></param>
		/// <param name="nx"></param>
		/// <param name="ny"></param>
		/// <param name="nz"></param>
		public PositionTextureColorNormal(float x, float y, float z, Color color, float tu, float tv, float nx, float ny, float nz)
			: this(x, y, z, color.ToArgb(), tu, tv, nx, ny, nz)
		{ }
		/// <summary>
		/// Constructor
		/// </summary>
		public PositionTextureColorNormal(float x, float y, float z, int color, float tu, float tv, float nx, float ny, float nz)
		{
			Position = new Vector3f(x, y, z);
			Texture = new Vector2f(tu, tv);
			this.Color = color;
			Normal = new Vector3f(nx, ny, nz);
		}

		/// <summary>
		/// X position
		/// </summary>
		public float X { get { return Position.X; } set { Position.X = value; } }
		/// <summary>
		/// Y position
		/// </summary>
		public float Y { get { return Position.Y; } set { Position.Y = value; } }
		/// <summary>
		/// Z position
		/// </summary>
		public float Z { get { return Position.Z; } set { Position.Z = value; } }

		/// <summary>
		/// Texture coordinate u
		/// </summary>
		public float U { get { return Texture.X; } set { Texture.X = value; } }
		/// <summary>
		/// Texture coordinate v
		/// </summary>
		public float V { get { return Texture.Y; } set { Texture.Y = value; } }

		/// <summary>
		/// ToString debugging information.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("X: {0} Y: {1} Z: {2} Color: {3} Tu: {4}, Tv: {5} Nx: {6} Ny: {7} Nz: {8}",
				Position.X, Position.Y, Position.Z, Color, Texture.X, Texture.Y, Normal.X, Normal.Y, Normal.Z);
		}

		static VertexLayout sLayout;

		/// <summary>
		/// Vertex Layout for PositionTextureColorNormal.
		/// </summary>
		public static VertexLayout VertexLayout
		{
			get
			{
				if (sLayout == null)
				{
					sLayout = new VertexLayout 
					{ 
						new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Position),
						new VertexElementDesc(VertexElementDataType.Float2, VertexElement.Texture),
						new VertexElementDesc(VertexElementDataType.Int, VertexElement.DiffuseColor),
						new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Normal),
					};
				}

				return sLayout;
			}
		}
	}

}
