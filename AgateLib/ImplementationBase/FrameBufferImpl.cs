using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.ImplementationBase
{
	public abstract class FrameBufferImpl : IDisposable 
	{
		bool mIsDisposed = false;


		public abstract void Dispose();

		public abstract Size Size { get; }

		public int Width { get { return Size.Width; } }
		public int Height { get { return Size.Height; } }

	}
}
