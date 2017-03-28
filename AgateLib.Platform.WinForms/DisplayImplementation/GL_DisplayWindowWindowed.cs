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
using System.Linq;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.CoordinateSystems;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms.Controls;
using AgateLib.Quality;
using OpenTK.Graphics;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	/// No OpenGL code here.
	/// </summary>
	public sealed class GL_DisplayWindowWindowed : GL_DisplayWindow
	{
		public GL_DisplayWindowWindowed(DesktopGLDisplay display, DisplayWindow owner, CreateWindowParams windowParams)
			: base(display, owner, windowParams)
		{
			const string invalidMessage =
				"GL_DisplayWindowWindowed is the wrong type for this CreateWindowParams object. Use GL_DisplayWindowFullScreen instead.";

			Require.False<ArgumentException>(windowParams.IsFullScreen, invalidMessage);
			
			this.display = display;
			
			if (windowParams.RenderToControl)
			{
				wfRenderTarget = windowParams.RenderTarget as System.Windows.Forms.Control;

				if (wfRenderTarget == null)
					throw new AgateException($"The specified render target is of type {windowParams.RenderTarget.GetType().Name}, " +
					                         "which does not derive from System.Windows.Forms.Control.");

				chooseResolution = new Resolution(wfRenderTarget.Size.ToGeometry());

				if (wfRenderTarget.TopLevelControl == null)
					throw new ArgumentException("The specified render target has not been added to a Form yet.  " +
						"Check to make sure that you are creating the DisplayWindow after all controls are added " +
						"to the Form. Do not create a DisplayWindow in a constructor for a UserControl, for example.");

				InitializeContexts();

				AttachKeyboardEvents();

			}
			else
			{
				CreateWindowedDisplay(windowParams);
			}

			display.InitializeCurrentContext();

			InitializationComplete();
		}


		protected override Size ContextSize => wfRenderTarget.Size.ToGeometry();

		public override bool IsClosed => isClosed;

		public override bool IsFullScreen => false;

		private void AttachKeyboardEvents()
		{
			var form = TopLevelForm;

			form.KeyPreview = true;

			form.KeyDown += form_KeyDown;
			form.KeyUp += form_KeyUp;
		}

		private void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			OnInputEvent(owner, AgateInputEventArgs.KeyUp(e.AgateKeyCode(), e.AgateKeyModifiers()));
		}

		private void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			OnInputEvent(owner, AgateInputEventArgs.KeyDown(e.AgateKeyCode(), e.AgateKeyModifiers()));
		}

		private void CreateWindowedDisplay(CreateWindowParams createParams)
		{
			Require.True<InvalidOperationException>(chooseResolution != null,
				"The resolution must not be null. This is most likely caused by " +
				"Resolution in your CreateWindowParams being null.");

			ScreenInfo targetScreen = createParams.TargetScreen;
			var physicalSize = createParams.PhysicalSize ??
			                   createParams.Resolution.Size;
			
			DetachEvents();

			using (new ResourceDisposer(windowInfo, wfForm))
			{
				System.Windows.Forms.Form myform = null;
				System.Windows.Forms.Control myRenderTarget = null;

				display.EventThread.Invoke(() => 
					FormUtil.InitializeWindowsForm(
						out myform, out myRenderTarget, targetScreen, choosePosition,
						title, physicalSize, false, chooseResize, hasFrame));

				wfForm = myform;
				wfRenderTarget = myRenderTarget;

				ShowOwnedForm();

				InitializeContexts();
			}

			AgateApp.IsActive = true;
		}
	}
}
