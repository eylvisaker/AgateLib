using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;

namespace AgateOTK
{
	abstract class GL_FrameBuffer: FrameBufferImpl 
	{
		GLDrawBuffer mDrawBuffer;

		public GL_FrameBuffer()
		{
			mDrawBuffer = ((GL_Display)Display.Impl).CreateDrawBuffer();
		}

		public GLDrawBuffer DrawBuffer { get { return mDrawBuffer; } }
		public abstract void MakeCurrent();

		// TODO: fix this hack and remove these interface members.
		[Obsolete]
		public void HideCursor() { throw new NotImplementedException(); }
		[Obsolete]
		public void ShowCursor() { throw new NotImplementedException(); }

	}
}
