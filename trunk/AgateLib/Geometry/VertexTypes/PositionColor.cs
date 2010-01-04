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
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
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
