using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Primitive type for types in a vertex buffer.
	/// </summary>
	public enum PrimitiveType
	{
		/// <summary>
		/// Every three vertices is a separate triangle
		/// </summary>
		TriangleList,
		/// <summary>
		/// The vertices indicate a fan; the first point creates a triangle with 
		/// each pair of points after that.
		/// </summary>
		TriangleFan,
		/// <summary>
		/// The vertices make a strip of triangles, so each triangle shares a vertex
		/// with the previous one.
		/// </summary>
		TriangleStrip,
	}
}
