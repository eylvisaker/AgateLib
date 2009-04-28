using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;

namespace AgateLib.DisplayLib
{
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

		public void WriteVertexData<T>(T[] data)
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
