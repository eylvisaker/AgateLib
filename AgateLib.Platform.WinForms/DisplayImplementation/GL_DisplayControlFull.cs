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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.InputLib;
using AgateLib.OpenGL;
using AgateLib.Platform.WinForms.Controls;
using AgateLib.Quality;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.X11;
using FrameBuffer = AgateLib.OpenGL.GL3.FrameBuffer;
using Point = AgateLib.Geometry.Point;
using PointF = AgateLib.Geometry.PointF;
using Size = AgateLib.Geometry.Size;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	/// No OpenGL code here.
	/// </summary>
	public sealed class GL_DisplayControlFull : GL_DisplayControl, IPrimaryWindow
	{
		private FrameBuffer rtFrameBuffer;
		private GL_Surface rtSurface;
		private SurfaceState rtSurfaceState = new SurfaceState();
		private Screen targetScreen;

		public GL_DisplayControlFull(DesktopGLDisplay display, DisplayWindow owner, CreateWindowParams windowParams)
			: base(display, owner, windowParams)
		{
			const string invalidMessage =
				"GL_DisplayControlFull is the wrong type for this CreateWindowParams object. Use GL_DisplayControlWindowed instead.";

			Require.False<ArgumentException>(windowParams.RenderToControl, invalidMessage);
			Require.True<ArgumentException>(windowParams.IsFullScreen, invalidMessage);
			
			CreateFullScreenDisplay(Array.IndexOf(Screen.AllScreens, Screen.PrimaryScreen));

			_applicationContext.AddForm(wfForm);

			display.InitializeCurrentContext();
		}

		protected override Size ContextSize => targetScreen.Bounds.Size.ToGeometry();

		public override void Dispose()
		{
			ExitMessageLoop();

			if (ctxFrameBuffer != null)
			{
				ctxFrameBuffer.Dispose();
				ctxFrameBuffer = null;
			}

			if (wfForm != null)
			{
				if (wfForm.InvokeRequired)
				{
					wfForm.BeginInvoke(new Action(() => wfForm?.Dispose()));
				}

				wfForm.Dispose();
				wfForm = null;
			}
		}

		public override FrameBufferImpl FrameBuffer =>
			rtFrameBuffer ?? (FrameBufferImpl)ctxFrameBuffer;

		public override bool IsClosed => isClosed;

		public override bool IsFullScreen => true;

		public override Size Size
		{
			get { return rtSurface.SurfaceSize; }
			set
			{
				if (wfRenderTarget.InvokeRequired)
				{
					wfRenderTarget.Invoke(new Action(() => Size = value));
					return;
				}

				chooseResolution.Size = value;

				CreateTargetFrameBuffer(chooseResolution.Size);
			}
		}

		public override string Title
		{
			get { return wfForm?.Text; }
			set
			{
				if (wfForm != null)
				{
					if (wfForm.InvokeRequired)
					{
						wfForm.BeginInvoke(new Action(() => Title = value));
						return;
					}

					wfForm.Text = value;
				}
			}
		}
		
		private void CreateFullScreenDisplay(int targetScreenIndex)
		{
			DetachEvents();

			using (new ResourceDisposer(windowInfo, wfForm, rtSurface, rtFrameBuffer))
			{
				targetScreen = Screen.AllScreens[targetScreenIndex];

				wfForm = new frmFullScreen
				{
					ClientSize = targetScreen.Bounds.Size,
					Location = targetScreen.Bounds.Location,
					Text = title,
					Icon = icon,
					TopLevel = true
				};

				wfForm.Show();

				wfRenderTarget = wfForm;
				windowInfo = CreateWindowInfo(CreateGraphicsMode());
				CreateContextFrameBuffer(new NativeCoordinates());

				AttachEvents();

				wfForm.Activate();

				CreateTargetFrameBuffer(chooseResolution.Size);
			}

			Core.IsActive = true;
		}

		private void CreateTargetFrameBuffer(Size size)
		{
			using (new ResourceDisposer(rtSurface, rtFrameBuffer))
			{
				rtSurface = new GL_Surface(size);

				rtFrameBuffer = new FrameBuffer(rtSurface);
				rtFrameBuffer.RenderComplete += RtFrameBuffer_RenderComplete;
			}
		}

		private void RtFrameBuffer_RenderComplete(object sender, EventArgs e)
		{
			BlitBufferImage();
		}

		private void BlitBufferImage()
		{
			AgateBuiltInShaders.Basic2DShader.CoordinateSystem = ctxFrameBuffer.CoordinateSystem.Coordinates;
			AgateBuiltInShaders.Basic2DShader.Activate();

			ctxFrameBuffer.BeginRender();

			var destRect = new Rectangle(Point.Empty, ctxFrameBuffer.Size);

			rtSurfaceState.ScaleWidth = destRect.Width / (double) rtSurface.SurfaceWidth;
			rtSurfaceState.ScaleHeight = destRect.Height / (double) rtSurface.SurfaceHeight;

			rtSurfaceState.ScaleWidth = 1;
			rtSurfaceState.ScaleHeight = 1;

			rtSurfaceState.DrawInstances.Clear();
			rtSurfaceState.DrawInstances.Add(
				new SurfaceDrawInstance(destRect.Location));
			rtSurface.Draw(rtSurfaceState);

			display.DrawBuffer.Flush();
			ctxFrameBuffer.EndRender();
		}

		private static GraphicsMode CreateGraphicsMode()
		{
			GraphicsMode newMode = new GraphicsMode(
						 GraphicsMode.Default.ColorFormat, GraphicsMode.Default.Depth,
						 0, 0, new ColorFormat(0), 2, false);

			return newMode;
		}
	}
}
