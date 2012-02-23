//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
		internal AgateLib.DisplayLib.DisplayWindow mAttachedWindow;

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

		public override AgateLib.DisplayLib.DisplayWindow AttachedWindow
		{
			get { return mAttachedWindow; }
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
