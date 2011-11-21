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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which represents a vertex buffer in memory.
	/// </summary>
	public sealed class VertexBuffer
	{
		VertexBufferImpl impl;

		/// <summary>
		/// Constructs a vertex buffer object.
		/// </summary>
		/// <param name="layout">A VertexLayout object which describes how the
		/// vertex data is laid out in memory.</param>
		/// <param name="vertexCount">The number of vertices to allocate in the buffer.</param>
		public VertexBuffer(VertexLayout layout, int vertexCount)
		{
			if (layout == null)
				throw new ArgumentNullException(
					"The supplied VertexLayout must not be null.  " +
					"You may wish to use one of the static members of VertexLayout.");

			if (layout.Count == 0)
				throw new ArgumentException(
					"The supplied VertexLayout has no items in it.  You must supply a valid layout.");

			impl = Display.Impl.CreateVertexBuffer(layout, vertexCount);
		}

		/// <summary>
		/// Writes data to the vertex buffer.
		/// </summary>
		/// <typeparam name="T">Type of the vertex data.  This must be a struct type, and it
		/// should have the attribute StructLayout(LayoutKind.Sequential, Pack = 1) </typeparam>
		/// <param name="data">The vertex data.  It must be a struct type, and its type
		/// should have the attribute StructLayout(LayoutKind.Sequential, Pack = 1) </param>
		public void WriteVertexData<T>(T[] data) where T:struct 
		{
			impl.Write(data);
		}
		/// <summary>
		/// Draws the vertex buffer.
		/// </summary>
		public void Draw()
		{
			impl.Draw(0, VertexCount);
		}
		/// <summary>
		/// Draws primitives from the specified vertex range in vertex buffer.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="count"></param>
		public void Draw(int start, int count)
		{
			impl.Draw(start, count);
		}
		/// <summary>
		/// Draws the vertices using the indexes in the index buffer.
		/// </summary>
		/// <param name="indexbuffer"></param>
		public void DrawIndexed(IndexBuffer indexbuffer)
		{
			impl.DrawIndexed(indexbuffer, 0, indexbuffer.Count);
		}
		/// <summary>
		/// Draws the vertices using the specified set of indexes in the index buffer.
		/// </summary>
		/// <param name="indexbuffer"></param>
		/// <param name="start"></param>
		/// <param name="count"></param>
		public void DrawIndexed(IndexBuffer indexbuffer, int start, int count)
		{
			impl.DrawIndexed(indexbuffer, start, count);
		}

		/// <summary>
		/// Gets the number of vertices in the vertex buffer.
		/// </summary>
		public int VertexCount
		{
			get { return impl.VertexCount; }
		}
		/// <summary>
		/// Gets the type of primitive that will be drawn from the vertex buffer.
		/// </summary>
		public PrimitiveType PrimitiveType
		{
			get { return impl.PrimitiveType; }
			set { impl.PrimitiveType = value; }
		}
		/// <summary>
		/// Gets a list of the bound textures for drawing from this vertex buffer.
		/// </summary>
		public TextureList Textures
		{
			get { return impl.Textures; }
		}
		/// <summary>
		/// Gets the in-memory layout of the vertex data.
		/// </summary>
		public VertexLayout VertexLayout
		{
			get { return impl.VertexLayout; }
		}
	}

	/// <summary>
	/// Class which contains a list of textures for drawing.
	/// </summary>
	public class TextureList
	{
		Surface[] surfs = new Surface[4];

		/// <summary>
		/// Gets or sets the texture at the specified index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Surface this[int index]
		{
			get { return surfs[index]; }
			set { surfs[index] = value; }
		}
		/// <summary>
		/// Gets the number of textures supported.
		/// </summary>
		public int Count
		{
			get { return surfs.Length; }
		}
		/// <summary>
		/// Gets the number of textures which are assigned in this vertex buffer.
		/// </summary>
		public int ActiveTextures
		{
			get
			{
				int activeCount = 0;
				for (int i = 0; i < Count; i++)
				{
					if (this[i] == null)
						continue;

					activeCount++;
				}

				return activeCount;
			}
		}
	}

}
