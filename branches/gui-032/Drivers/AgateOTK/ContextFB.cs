using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using AgateLib.Geometry;

namespace AgateOTK
{
	class ContextFB : GL_FrameBuffer 
	{
		IGraphicsContext mContext;
		IWindowInfo mWindowInfo;
		Size mSize;

		public ContextFB(IGraphicsContext context, IWindowInfo window, Size size, 
						 bool depthBuffer, bool stencilBuffer)
		{
			mContext = context;
			mWindowInfo = window;
			mSize = size;

			mHasDepth = depthBuffer;
			mHasStencil = stencilBuffer;
		}

		public override void Dispose()
		{
		}

		public override AgateLib.Geometry.Size Size
		{
			get { return mSize; }
		}

		public void SetSize(Size size)
		{
			mSize = size;
		}


		public override void MakeCurrent()
		{
			if (mContext.IsCurrent == false)
			{
				mContext.MakeCurrent(mWindowInfo);
			}

			GL.Viewport(0, 0, Width, Height);

		}

		public override void BeginRender()
		{
			MakeCurrent();

		}

		public override void EndRender()
		{
			bool vsync = AgateLib.DisplayLib.Display.RenderState.WaitForVerticalBlank;
			if (mContext.VSync != vsync)
				mContext.VSync = vsync;

			mContext.SwapBuffers();
		}

	}
}
