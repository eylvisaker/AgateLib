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
		DisplayWindow mOwner;
		Form frm;
		Control mRenderTarget;
		IWindowInfo mWindowInfo;

		DesktopGLDisplay mDisplay;
		Drawing.Icon mIcon;
		bool mIsClosed = false;
		bool mIsFullScreen = false;

		string mTitle;
		bool mChooseFullscreen;
		int mChooseWidth;
		int mChooseHeight;
		int mChooseBitDepth = 32;
		bool mChooseResize;
		WindowPosition mChoosePosition;

		bool mHasFrame = true;

		ContextFB mFrameBuffer;
		ICoordinateSystem mCoords;

		public override FrameBufferImpl FrameBuffer
		{
			get { return mFrameBuffer; }
		}
		public GL_DisplayControl(DisplayWindow owner, CreateWindowParams windowParams)
		{
			mOwner = owner;
			mChoosePosition = windowParams.WindowPosition;
			mCoords = windowParams.Coordinates;

			if (windowParams.RenderToControl)
			{
				if (windowParams.RenderTarget is Control == false)
					throw new AgateException(string.Format("The specified render target is of type {0}, " +
						"which does not derive from System.Windows.Forms.Control.", windowParams.RenderTarget.GetType().Name));

				mRenderTarget = (Control)windowParams.RenderTarget;
				mWindowInfo = CreateWindowInfo(CreateGraphicsMode());

				if (mRenderTarget.TopLevelControl == null)
					throw new ArgumentException("The specified render target has not been added to a Form yet.  " +
						"Check to make sure that you are creating the DisplayWindow after all controls are added " +
						"to the Form.  Do not create a DisplayWindow in a constructor for a UserControl, for example.");

				mChooseFullscreen = false;
				mChooseWidth = mRenderTarget.ClientSize.Width;
				mChooseHeight = mRenderTarget.ClientSize.Height;

				mDisplay = Display.Impl as DesktopGLDisplay;

				CreateFrameBuffer(mCoords);

				AttachEvents();
			}
			else
			{
				if (string.IsNullOrEmpty(windowParams.IconFile) == false)
					mIcon = new Drawing.Icon(windowParams.IconFile);

				mTitle = windowParams.Title;
				mChooseFullscreen = windowParams.IsFullScreen;
				mChooseWidth = windowParams.Width;
				mChooseHeight = windowParams.Height;
				mChooseResize = windowParams.IsResizable;
				mHasFrame = windowParams.HasFrame;

				if (mChooseFullscreen)
					CreateFullScreenDisplay();
				else
					CreateWindowedDisplay();

				mDisplay = Display.Impl as DesktopGLDisplay;
			}

			mDisplay.InitializeCurrentContext();
		}

		private void CreateFullScreenDisplay()
		{
			DetachEvents();

			Form oldForm = frm;
			IWindowInfo oldWindowInfo = mWindowInfo;

			frm = new frmFullScreen();
			frm.Show();

			frm.Text = mTitle;
			frm.Icon = mIcon;
			frm.TopLevel = true;

			mRenderTarget = frm;
			mWindowInfo = CreateWindowInfo(CreateGraphicsMode());

			AttachEvents();
			CreateFrameBuffer(mCoords);

			OpenTK.DisplayResolution resolution = OpenTK.DisplayDevice.Default.SelectResolution(
				mChooseWidth, mChooseHeight, 32, 0);
			OpenTK.DisplayDevice.Default.ChangeResolution(resolution);

			frm.Location = System.Drawing.Point.Empty;
			frm.ClientSize = new System.Drawing.Size(mChooseWidth, mChooseHeight);
			frm.Activate();

			System.Threading.Thread.Sleep(1000);
			mIsFullScreen = true;

			if (oldWindowInfo != null) oldWindowInfo.Dispose();
			if (oldForm != null) oldForm.Dispose();

			Core.IsActive = true;
		}

		private void CreateWindowedDisplay()
		{
			DetachEvents();

			Form oldForm = frm;
			IWindowInfo oldWindowInfo = mWindowInfo;

			mIsFullScreen = false;

			Form myform;
			Control myRenderTarget;

			OpenTK.DisplayDevice.Default.RestoreResolution();

			AgateLib.Platform.WinForms.Controls.FormUtil.InitializeWindowsForm(
				out myform, out myRenderTarget, mChoosePosition,
				mTitle, mChooseWidth, mChooseHeight, mChooseFullscreen, mChooseResize, mHasFrame);

			frm = myform;
			mRenderTarget = myRenderTarget;
			mWindowInfo = CreateWindowInfo(CreateGraphicsMode());

			if (mIcon != null)
				frm.Icon = mIcon;

			frm.Show();
			CreateFrameBuffer(mCoords);

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
			var context = new OpenTK.Graphics.GraphicsContext(newMode, mWindowInfo, 3, 1, flags);
			context.MakeCurrent(mWindowInfo);
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
			var old = mFrameBuffer;

			mFrameBuffer = new ContextFB(mOwner, CreateGraphicsMode(), mWindowInfo, this.Size, true, false, coords);

			if (old != null)
				old.Dispose();
		}

		private IWindowInfo CreateWindowInfo(GraphicsMode mode)
		{
			switch (AgateLib.Core.Platform.PlatformType)
			{
				case PlatformType.Windows:
					return OpenTK.Platform.Utilities.CreateWindowsWindowInfo(mRenderTarget.Handle);
				case PlatformType.MacOS:
					return OpenTK.Platform.Utilities.CreateMacOSCarbonWindowInfo(mRenderTarget.Handle, false, true);
				case PlatformType.Linux:
					return CreateX11WindowInfo(mode);
				case PlatformType.Gp2x:
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
				display, screen, mRenderTarget.Handle, rootWindow, infoPtr);

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
			if (mFrameBuffer != null)
			{
				mFrameBuffer.Dispose();
				mFrameBuffer = null;
			}

			if (frm != null)
			{
				frm.Close();
				frm = null;
			}

		}

		private void AttachEvents()
		{
			if (mRenderTarget == null)
				return;

			mRenderTarget.Resize += new EventHandler(mRenderTarget_Resize);
			mRenderTarget.Disposed += new EventHandler(mRenderTarget_Disposed);

			mRenderTarget.MouseWheel += new MouseEventHandler(mRenderTarget_MouseWheel);
			mRenderTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
			mRenderTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
			mRenderTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
			mRenderTarget.DoubleClick += new EventHandler(mRenderTarget_DoubleClick);

			System.Windows.Forms.Form form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);
			form.KeyPreview = true;

			form.KeyDown += new System.Windows.Forms.KeyEventHandler(form_KeyDown);
			form.KeyUp += new System.Windows.Forms.KeyEventHandler(form_KeyUp);

			form.FormClosing += new FormClosingEventHandler(form_FormClosing);
			form.FormClosed += new FormClosedEventHandler(form_FormClosed);
		}

		private void DetachEvents()
		{
			if (mRenderTarget == null)
				return;

			mRenderTarget.Resize -= new EventHandler(mRenderTarget_Resize);
			mRenderTarget.Disposed -= new EventHandler(mRenderTarget_Disposed);

			mRenderTarget.MouseWheel -= mRenderTarget_MouseWheel;
			mRenderTarget.MouseMove -= new System.Windows.Forms.MouseEventHandler(pct_MouseMove);
			mRenderTarget.MouseDown -= new System.Windows.Forms.MouseEventHandler(pct_MouseDown);
			mRenderTarget.MouseUp -= new System.Windows.Forms.MouseEventHandler(pct_MouseUp);
			mRenderTarget.DoubleClick -= new EventHandler(mRenderTarget_DoubleClick);

			System.Windows.Forms.Form form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);

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

		InputLib.MouseButton GetButtons(System.Windows.Forms.MouseButtons buttons)
		{
			var result = InputLib.MouseButton.None;

			if ((buttons & System.Windows.Forms.MouseButtons.Left) != 0)
				result |= InputLib.MouseButton.Primary;
			if ((buttons & System.Windows.Forms.MouseButtons.Right) != 0)
				result |= InputLib.MouseButton.Secondary;
			if ((buttons & System.Windows.Forms.MouseButtons.Middle) != 0)
				result |= InputLib.MouseButton.Middle;
			if ((buttons & System.Windows.Forms.MouseButtons.XButton1) != 0)
				result |= InputLib.MouseButton.ExtraButton1;
			if ((buttons & System.Windows.Forms.MouseButtons.XButton2) != 0)
				result |= InputLib.MouseButton.ExtraButton2;

			return result;
		}

		void mRenderTarget_Disposed(object sender, EventArgs e)
		{
			mIsClosed = true;
		}
		void mRenderTarget_Resize(object sender, EventArgs e)
		{
			mFrameBuffer.SetSize(new Size(mRenderTarget.Width, mRenderTarget.Height));

			OnResize();
		}

		void mRenderTarget_DoubleClick(object sender, EventArgs e)
		{
			Mouse.OnMouseDoubleClick(InputLib.MouseButton.Primary);
		}
		void mRenderTarget_MouseWheel(object sender, MouseEventArgs e)
		{
			Mouse.OnMouseWheel(-(e.Delta * 100) / 120);
		}
		void pct_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			var btn = GetButtons(e.Button);

			//Mouse.Buttons[btn] = false;

			Input.QueueInputEvent(AgateInputEventArgs.MouseUp(
				this, PixelToLogicalCoords(new Point(e.X, e.Y)), btn));
		}
		void pct_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			var btn = GetButtons(e.Button);

			//Mouse.Buttons[btn] = true;
			Input.QueueInputEvent(AgateInputEventArgs.MouseDown(
				this, PixelToLogicalCoords(new Point(e.X, e.Y)), btn));
		}
		void pct_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//SetInternalMousePosition(e.Location));
			Input.QueueInputEvent(AgateInputEventArgs.MouseMove(
				this, PixelToLogicalCoords(new Point(e.X, e.Y))));
		}
		void renderTarget_Disposed(object sender, EventArgs e)
		{
			mIsClosed = true;
		}

		void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = false;
		}
		void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			var key = FormUtil.TransformWinFormsKey(e.KeyCode);

			Keyboard.Keys[key] = true;
		}

		public override bool IsClosed
		{
			get { return mIsClosed; }
		}

		public override bool IsFullScreen
		{
			get
			{
				return mIsFullScreen;
			}
		}


		public override Size Size
		{
			get
			{
				return mRenderTarget.ClientSize.ToGeometry();
			}
			set
			{
				mRenderTarget.ClientSize = value.ToDrawing();

				if (frm != null)
					frm.ClientSize = value.ToDrawing();
			}
		}

		public override string Title
		{
			get
			{
				if (frm != null)
					return frm.Text;
				else
					return null;
			}
			set
			{
				if (frm != null)
					frm.Text = value;
			}
		}
		[Obsolete]
		public override Point MousePosition
		{
			get
			{
				return mRenderTarget.PointToClient(Cursor.Position).ToGeometry();
			}
			set
			{
				Cursor.Position = mRenderTarget.PointToScreen(value.ToDrawing());
			}
		}


		#region GL_IRenderTarget Members


		public void HideCursor()
		{
			mRenderTarget.Cursor = FormUtil.BlankCursor;
		}
		public void ShowCursor()
		{
			mRenderTarget.Cursor = Cursors.Arrow;
		}

		#endregion

		bool customMessageLoop;

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
			mFrameBuffer.CreateContextForThread();
		}


		public Control TopLevelForm
		{
			get { return mRenderTarget.TopLevelControl; }
		}
	}
}
