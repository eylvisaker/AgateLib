using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;

namespace AgateLib.DisplayLib
{
	public class FrameBuffer
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

		public FrameBufferImpl Impl
		{
			get { return impl; }
		}
	}
}
