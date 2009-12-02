using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;

namespace AgateLib.ImplementationBase
{
	/// <summary>
	/// Base class for implementing a hardware stored index buffer.
	/// </summary>
	public abstract class IndexBufferImpl
	{
		/// <summary>
		/// Writes indices to the index buffer.
		/// </summary>
		/// <param name="indices"></param>
		public abstract void WriteIndices(short[] indices);
		/// <summary>
		/// Writes indices to the index buffer.
		/// </summary>
		/// <param name="indices"></param>
		public abstract void WriteIndices(int[] indices);

		/// <summary>
		/// Gets the number of indices in the index buffer.
		/// </summary>
		public abstract int Count { get; }

		/// <summary>
		/// Gets the type of indices in the index buffer.
		/// </summary>
		public abstract IndexBufferType IndexType { get; }
	}
}
