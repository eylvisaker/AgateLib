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
			if (impl.IndexType == IndexBufferType.Int32)
			{
				WriteIndices(ConvertToInt32(indices));
				return;
			}

			impl.WriteIndices(indices);
		}
		public void WriteIndices(int[] indices)
		{
			if (impl.IndexType == IndexBufferType.Int16)
			{
				WriteIndices(ConvertToInt16(indices));
				return;
			}

			impl.WriteIndices(indices);
		}

		private short[] ConvertToInt16(int[] indices)
		{
			short[] retval = new short[indices.Length];

			int i = 0;

			try
			{
				checked
				{
					for (i = 0; i < retval.Length; i++)
					{
						retval[i] = (short)indices[i];
					}
				}
			}
			catch (OverflowException ex)
			{
				throw new AgateLib.AgateException(ex, string.Format(
					"A 16 bit index buffer cannot contain values greater than {0}, " +
					"but there is a value of {1} at index {2}.",
					short.MaxValue, indices[i], i));
			}

			return retval;
		}
		private int[] ConvertToInt32(short[] indices)
		{
			int[] retval = new int[indices.Length];

			for (int i = 0; i < retval.Length; i++)
			{
				retval[i] = (short)indices[i];
			}

			return retval;
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
