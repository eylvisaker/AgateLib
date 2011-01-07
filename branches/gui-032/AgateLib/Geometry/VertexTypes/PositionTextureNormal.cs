using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AgateLib.Geometry.VertexTypes
{
	/// <summary>
	/// Vertex structure with position, texture and normal values.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct PositionTextureNormal
	{
		/// <summary>
		/// Position
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// Texture coordinates
		/// </summary>
		public Vector2 Texture;
		/// <summary>
		/// Normal value
		/// </summary>
		public Vector3 Normal;

		/// <summary>
		/// Vertex layout for PositionTextureNormal structure.
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
				};
			}
		}
	}
}
