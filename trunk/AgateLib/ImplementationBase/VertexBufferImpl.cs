using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;

namespace AgateLib.ImplementationBase
{
	/// <summary>
	/// Class for implementing a vertex buffer stored in hardware memory.
	/// </summary>
	public abstract class VertexBufferImpl
	{
		/// <summary>
		/// Constructs a vertex buffer implementation.
		/// </summary>
		public VertexBufferImpl()
		{
			Textures = new TextureList();
		}

		/// <summary>
		/// Writes vertices to the vertex buffer.
		/// </summary>
		/// <typeparam name="T">The type of vertices in the buffer.  This type must be a struct, and may not
		/// contain any reference types.</typeparam>
		/// <param name="vertices">The vertex data.</param>
		public abstract void Write<T>(T[] vertices) where T:struct;

		/// <summary>
		/// Gets the number of vertices in the buffer.
		/// </summary>
		public abstract int VertexCount { get; }

		/// <summary>
		/// Gets or sets an enum value indicating the type of primitives drawn from this
		/// vertex buffer.
		/// </summary>
		public virtual PrimitiveType PrimitiveType { get; set; }
		/// <summary>
		/// Gets the textures that should be used in drawing primitives from this vertex
		/// buffer.
		/// </summary>
		public TextureList Textures { get; set; }

		/// <summary>
		/// Draws primitives from this vertex buffer.
		/// </summary>
		/// <param name="start">Index of the first vertex to use when drawing.</param>
		/// <param name="count">Number of primitives to draw.</param>
		public abstract void Draw(int start, int count);
		/// <summary>
		/// Draws primitives from this vertex buffer, using the specified index buffer.
		/// </summary>
		/// <param name="indexbuffer">The index buffer from which to draw indices from.</param>
		/// <param name="start">Index of the first value in the index buffer to use.</param>
		/// <param name="count">Number of primitives to draw.</param>
		public abstract void DrawIndexed(IndexBuffer indexbuffer, int start, int count);

		/// <summary>
		/// Gets the vertex layout which is used to interpret the data in the vertex buffer.
		/// </summary>
		public abstract VertexLayout VertexLayout { get; }
	}
}
