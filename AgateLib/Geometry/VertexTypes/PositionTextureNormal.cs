using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AgateLib.Geometry.VertexTypes
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct PositionTextureNormal
	{
		public Vector3 Position;
		public Vector2 Texture;
		public Vector3 Normal;

		public static VertexLayout VertexLayout
		{
			get
			{
				return new VertexLayout 
				{ 
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Position),
					new VertexElementDesc(VertexElementDataType.Float2, VertexElement.Texture),
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Normal),
				};
			}
		}
	}
}
