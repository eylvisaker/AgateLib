using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AgateLib.Geometry.VertexTypes
{
	/// <summary>
	/// Vertex structure with position, texture and color values
	/// </summary>
	[StructLayout(LayoutKind.Sequential
#if !XNA
		, Pack = 1
#endif
)]
	public struct PositionTextureColor
	{
		/// <summary>
		/// Position of vertex.
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Texture coordinates of vertex.
		/// </summary>
		public Vector2 TexCoord;
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
			: this(x, y, z, color.ToArgb(), tu, tv)
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
			Position = new Vector3(x, y, z);
			TexCoord = new Vector2(tu, tv);
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
