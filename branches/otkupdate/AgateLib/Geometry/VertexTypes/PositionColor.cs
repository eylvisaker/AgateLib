using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AgateLib.Geometry.VertexTypes
{
	[StructLayout(LayoutKind.Sequential)]
	public struct PositionColor
	{
		public Vector3 Position;
		public int Color;

		public PositionColor(float x, float y, float z, Color color)
			: this(x, y, z, color.ToArgb())
		{ }
		public PositionColor(float x, float y, float z, int color)
		{
			Position = new Vector3(x, y, z);
			this.Color = color;
		}


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
