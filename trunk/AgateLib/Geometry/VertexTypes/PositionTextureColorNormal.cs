using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AgateLib.Geometry.VertexTypes
{
	[StructLayout(LayoutKind.Sequential)]
	public struct PositionTextureColorNormal
	{
		public Vector3 Position;
		public Vector2 TexCoord;
		public int Color;
		public Vector3 Normal;

		public PositionTextureColorNormal(float x, float y, float z, Color color, float tu, float tv, float nx, float ny, float nz)
			: this(x, y, z, color.ToArgb(), tu, tv, nx, ny, nz)
		{ }
		public PositionTextureColorNormal(float x, float y, float z, int color, float tu, float tv, float nx, float ny, float nz)
		{
			Position = new Vector3(x, y, z);
			TexCoord = new Vector2(tu, tv);
			this.Color = color;
			Normal = new Vector3(nx, ny, nz);
		}

		public float X { get { return Position.X; } set { Position.X = value; } }
		public float Y { get { return Position.Y; } set { Position.Y = value; } }
		public float Z { get { return Position.Z; } set { Position.Z = value; } }

		public float U { get { return TexCoord.X; } set { TexCoord.X = value; } }
		public float V { get { return TexCoord.Y; } set { TexCoord.Y = value; } }

		public override string ToString()
		{
			return string.Format("X: {0} Y: {1} Z: {2} Color: {3} Tu: {4}, Tv: {5} Nx: {6} Ny: {7} Nz: {8}",
				Position.X, Position.Y, Position.Z, Color, TexCoord.X, TexCoord.Y, Normal.X, Normal.Y, Normal.Z);
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
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Normal),
				};
			}
		}
	}

}
