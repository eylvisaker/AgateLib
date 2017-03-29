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
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.ImplementationBase;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which represents an index buffer in video memory.  An index buffer is
	/// used to reduce the amount of vertex data that needs to be stored/sent to the
	/// graphics adapter.
	/// </summary>
	public class IndexBuffer : IDisposable
	{
		IndexBufferImpl impl;

		/// <summary>
		/// Constructs an index buffer.
		/// </summary>
		/// <param name="type">The type of index buffer to create, 16 or 32 bit indices.</param>
		/// <param name="size">The number of indices that are contained in the index buffer.</param>
		public IndexBuffer(IndexBufferType type, int size)
		{
			impl = Display.Impl.CreateIndexBuffer(type, size);
		}

		/// <summary>
		/// Disposes of the buffer.
		/// </summary>
		public void Dispose()
		{
			impl.Dispose();
		}

		/// <summary>
		/// Writes indices to the index buffer.
		/// </summary>
		/// <param name="indices">The indices to write.</param>
		public void WriteIndices(short[] indices)
		{
			if (impl.IndexType == IndexBufferType.Int32)
			{
				WriteIndices(ConvertToInt32(indices));
				return;
			}

			impl.WriteIndices(indices);
		}
		/// <summary>
		/// Writes indices to the index buffer.
		/// </summary>
		/// <param name="indices">The indices to write.  If this is a 16 bit index buffer, you must not 
		/// have an index greater than 32767.</param>
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
			short[] result = new short[indices.Length];

			int i = 0;

			try
			{
				checked
				{
					for (i = 0; i < result.Length; i++)
					{
						result[i] = (short)indices[i];
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

			return result;
		}
		private int[] ConvertToInt32(short[] indices)
		{
			int[] result = new int[indices.Length];

			for (int i = 0; i < result.Length; i++)
			{
				result[i] = (short)indices[i];
			}

			return result;
		}

		/// <summary>
		/// Gets the number of indices in the buffer.
		/// </summary>
		public int Count
		{
			get { return impl.Count; }
		}

		/// <summary>
		/// Gets the index buffer implementation.
		/// </summary>
		public IndexBufferImpl Impl
		{
			get { return impl; }
		}
	}

	/// <summary>
	/// Enum indicating the type of index buffer.
	/// </summary>
	public enum IndexBufferType
	{
		/// <summary>
		/// A 16-bit index buffer.
		/// </summary>
		Int16,
		/// <summary>
		/// A 32-bit index buffer.
		/// </summary>
		Int32,
	}
}
