using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.ImplementationBase;

namespace AgateOTK
{
	abstract class GL_FrameBuffer: FrameBufferImpl 
	{
		GLDrawBuffer mDrawBuffer = new GLDrawBuffer();

		public GLDrawBuffer DrawBuffer { get { return mDrawBuffer; } }
		public abstract void MakeCurrent();

		// TODO: fix this hack and remove these interface members.
		[Obsolete]
		public void HideCursor() { throw new NotImplementedException(); }
		[Obsolete]
		public void ShowCursor() { throw new NotImplementedException(); }

	}
}
