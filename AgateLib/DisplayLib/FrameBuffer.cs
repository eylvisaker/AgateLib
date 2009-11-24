using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;

namespace AgateLib.DisplayLib
{
	public class FrameBuffer : IDisposable 
	{
		FrameBufferImpl impl;

		public FrameBuffer(Size size)
		{
			impl = Display.Impl.CreateFrameBuffer(size);
		}
		public FrameBuffer(int width, int height)
			: this(new Size(width, height))
		{ }
		public FrameBuffer(FrameBufferImpl impl)
		{
			this.impl = impl;
		}

		public void Dispose()
		{
			impl.Dispose();
		}

		public FrameBufferImpl Impl
		{
			get { return impl; }
		}

		public Size Size
		{
			get { return Impl.Size; }
		}
		public int Height
		{
			get { return Impl.Height; }
		}
		public int Width
		{
			get { return Impl.Width; }
		}

		public void BeginRender()
		{
		}
		public void EndRender()
		{ }

		public bool CanAccessBackBuffer
		{
			get { return Impl.CanAccessBackBuffer; }
		}

		Surface mBackBuffer;

		public Surface BackBuffer
		{
			get
			{
				if (mBackBuffer != null && mBackBuffer.Impl != Impl.BackBuffer)
				{
					mBackBuffer.Dispose();
					mBackBuffer = null;
				}

				if (mBackBuffer == null)
					mBackBuffer = new Surface(Impl.BackBuffer);

				return mBackBuffer;
			}
		}
	}
}
