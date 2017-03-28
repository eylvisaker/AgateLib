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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.CoordinateSystems;
using AgateLib.Quality;
using OpenTK.Graphics;
using FrameBuffer = AgateLib.OpenGL.GL3.FrameBuffer;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	/// No OpenGL code here.
	/// </summary>
	public sealed class GL_DisplayWindowFullScreen : GL_DisplayWindow
	{
		private System.Windows.Forms.Screen targetScreen;

		public GL_DisplayWindowFullScreen(DesktopGLDisplay display, DisplayWindow owner, CreateWindowParams windowParams)
			: base(display, owner, windowParams)
		{
			const string invalidMessage =
				"GL_DisplayWindowFullScreen is the wrong type for this CreateWindowParams object. Use GL_DisplayWindowWindowed instead.";

			Require.False<ArgumentException>(windowParams.RenderToControl, invalidMessage);
			Require.True<ArgumentException>(windowParams.IsFullScreen, invalidMessage);

			CreateFullScreenDisplay((int)windowParams.TargetScreen.SystemIndex);

			display.InitializeCurrentContext();

			InitializationComplete();
		}

		protected override Size ContextSize => targetScreen.Bounds.Size.ToGeometry();

		public override bool IsClosed => isClosed;

		public override bool IsFullScreen => true;

		private void CreateFullScreenDisplay(int targetScreenIndex)
		{
			DetachEvents();

			using (new ResourceDisposer(windowInfo, wfForm, rtFrameBuffer))
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

				wfRenderTarget = wfForm;

				ShowOwnedForm();

				InitializeContexts();
			}

			AgateApp.IsActive = true;
		}
	}
}
