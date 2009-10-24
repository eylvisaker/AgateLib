using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;

namespace AgateLib.ImplementationBase
{
	public abstract class IndexBufferImpl
	{
		public abstract void WriteIndices(short[] indices);
		public abstract void WriteIndices(int[] indices);

		public abstract int Count { get; }

		public abstract IndexBufferType IndexType { get; }
	}
}
