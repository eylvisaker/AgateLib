using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.ImplementationBase
{
	public abstract class FrameBufferImpl : IDisposable 
	{
		public abstract void Dispose();

		public abstract Size Size { get; }

		public int Width { get { return Size.Width; } }
		public int Height { get { return Size.Height; } }

		public abstract void BeginRender();
		public abstract void EndRender();

		public virtual bool CanAccessBackBuffer
		{
			get { return false; }
		}
		public virtual SurfaceImpl BackBuffer
		{
			get { throw new NotImplementedException(); }
		}
	}
}
