using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AgateLib.Geometry.VertexTypes
{
	[StructLayout(LayoutKind.Sequential)]
	public struct PositionTextureColor
	{
		public Vector3 Position;
		public Vector2 TexCoord;
		public int Color;

		public PositionTextureColor(float x, float y, float z, Color color, float tu, float tv)
			: this(x, y, z, color.ToArgb(), tu, tv)
		{ }
		public PositionTextureColor(float x, float y, float z, int color, float tu, float tv)
		{
			Position = new Vector3(x, y, z);
			TexCoord = new Vector2(tu, tv);
			this.Color = color;
		}

		public float X { get { return Position.X; } set { Position.X = value; } }
		public float Y { get { return Position.Y; } set { Position.Y = value; } }
		public float Z { get { return Position.Z; } set { Position.Z = value; } }

		public override string ToString()
		{
			return string.Format("X: {0} Y: {1} Z: {2} Color: {3} Tu: {4}, Tv: {5}",
				Position.X, Position.Y, Position.Z, Color, TexCoord.X, TexCoord.Y);
		}

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
