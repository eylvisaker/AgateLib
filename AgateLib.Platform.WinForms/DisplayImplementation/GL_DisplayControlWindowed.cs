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
	public sealed class GL_DisplayControlWindowed : GL_DisplayControl, IPrimaryWindow
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
				if (windowParams.RenderTarget is Control == false)
					throw new AgateException(string.Format("The specified render target is of type {0}, " +
						"which does not derive from System.Windows.Forms.Control.", windowParams.RenderTarget.GetType().Name));

				wfRenderTarget = (Control)windowParams.RenderTarget;
				windowInfo = CreateWindowInfo(CreateGraphicsMode());

				chooseResolution = new Resolution(wfRenderTarget.Size.ToGeometry());

				if (wfRenderTarget.TopLevelControl == null)
					throw new ArgumentException("The specified render target has not been added to a Form yet.  " +
						"Check to make sure that you are creating the DisplayWindow after all controls are added " +
						"to the Form. Do not create a DisplayWindow in a constructor for a UserControl, for example.");
				
				CreateContextFrameBuffer(coords);

				AttachEvents();
			}
			else
			{
				CreateWindowedDisplay();

				_applicationContext.AddForm(wfForm);
			}

			display.InitializeCurrentContext();
		}

		protected override Size ContextSize => Size;

		public override bool IsClosed => isClosed;

		public override bool IsFullScreen => false;

		public override IResolution Resolution
		{
			get { return chooseResolution; }
			set
			{
				if (wfRenderTarget.InvokeRequired)
				{
					wfRenderTarget.Invoke(new Action(() => Resolution = value));
					return;
				}

				chooseResolution = value;

				wfRenderTarget.ClientSize = chooseResolution.Size.ToDrawing();

				if (wfForm != null)
				{
					wfForm.ClientSize = chooseResolution.Size.ToDrawing();
				}
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
		
		private void CreateWindowedDisplay()
		{
			DetachEvents();

			using (new ResourceDisposer(windowInfo, wfForm))
			{
				Form myform;
				Control myRenderTarget;

				FormUtil.InitializeWindowsForm(
					out myform, out myRenderTarget, choosePosition,
					title, chooseResolution.Size, false, chooseResize, hasFrame);

				wfForm = myform;
				wfRenderTarget = myRenderTarget;
				windowInfo = CreateWindowInfo(CreateGraphicsMode());

				if (icon != null)
					wfForm.Icon = icon;

				wfForm.Show();
				CreateContextFrameBuffer(coords);

				AttachEvents();
			}

			Core.IsActive = true;
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
