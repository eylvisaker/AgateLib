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
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.Quality;
using OpenTK.Graphics;
using FrameBuffer = AgateLib.OpenGL.GL3.FrameBuffer;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	/// No OpenGL code here.
	/// </summary>
	public sealed class GL_DisplayControlFull : GL_DisplayControl
	{
		private System.Windows.Forms.Screen targetScreen;

		public GL_DisplayControlFull(DesktopGLDisplay display, DisplayWindow owner, CreateWindowParams windowParams)
			: base(display, owner, windowParams)
		{
			const string invalidMessage =
				"GL_DisplayControlFull is the wrong type for this CreateWindowParams object. Use GL_DisplayControlWindowed instead.";

			Require.False<ArgumentException>(windowParams.RenderToControl, invalidMessage);
			Require.True<ArgumentException>(windowParams.IsFullScreen, invalidMessage);
			Require.True<InvalidOperationException>(windowParams.TargetScreen.DisplayWindow == null, 
				$"A full screen window already exists for {windowParams.TargetScreen.DeviceName}.");

			CreateFullScreenDisplay((int) windowParams.TargetScreen.SystemIndex);

			windowParams.TargetScreen.DisplayWindow = owner;

			_applicationContext.AddForm(wfForm);

			display.InitializeCurrentContext();
		}

		protected override void Dispose(bool disposing)
		{
			foreach (var screen in display.Screens.AllScreens)
			{
				if (screen.DisplayWindow == owner)
					screen.DisplayWindow = null;
			}

			base.Dispose(disposing);
		}

		protected override Size ContextSize => targetScreen.Bounds.Size.ToGeometry();

		public override bool IsClosed => isClosed;

		public override bool IsFullScreen => true;

		public override IResolution Resolution
		{
			get { return chooseResolution; }
			set
			{
				bool setRenderTarget = Display.RenderTarget == owner.FrameBuffer;

				chooseResolution = value.Clone();

				CreateTargetFrameBuffer(chooseResolution.Size);

				if (setRenderTarget)
					Display.RenderTarget = owner.FrameBuffer;
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
				targetScreen = System.Windows.Forms.Screen.AllScreens[targetScreenIndex];

				wfForm = display.EventThread.Invoke(() => new frmFullScreen
				{
					ClientSize = targetScreen.Bounds.Size,
					Location = targetScreen.Bounds.Location,
					Text = title,
					Icon = icon,
					TopLevel = true
				});

				wfForm.Show();

				wfRenderTarget = wfForm;
				windowInfo = CreateWindowInfo(CreateGraphicsMode());
				CreateContextFrameBuffer(new NativeCoordinates());

				AttachEvents();

				wfForm.Activate();

				CreateTargetFrameBuffer(chooseResolution.Size);

				CreateContextForCurrentThread();
			}

			Core.IsActive = true;
		}

		public override Point PixelToLogicalCoords(Point point)
		{
			var bufferPoint = chooseResolution.RenderMode
				?.MousePoint(point, rtSurface.SurfaceSize, ctxFrameBuffer.Size) ?? point;

			return base.PixelToLogicalCoords(bufferPoint);
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
