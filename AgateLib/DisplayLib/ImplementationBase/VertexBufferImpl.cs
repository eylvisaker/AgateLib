//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry.VertexTypes;

namespace AgateLib.DisplayLib.ImplementationBase
{
	/// <summary>
	/// Class for implementing a vertex buffer stored in hardware memory.
	/// </summary>
	public abstract class VertexBufferImpl : IDisposable
	{
		/// <summary>
		/// Constructs a vertex buffer implementation.
		/// </summary>
		protected VertexBufferImpl()
		{
			Textures = new TextureList();
		}

		/// <summary>
		/// Disposes of the buffer.
		/// </summary>
		public abstract void Dispose();

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
