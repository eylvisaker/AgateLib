﻿//     The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
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

		public void WriteVertexData<T>(T[] data) where T:struct 
		{
			impl.Write(data);
		}
		public void Draw()
		{
			impl.Draw(0, VertexCount);
		}
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

		public int VertexCount
		{
			get { return impl.VertexCount; }
		}
		public PrimitiveType PrimitiveType
		{
			get { return impl.PrimitiveType; }
			set { impl.PrimitiveType = value; }
		}
		public TextureList Textures
		{
			get { return impl.Textures; }
		}

		public VertexLayout VertexLayout
		{
			get { return impl.VertexLayout; }
		}
	}

	public class TextureList
	{
		Surface[] surfs = new Surface[4];

		public Surface this[int index]
		{
			get { return surfs[index]; }
			set { surfs[index] = value; }
		}
		public int Count
		{
			get { return surfs.Length; }
		}
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