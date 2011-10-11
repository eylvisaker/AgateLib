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
		readonly GLDrawBuffer mDrawBuffer;
		protected bool mHasDepth;
		protected bool mHasStencil;

		protected GL_FrameBuffer()
		{
			mDrawBuffer = ((GL_Display)Display.Impl).CreateDrawBuffer();
		}

		public GLDrawBuffer DrawBuffer { get { return mDrawBuffer; } }
		public abstract void MakeCurrent();

		public override bool HasDepthBuffer
		{
			get { return mHasDepth; }
		}
		public override bool HasStencilBuffer
		{
			get { return mHasStencil; }
		}

	}
}
