using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.ImplementationBase;

namespace AgateLib.DisplayLib
{
	public class IndexBuffer
	{
		IndexBufferImpl impl;

		public IndexBuffer(IndexBufferType type, int size)
		{
			impl = Display.Impl.CreateIndexBuffer(type, size);
		}

		public void WriteIndices(short[] indices)
		{
			impl.WriteIndices(indices);
		}
		public void WriteIndices(int[] indices)
		{
			impl.WriteIndices(indices);
		}

		public int Count
		{
			get { return impl.Count; }
		}

		public IndexBufferImpl Impl
		{
			get { return impl; }
		}
	}

	public enum IndexBufferType
	{
		Int16,
		Int32,
	}
}
