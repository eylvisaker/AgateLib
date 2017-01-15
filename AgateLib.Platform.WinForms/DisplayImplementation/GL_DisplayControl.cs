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
	public abstract class GL_DisplayControl : DisplayWindowImpl, IPrimaryWindow
	{
		static protected DisplayControlContext _applicationContext { get; private set; }

		protected readonly DisplayWindow owner;
		protected Form wfForm;
		protected Control wfRenderTarget;
		protected IWindowInfo windowInfo;
		
		protected DesktopGLDisplay display;
		protected readonly System.Drawing.Icon icon;
		protected bool isClosed;
		
		protected readonly string title;
		protected readonly Resolution chooseResolution;
		protected readonly bool chooseResize;
		protected readonly WindowPosition choosePosition;
		protected Point lastMousePoint;
		
		protected readonly bool hasFrame = true;
		
		protected ContextFrameBuffer ctxFrameBuffer;
		
		protected readonly ICoordinateSystem coords;

		public GL_DisplayControl(DesktopGLDisplay display, DisplayWindow owner, CreateWindowParams windowParams)
		{
			this.owner = owner;
			choosePosition = windowParams.WindowPosition;
			coords = windowParams.Coordinates;

			this.display = display;

			if (_applicationContext == null)
			{
				_applicationContext = new DisplayControlContext();
			}

			if (string.IsNullOrEmpty(windowParams.IconFile) == false)
				icon = new System.Drawing.Icon(windowParams.IconFile);

			title = windowParams.Title;
			chooseResolution = windowParams.Resolution;
			chooseResize = windowParams.IsResizable;
			hasFrame = windowParams.HasFrame;
		}

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

		Form TopLevelForm => (Form)wfRenderTarget.TopLevelControl;

		public override FrameBufferImpl FrameBuffer => ctxFrameBuffer;

		public override bool IsClosed => isClosed;

		public override Size Size
		{
			get { return wfRenderTarget.ClientSize.ToGeometry(); }
			set
			{
				if (wfRenderTarget.InvokeRequired)
				{
					wfRenderTarget.Invoke(new Action(() => Size = value));
					return;
				}

				wfRenderTarget.ClientSize = value.ToDrawing();

				if (wfForm != null)
				{
					wfForm.ClientSize = value.ToDrawing();
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

		public void HideCursor()
		{
			if (wfRenderTarget.InvokeRequired)
			{
				wfRenderTarget.BeginInvoke(new Action(HideCursor));
				return;
			}

			wfRenderTarget.Cursor = FormUtil.BlankCursor;
		}

		public void ShowCursor()
		{
			if (wfRenderTarget.InvokeRequired)
			{
				wfRenderTarget.BeginInvoke(new Action(ShowCursor));
				return;
			}

			wfRenderTarget.Cursor = Cursors.Arrow;
		}

		public void ExitMessageLoop()
		{
			_applicationContext?.ExitThread();
		}

		public void CreateContextForThread()
		{
			ctxFrameBuffer.CreateContextForThread();
		}
		
		public GraphicsContext CreateContext()
		{
			GraphicsMode newMode = CreateGraphicsMode();

			Debug.Print("AgateLib GraphicsMode: {0}", newMode);

			GraphicsContextFlags flags = GraphicsContextFlags.Default;
#if DEBUG
			//flags = GraphicsContextFlags.ForwardCompatible;
#endif
			var context = new GraphicsContext(newMode, windowInfo, 3, 1, flags);
			context.MakeCurrent(windowInfo);
			context.LoadAll();

			return context;
		}

		private static GraphicsMode CreateGraphicsMode()
		{
			GraphicsMode newMode = new GraphicsMode(
						 GraphicsMode.Default.ColorFormat, GraphicsMode.Default.Depth,
						 0, 0, new ColorFormat(0), 2, false);

			return newMode;
		}

		private void CreateContextFrameBuffer(ICoordinateSystem fbCoords)
		{
			using (new ResourceDisposer(ctxFrameBuffer))
			{
				ctxFrameBuffer = new ContextFrameBuffer(owner, CreateGraphicsMode(), windowInfo, Size, true, false, fbCoords);
			}
		}

		protected IWindowInfo CreateWindowInfo(GraphicsMode mode)
		{
			switch (Core.Platform.PlatformType)
			{
				case PlatformType.Windows:
					return Utilities.CreateWindowsWindowInfo(wfRenderTarget.Handle);
				case PlatformType.MacOS:
					return Utilities.CreateMacOSCarbonWindowInfo(wfRenderTarget.Handle, false, true);
				case PlatformType.Linux:
					return CreateX11WindowInfo(mode);
				default:
					throw new InvalidOperationException("Platform not implemented.");
			}
		}

		protected IWindowInfo CreateX11WindowInfo(GraphicsMode mode)
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

			IWindowInfo window = Utilities.CreateX11WindowInfo(
				display, screen, wfRenderTarget.Handle, rootWindow, infoPtr);

			return window;

		}

		protected void AttachEvents()
		{
			if (wfRenderTarget == null)
				return;

			wfRenderTarget.Resize += mRenderTarget_Resize;
			wfRenderTarget.Disposed += mRenderTarget_Disposed;

			wfRenderTarget.MouseWheel += mRenderTarget_MouseWheel;
			wfRenderTarget.MouseMove += pct_MouseMove;
			wfRenderTarget.MouseDown += pct_MouseDown;
			wfRenderTarget.MouseUp += pct_MouseUp;
			wfRenderTarget.DoubleClick += mRenderTarget_DoubleClick;

			Form form = (wfRenderTarget.TopLevelControl as Form);
			form.KeyPreview = true;

			form.KeyDown += form_KeyDown;
			form.KeyUp += form_KeyUp;

			form.FormClosing += form_FormClosing;
			form.FormClosed += form_FormClosed;
		}

		protected void DetachEvents()
		{
			if (wfRenderTarget == null)
				return;

			wfRenderTarget.Resize -= mRenderTarget_Resize;
			wfRenderTarget.Disposed -= mRenderTarget_Disposed;

			wfRenderTarget.MouseWheel -= mRenderTarget_MouseWheel;
			wfRenderTarget.MouseMove -= pct_MouseMove;
			wfRenderTarget.MouseDown -= pct_MouseDown;
			wfRenderTarget.MouseUp -= pct_MouseUp;
			wfRenderTarget.DoubleClick -= mRenderTarget_DoubleClick;

			Form form = TopLevelForm as Form;

			form.KeyDown -= form_KeyDown;
			form.KeyUp -= form_KeyUp;
			form.FormClosing -= form_FormClosing;
			form.FormClosed -= form_FormClosed;
		}

		void form_FormClosed(object sender, FormClosedEventArgs e)
		{
			DetachEvents();
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
			ctxFrameBuffer.SetSize(new Size(wfRenderTarget.Width, wfRenderTarget.Height));

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

		void pct_MouseUp(object sender, MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseUp(lastMousePoint, e.AgateMousebutton()));
		}

		void pct_MouseDown(object sender, MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseDown(lastMousePoint, e.AgateMousebutton()));
		}

		void pct_MouseMove(object sender, MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseMove(lastMousePoint));
		}

		void form_KeyUp(object sender, KeyEventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.KeyUp(e.AgateKeyCode(), e.AgateKeyModifiers()));
		}

		void form_KeyDown(object sender, KeyEventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.KeyDown(e.AgateKeyCode(), e.AgateKeyModifiers()));
		}

		void renderTarget_Disposed(object sender, EventArgs e)
		{
			isClosed = true;
		}

		void IPrimaryWindow.RunApplication()
		{
			_applicationContext.RunMessageLoop();
		}

		#region --- X11 imports ---

		[StructLayout(LayoutKind.Sequential)]
		struct XVisualInfo
		{
			public IntPtr Visual;
			public IntPtr VisualID;
			public int Screen;
			public int Depth;
			public XVisualClass Class;
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
			All = 0x1FF
		}


		#endregion
		#region --- Utility functions for reading/writing non-public static fields through reflection ---

		protected static object GetStaticFieldValue(Type type, string fieldName)
		{
			return type.GetField(fieldName,
				BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		}
		protected static void SetStaticFieldValue(Type type, string fieldName, object value)
		{
			type.GetField(fieldName,
				BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, value);
		}

		#endregion

	}
}
