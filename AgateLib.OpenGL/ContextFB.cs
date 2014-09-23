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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
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
using System.Threading;
using System.Diagnostics;

namespace AgateLib.OpenGL
{
	public class ContextFB : GL_FrameBuffer
	{
		Dictionary<Thread, GraphicsContext> mContexts = new Dictionary<Thread, GraphicsContext>();

		GraphicsMode mGraphicsMode;
		IWindowInfo mWindowInfo;
		Size mSize;
		AgateLib.DisplayLib.DisplayWindow mAttachedWindow;
		bool mIsDisposed;

		public ContextFB(AgateLib.DisplayLib.DisplayWindow attachedWindow,
						 GraphicsMode graphicsMode, IWindowInfo window, Size size,
						 bool depthBuffer, bool stencilBuffer,
						 ICoordinateSystem coords)
			: base(coords)
		{
			mGraphicsMode = graphicsMode;
			mAttachedWindow = attachedWindow;

			mWindowInfo = window;
			mSize = size;

			mHasDepth = depthBuffer;
			mHasStencil = stencilBuffer;

			CreateContextForThread();

			InitializeDrawBuffer();
		}

		public void CreateContextForThread()
		{
			if (IsDisposed)
				throw new InvalidOperationException("Cannot create a context for a framebuffer which is disposed.");
			if (mContexts.ContainsKey(Thread.CurrentThread))
				return;

			var context = new GraphicsContext(mGraphicsMode, mWindowInfo);
			mContexts.Add(Thread.CurrentThread, context);

			context.LoadAll();
			context.MakeCurrent(mWindowInfo);

			Debug.WriteLine(string.Format("Created context {0} for thread {1}",
				context.ToString(), Thread.CurrentThread.ManagedThreadId));
		}

		public bool IsDisposed { get { return mIsDisposed; } }

		GraphicsContext CurrentContext
		{
			get
			{
				if (mContexts.ContainsKey(Thread.CurrentThread) == false)
					return null;

				return mContexts[Thread.CurrentThread];
			}
		}
		public override void Dispose()
		{
			if (mIsDisposed)
				return;

			foreach(var context in mContexts.Values)
			{
				context.Dispose();
			}

			mContexts.Clear();
			mIsDisposed = true;
		}

		public override AgateLib.Geometry.Size Size
		{
			get { return mSize; }
		}

		public void SetSize(Size size)
		{
			mSize = size;

			foreach (var context in mContexts.Values)
				context.Update(mWindowInfo);
		}

		public override AgateLib.DisplayLib.DisplayWindow AttachedWindow
		{
			get { return mAttachedWindow; }
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
			if (CurrentContext.VSync != vsync)
				CurrentContext.VSync = vsync;

			CurrentContext.SwapBuffers();
		}

	}
}
