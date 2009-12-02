using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AgateLib.Geometry.VertexTypes
{
	/// <summary>
	/// Vertex structure with position, texture coordinates, normal, tangent, bitangent.
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct PositionTextureNTB
	{
		/// <summary>
		/// 
		/// </summary>
		public Vector3 Position;
		/// <summary>
		/// 
		/// </summary>
		public Vector2 Texture;
		/// <summary>
		/// 
		/// </summary>
		public Vector3 Normal;
		/// <summary>
		/// 
		/// </summary>
		public Vector3 Tangent;
		/// <summary>
		/// 
		/// </summary>
		public Vector3 Bitangent;

		/// <summary>
		/// 
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
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Tangent),
					new VertexElementDesc(VertexElementDataType.Float3, VertexElement.Bitangent),
				};
			}
		}
	}
}
