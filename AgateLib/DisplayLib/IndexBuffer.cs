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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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
