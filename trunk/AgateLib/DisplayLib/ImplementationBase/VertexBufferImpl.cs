//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;

namespace AgateLib.DisplayLib.ImplementationBase
{
	/// <summary>
	/// Class for implementing a vertex buffer stored in hardware memory.
	/// </summary>
	public abstract class VertexBufferImpl
	{
		/// <summary>
		/// Constructs a vertex buffer implementation.
		/// </summary>
		protected VertexBufferImpl()
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
