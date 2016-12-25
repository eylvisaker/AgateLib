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
using System.Diagnostics;
using Drawing = System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.InputLib;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Platform;
using AgateLib.OpenGL;
using AgateLib.InputLib.Legacy;
using AgateLib.Platform.WinForms.Controls;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	/// No OpenGL code here.
	/// </summary>
	public sealed class GL_DisplayControl : DisplayWindowImpl, IPrimaryWindow
	{
		DisplayWindow owner;
		Form frm;
		Control renderTarget;
		IWindowInfo windowInfo;

		DesktopGLDisplay display;
		Drawing.Icon icon;
		bool isClosed = false;
		bool isFullScreen = false;

		string title;
		bool chooseFullscreen;
		int chooseWidth;
		int chooseHeight;
		int chooseBitDepth = 32;
		bool chooseResize;
		WindowPosition choosePosition;
		Point lastMousePoint;

		bool hasFrame = true;

		ContextFB frameBuffer;
		ICoordinateSystem coords;

		bool customMessageLoop;

		public GL_DisplayControl(DisplayWindow owner, CreateWindowParams windowParams)
		{
			this.owner = owner;
			choosePosition = windowParams.WindowPosition;
			coords = windowParams.Coordinates;

			if (windowParams.RenderToControl)
			{
				if (windowParams.RenderTarget is Control == false)
					throw new AgateException(string.Format("The specified render target is of type {0}, " +
						"which does not derive from System.Windows.Forms.Control.", windowParams.RenderTarget.GetType().Name));

				renderTarget = (Control)windowParams.RenderTarget;
				windowInfo = CreateWindowInfo(CreateGraphicsMode());

				if (renderTarget.TopLevelControl == null)
					throw new ArgumentException("The specified render target has not been added to a Form yet.  " +
						"Check to make sure that you are creating the DisplayWindow after all controls are added " +
						"to the Form.  Do not create a DisplayWindow in a constructor for a UserControl, for example.");

				chooseFullscreen = false;
				chooseWidth = renderTarget.ClientSize.Width;
				chooseHeight = renderTarget.ClientSize.Height;

				display = Display.Impl as DesktopGLDisplay;

				CreateFrameBuffer(coords);

				AttachEvents();
			}
			else
			{
				if (string.IsNullOrEmpty(windowParams.IconFile) == false)
					icon = new Drawing.Icon(windowParams.IconFile);

				title = windowParams.Title;
				chooseFullscreen = windowParams.IsFullScreen;
				chooseWidth = windowParams.Width;
				chooseHeight = windowParams.Height;
				chooseResize = windowParams.IsResizable;
				hasFrame = windowParams.HasFrame;

				if (chooseFullscreen)
					CreateFullScreenDisplay();
				else
					CreateWindowedDisplay();

				display = Display.Impl as DesktopGLDisplay;
			}

			display.InitializeCurrentContext();
		}

		public override FrameBufferImpl FrameBuffer => frameBuffer;

		private void CreateFullScreenDisplay()
		{
			DetachEvents();

			Form oldForm = frm;
			IWindowInfo oldWindowInfo = windowInfo;

			frm = new frmFullScreen();
			frm.Show();

			frm.Text = title;
			frm.Icon = icon;
			frm.TopLevel = true;

			renderTarget = frm;
			windowInfo = CreateWindowInfo(CreateGraphicsMode());

			AttachEvents();
			CreateFrameBuffer(coords);

			OpenTK.DisplayResolution resolution = OpenTK.DisplayDevice.Default.SelectResolution(
				chooseWidth, chooseHeight, 32, 0);
			OpenTK.DisplayDevice.Default.ChangeResolution(resolution);

			frm.Location = System.Drawing.Point.Empty;
			frm.ClientSize = new System.Drawing.Size(chooseWidth, chooseHeight);
			frm.Activate();

			System.Threading.Thread.Sleep(1000);
			isFullScreen = true;

			if (oldWindowInfo != null) oldWindowInfo.Dispose();
			if (oldForm != null) oldForm.Dispose();

			Core.IsActive = true;
		}

		private void CreateWindowedDisplay()
		{
			DetachEvents();

			Form oldForm = frm;
			IWindowInfo oldWindowInfo = windowInfo;

			isFullScreen = false;

			Form myform;
			Control myRenderTarget;

			OpenTK.DisplayDevice.Default.RestoreResolution();

			AgateLib.Platform.WinForms.Controls.FormUtil.InitializeWindowsForm(
				out myform, out myRenderTarget, choosePosition,
				title, chooseWidth, chooseHeight, chooseFullscreen, chooseResize, hasFrame);

			frm = myform;
			renderTarget = myRenderTarget;
			windowInfo = CreateWindowInfo(CreateGraphicsMode());

			if (icon != null)
				frm.Icon = icon;

			frm.Show();
			CreateFrameBuffer(coords);

			AttachEvents();

			if (oldWindowInfo != null) oldWindowInfo.Dispose();
			if (oldForm != null) oldForm.Dispose();

			Core.IsActive = true;
		}

		public OpenTK.Graphics.GraphicsContext CreateContext()
		{
			GraphicsMode newMode = CreateGraphicsMode();

			Debug.Print("AgateLib GraphicsMode: {0}", newMode);

			GraphicsContextFlags flags = GraphicsContextFlags.Default;
#if DEBUG
			//flags = GraphicsContextFlags.ForwardCompatible;
#endif
			var context = new OpenTK.Graphics.GraphicsContext(newMode, windowInfo, 3, 1, flags);
			context.MakeCurrent(windowInfo);
			(context as IGraphicsContextInternal).LoadAll();

			return context;
		}

		private static GraphicsMode CreateGraphicsMode()
		{
			GraphicsMode newMode = new GraphicsMode(
						 GraphicsMode.Default.ColorFormat, GraphicsMode.Default.Depth,
						 0, 0, new ColorFormat(0), 2, false);
			return newMode;
		}

		private void CreateFrameBuffer(ICoordinateSystem coords)
		{
			var old = frameBuffer;

			frameBuffer = new ContextFB(owner, CreateGraphicsMode(), windowInfo, this.Size, true, false, coords);

			if (old != null)
				old.Dispose();
		}

		private IWindowInfo CreateWindowInfo(GraphicsMode mode)
		{
			switch (AgateLib.Core.Platform.PlatformType)
			{
				case PlatformType.Windows:
					return OpenTK.Platform.Utilities.CreateWindowsWindowInfo(renderTarget.Handle);
				case PlatformType.MacOS:
					return OpenTK.Platform.Utilities.CreateMacOSCarbonWindowInfo(renderTarget.Handle, false, true);
				case PlatformType.Linux:
					return CreateX11WindowInfo(mode);
				default:
					throw new InvalidOperationException("Platform not implemented.");
			}
		}

		private IWindowInfo CreateX11WindowInfo(GraphicsMode mode)
		{
			Type xplatui = Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms");
			if (xplatui == null) throw new PlatformNotSupportedException(
					"System.Windows.Forms.XplatUIX11 missing. Unsupported platform or Mono runtime version, aborting.");

			// get the required handles from the X11 API.
			IntPtr display = (IntPtr)GetStaticFieldValue(xplatui, "DisplayHandle");
			IntPtr rootWindow = (IntPtr)GetStaticFieldValue(xplatui, "RootWindow");
			int screen = (int)GetStaticFieldValue(xplatui, "ScreenNo");

			// get the X11 Visual info for the display.
			XVisualInfo info = new XVisualInfo();
			info.VisualID = mode.Index.Value;
			int dummy;
			info = (XVisualInfo)Marshal.PtrToStructure(
				XGetVisualInfo(display, XVisualInfoMask.ID, ref info, out dummy), typeof(XVisualInfo));

			// set the X11 colormap.
			SetStaticFieldValue(xplatui, "CustomVisual", info.Visual);
			SetStaticFieldValue(xplatui, "CustomColormap",
				XCreateColormap(display, rootWindow, info.Visual, 0));

			IntPtr infoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(info));
			Marshal.StructureToPtr(info, infoPtr, false);

			IWindowInfo window = OpenTK.Platform.Utilities.CreateX11WindowInfo(
				display, screen, renderTarget.Handle, rootWindow, infoPtr);

			return window;

		}

		#region --- X11 imports ---

		[StructLayout(LayoutKind.Sequential)]
		struct XVisualInfo
		{
			public IntPtr Visual;
			public IntPtr VisualID;
			public int Screen;
			public int Depth;
			public OpenTK.Platform.X11.XVisualClass Class;
			public long RedMask;
			public long GreenMask;
			public long blueMask;
			public int ColormapSize;
			public int BitsPerRgb;

			public override string ToString()
			{
				return String.Format("id ({0}), screen ({1}), depth ({2}), class ({3})",
					VisualID, Screen, Depth, Class);
			}
		}
		[DllImport("libX11")]
		public static extern IntPtr XCreateColormap(IntPtr display, IntPtr window, IntPtr visual, int alloc);

		[DllImport("libX11", EntryPoint = "XGetVisualInfo")]
		static extern IntPtr XGetVisualInfoInternal(IntPtr display, IntPtr vinfo_mask, ref XVisualInfo template, out int nitems);

		static IntPtr XGetVisualInfo(IntPtr display, XVisualInfoMask vinfo_mask, ref XVisualInfo template, out int nitems)
		{
			return XGetVisualInfoInternal(display, (IntPtr)(int)vinfo_mask, ref template, out nitems);
		}

		[Flags]
		internal enum XVisualInfoMask
		{
			No = 0x0,
			ID = 0x1,
			Screen = 0x2,
			Depth = 0x4,
			Class = 0x8,
			Red = 0x10,
			Green = 0x20,
			Blue = 0x40,
			ColormapSize = 0x80,
			BitsPerRGB = 0x100,
			All = 0x1FF,
		}


		#endregion
		#region --- Utility functions for reading/writing non-public static fields through reflection ---

		private static object GetStaticFieldValue(Type type, string fieldName)
		{
			return type.GetField(fieldName,
				System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null);
		}
		private static void SetStaticFieldValue(Type type, string fieldName, object value)
		{
			type.GetField(fieldName,
				System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).SetValue(null, value);
		}

		#endregion

		public override void Dispose()
		{
			ExitMessageLoop();

			if (frameBuffer != null)
			{
				frameBuffer.Dispose();
				frameBuffer = null;
			}

			if (frm != null)
			{
				if (frm.InvokeRequired)
				{
					frm.BeginInvoke(new Action(frm.Close));
				}
				frm = null;
			}
		}

		private void AttachEvents()
		{
			if (renderTarget == null)
				return;

			renderTarget.Resize += new EventHandler(mRenderTarget_Resize);
			renderTarget.Disposed += new EventHandler(mRenderTarget_Disposed);

			renderTarget.MouseWheel += new MouseEventHandler(mRenderTarget_MouseWheel);
			renderTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
			renderTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
			renderTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
			renderTarget.DoubleClick += new EventHandler(mRenderTarget_DoubleClick);

			System.Windows.Forms.Form form = (renderTarget.TopLevelControl as System.Windows.Forms.Form);
			form.KeyPreview = true;

			form.KeyDown += new System.Windows.Forms.KeyEventHandler(form_KeyDown);
			form.KeyUp += new System.Windows.Forms.KeyEventHandler(form_KeyUp);

			form.FormClosing += new FormClosingEventHandler(form_FormClosing);
			form.FormClosed += new FormClosedEventHandler(form_FormClosed);
		}

		private void DetachEvents()
		{
			if (renderTarget == null)
				return;

			renderTarget.Resize -= new EventHandler(mRenderTarget_Resize);
			renderTarget.Disposed -= new EventHandler(mRenderTarget_Disposed);

			renderTarget.MouseWheel -= mRenderTarget_MouseWheel;
			renderTarget.MouseMove -= new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
			renderTarget.MouseDown -= new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
			renderTarget.MouseUp -= new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
			renderTarget.DoubleClick -= new EventHandler(mRenderTarget_DoubleClick);

			System.Windows.Forms.Form form = (renderTarget.TopLevelControl as System.Windows.Forms.Form);

			form.KeyDown -= new System.Windows.Forms.KeyEventHandler(form_KeyDown);
			form.KeyUp -= new System.Windows.Forms.KeyEventHandler(form_KeyUp);
			form.FormClosing -= new FormClosingEventHandler(form_FormClosing);
			form.FormClosed -= new FormClosedEventHandler(form_FormClosed);
		}

		void form_FormClosed(object sender, FormClosedEventArgs e)
		{
			OnClosed();
		}

		void form_FormClosing(object sender, FormClosingEventArgs e)
		{
			bool cancel = false;

			OnClosing(ref cancel);

			e.Cancel = cancel;
		}

		void mRenderTarget_Disposed(object sender, EventArgs e)
		{
			isClosed = true;
		}

		void mRenderTarget_Resize(object sender, EventArgs e)
		{
			frameBuffer.SetSize(new Size(renderTarget.Width, renderTarget.Height));

			OnResize();
		}

		void mRenderTarget_DoubleClick(object sender, EventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.MouseDoubleClick(lastMousePoint, MouseButton.Primary));
		}

		void mRenderTarget_MouseWheel(object sender, MouseEventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.MouseWheel(lastMousePoint, -(e.Delta * 100) / 120));
		}

		void pct_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseUp(lastMousePoint, e.AgateMousebutton()));
		}

		void pct_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseDown(lastMousePoint, e.AgateMousebutton()));
		}

		void pct_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseMove(lastMousePoint));
		}

		void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.KeyUp(e.AgateKeyCode(), e.AgateKeyModifiers()));
		}

		void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.KeyDown(e.AgateKeyCode(), e.AgateKeyModifiers()));
		}

		void renderTarget_Disposed(object sender, EventArgs e)
		{
			isClosed = true;
		}

		public override bool IsClosed => isClosed;

		public override bool IsFullScreen => isFullScreen;

		public override Size Size
		{
			get { return renderTarget.ClientSize.ToGeometry(); }
			set
			{
				renderTarget.ClientSize = value.ToDrawing();

				if (frm != null)
					frm.ClientSize = value.ToDrawing();
			}
		}

		public override string Title
		{
			get { return frm?.Text; }
			set
			{
				if (frm != null)
					frm.Text = value;
			}
		}

		#region GL_IRenderTarget Members


		public void HideCursor()
		{
			renderTarget.Cursor = FormUtil.BlankCursor;
		}
		public void ShowCursor()
		{
			renderTarget.Cursor = Cursors.Arrow;
		}

		#endregion

		void IPrimaryWindow.RunApplication()
		{
			if (Application.MessageLoop == false)
			{
				Application.Run(frm);
			}
			else
			{
				customMessageLoop = true;
				while (customMessageLoop)
				{
					Application.DoEvents();
				}
			}
		}


		public void ExitMessageLoop()
		{
			if (customMessageLoop)
				customMessageLoop = false;
			else
				Application.Exit();
		}


		public void CreateContextForThread()
		{
			frameBuffer.CreateContextForThread();
		}


		public Control TopLevelForm
		{
			get { return renderTarget.TopLevelControl; }
		}
	}
}
