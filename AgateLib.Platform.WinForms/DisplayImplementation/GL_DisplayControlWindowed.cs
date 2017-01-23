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
using System.Linq;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.Platform.WinForms.Controls;
using AgateLib.Quality;
using OpenTK.Graphics;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	/// No OpenGL code here.
	/// </summary>
	public sealed class GL_DisplayControlWindowed : GL_DisplayControl
	{
		public GL_DisplayControlWindowed(DesktopGLDisplay display, DisplayWindow owner, CreateWindowParams windowParams)
			: base(display, owner, windowParams)
		{
			const string invalidMessage =
				"GL_DisplayControlWindowed is the wrong type for this CreateWindowParams object. Use GL_DisplayControlFull instead.";

			Require.False<ArgumentException>(windowParams.IsFullScreen, invalidMessage);
			
			this.display = display;
			
			if (windowParams.RenderToControl)
			{
				wfRenderTarget = windowParams.RenderTarget as System.Windows.Forms.Control;

				if (wfRenderTarget == null)
					throw new AgateException($"The specified render target is of type {windowParams.RenderTarget.GetType().Name}, " +
					                         "which does not derive from System.Windows.Forms.Control.");

				windowInfo = CreateWindowInfo(CreateGraphicsMode());

				chooseResolution = new Resolution(wfRenderTarget.Size.ToGeometry());

				if (wfRenderTarget.TopLevelControl == null)
					throw new ArgumentException("The specified render target has not been added to a Form yet.  " +
						"Check to make sure that you are creating the DisplayWindow after all controls are added " +
						"to the Form. Do not create a DisplayWindow in a constructor for a UserControl, for example.");
				
				InitializeContexts();

				AttachEvents();
			}
			else
			{
				CreateWindowedDisplay(windowParams);

				_applicationContext.AddForm(wfForm);
			}

			display.InitializeCurrentContext();
		}

		protected override Size ContextSize => wfRenderTarget.Size.ToGeometry();

		public override bool IsClosed => isClosed;

		public override bool IsFullScreen => false;
		
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
