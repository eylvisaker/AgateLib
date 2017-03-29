//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Threading;
using System.Diagnostics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;

namespace AgateLib.OpenGL
{
	public class ContextFrameBuffer : GL_FrameBuffer
	{
		Dictionary<Thread, GraphicsContext> mContexts = new Dictionary<Thread, GraphicsContext>();

		GraphicsMode mGraphicsMode;
		IWindowInfo mWindowInfo;
		Size mSize;
		AgateLib.DisplayLib.DisplayWindow mAttachedWindow;
		bool mIsDisposed;

		public ContextFrameBuffer(AgateLib.DisplayLib.DisplayWindow attachedWindow,
						 GraphicsMode graphicsMode, IWindowInfo window, Size size,
						 bool depthBuffer, bool stencilBuffer,
						 ICoordinateSystem coords)
			: base(coords)
		{
			Require.ArgumentNotNull(window, "WindowInfo must not be null.");
			mGraphicsMode = graphicsMode;
			mAttachedWindow = attachedWindow;

			mWindowInfo = window;
			mSize = size;

			CoordinateSystem.RenderTargetSize = mSize;
			
			hasDepth = depthBuffer;
			hasStencil = stencilBuffer;

			CreateContextForThread();

			InitializeDrawBuffer();
		}

		public override void Dispose()
		{
			if (mIsDisposed)
				return;

			foreach (var context in mContexts.Values)
			{
				context.Dispose();
			}

			mContexts.Clear();
			mIsDisposed = true;
		}

		GraphicsContext CurrentContext
		{
			get
			{
				if (mContexts.ContainsKey(Thread.CurrentThread) == false)
					return null;

				return mContexts[Thread.CurrentThread];
			}
		}

		public bool IsDisposed => mIsDisposed;

		public override AgateLib.Mathematics.Geometry.Size Size => mSize;

		public void SetSize(Size size)
		{
			mSize = size;

			foreach (var context in mContexts.Values)
				context.Update(mWindowInfo);

			CoordinateSystem.RenderTargetSize = mSize;
		}

		public override AgateLib.DisplayLib.DisplayWindow AttachedWindow => mAttachedWindow;

		public void CreateContextForThread()
		{
			if (IsDisposed)
				throw new InvalidOperationException("Cannot create a context for a framebuffer which is disposed.");
			if (mContexts.ContainsKey(Thread.CurrentThread))
				return;

			GraphicsContext.ShareContexts = true;
			var context = new GraphicsContext(mGraphicsMode, mWindowInfo);
			mContexts.Add(Thread.CurrentThread, context);

			context.LoadAll();
			context.MakeCurrent(mWindowInfo);

			Debug.WriteLine($"Created context {context} for thread {Thread.CurrentThread.ManagedThreadId}");
		}

		public override void MakeCurrent()
		{
			if (CurrentContext.IsCurrent == false)
			{
				CurrentContext.MakeCurrent(mWindowInfo);
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

			if (vsync)
				CurrentContext.SwapInterval = -1;
			else
				CurrentContext.SwapInterval = 0;

			CurrentContext.SwapBuffers();

			OnRenderComplete();
		}

	}
}
