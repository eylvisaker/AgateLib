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
using AgateLib.InputLib;
using AgateLib.OpenGL;
using AgateLib.Platform.WinForms.Controls;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.X11;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	///     No OpenGL code here.
	/// </summary>
	public abstract class GL_DisplayControl : DisplayWindowImpl
	{
		protected static WinFormsControlContext _applicationContext { get; private set; }

		protected readonly DisplayWindow owner;
		protected DesktopGLDisplay display;
		protected readonly System.Drawing.Icon icon;
		protected readonly string title;
		protected readonly bool chooseResize;
		protected readonly WindowPosition choosePosition;
		protected readonly bool hasFrame = true;

		protected System.Windows.Forms.Form wfForm;
		protected System.Windows.Forms.Control wfRenderTarget;
		protected IWindowInfo windowInfo;

		protected bool isClosed;

		protected IResolution chooseResolution;
		protected ICoordinateSystem coords = new NativeCoordinates();

		protected Point lastMousePoint;

		protected ContextFrameBuffer ctxFrameBuffer;

		private readonly SurfaceState rtSurfaceState = new SurfaceState();

		protected AgateLib.OpenGL.GL3.FrameBuffer rtFrameBuffer;
		protected GL_Surface rtSurface;

		protected GL_DisplayControl(DesktopGLDisplay display, DisplayWindow owner, CreateWindowParams windowParams)
		{
			this.owner = owner;
			choosePosition = windowParams.WindowPosition;

			this.display = display;

			if (_applicationContext == null)
				_applicationContext = new WinFormsControlContext();

			if (string.IsNullOrEmpty(windowParams.IconFile) == false)
				icon = new System.Drawing.Icon(windowParams.IconFile);

			title = windowParams.Title;
			chooseResolution = windowParams.Resolution?.Clone();
			chooseResize = windowParams.IsResizable;
			hasFrame = windowParams.HasFrame;
		}

		protected override void Dispose(bool disposing)
		{
			SafeDispose(ref windowInfo);
			SafeDispose(ref ctxFrameBuffer);

			SafeDispose(ref rtFrameBuffer);
			SafeDispose(ref rtSurface);

			if (wfForm != null)
			{
				if (wfForm.InvokeRequired)
					wfForm.Invoke(new Action(() => wfForm?.Dispose()));
				else
					wfForm.Dispose();

				wfForm = null;
			}

			ExitMessageLoop();

			base.Dispose(disposing);
		}

		protected void SafeDispose<T>(ref T disposable) where T : class, IDisposable
		{
			if (disposable == null)
				return;

			disposable.Dispose();
			disposable = null;
		}

		private System.Windows.Forms.Form TopLevelForm 
			=> (System.Windows.Forms.Form)wfRenderTarget.TopLevelControl;

		protected abstract Size ContextSize { get; }

		public override FrameBufferImpl FrameBuffer =>
			rtFrameBuffer ?? (FrameBufferImpl)ctxFrameBuffer;

		public override bool IsClosed => isClosed;

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

		public AgateLib.Geometry.Rectangle DesktopBounds => new AgateLib.Geometry.Rectangle(
			wfRenderTarget.PointToScreen(System.Drawing.Point.Empty).ToGeometry(),
			wfRenderTarget.Size.ToGeometry());

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

			wfRenderTarget.Cursor = System.Windows.Forms.Cursors.Arrow;
		}

		public void ExitMessageLoop()
		{
			_applicationContext?.ExitThread();
		}

		public void CreateContextForCurrentThread()
		{
			ctxFrameBuffer.CreateContextForThread();
		}

		private static GraphicsMode CreateGraphicsMode()
		{
			var newMode = new GraphicsMode(
				GraphicsMode.Default.ColorFormat, GraphicsMode.Default.Depth,
				0, 0, new ColorFormat(0), 2, false);

			return newMode;
		}

		protected void CreateContextFrameBuffer(ICoordinateSystem fbCoords)
		{
			using (new ResourceDisposer(ctxFrameBuffer))
			{
				ctxFrameBuffer = new ContextFrameBuffer(owner, CreateGraphicsMode(), windowInfo, ContextSize, true, false, fbCoords);
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
			var xplatui = Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms");
			if (xplatui == null)
				throw new PlatformNotSupportedException(
					"System.Windows.Forms.XplatUIX11 missing. Unsupported platform or Mono runtime version, aborting.");

			// get the required handles from the X11 API.
			var display = (IntPtr)GetStaticFieldValue(xplatui, "DisplayHandle");
			var rootWindow = (IntPtr)GetStaticFieldValue(xplatui, "RootWindow");
			var screen = (int)GetStaticFieldValue(xplatui, "ScreenNo");

			// get the X11 Visual info for the display.
			var info = new XVisualInfo();
			info.VisualID = mode.Index.Value;
			int dummy;
			info = (XVisualInfo)Marshal.PtrToStructure(
				XGetVisualInfo(display, XVisualInfoMask.ID, ref info, out dummy), typeof(XVisualInfo));

			// set the X11 colormap.
			SetStaticFieldValue(xplatui, "CustomVisual", info.Visual);
			SetStaticFieldValue(xplatui, "CustomColormap",
				XCreateColormap(display, rootWindow, info.Visual, 0));

			var infoPtr = Marshal.AllocHGlobal(Marshal.SizeOf(info));
			Marshal.StructureToPtr(info, infoPtr, false);

			var window = Utilities.CreateX11WindowInfo(
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

			var form = wfRenderTarget.TopLevelControl as System.Windows.Forms.Form;
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

			var form = TopLevelForm;

			form.KeyDown -= form_KeyDown;
			form.KeyUp -= form_KeyUp;
			form.FormClosing -= form_FormClosing;
			form.FormClosed -= form_FormClosed;
		}

		protected void CreateTargetFrameBuffer(Size size)
		{
			using (new ResourceDisposer(rtSurface, rtFrameBuffer))
			{
				rtSurface = new GL_Surface(size);

				rtFrameBuffer = new AgateLib.OpenGL.GL3.FrameBuffer(rtSurface);
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
			display.Clear(Color.Black);

			var destRect = chooseResolution.RenderMode.DestRect(
				rtSurface.SurfaceSize, ctxFrameBuffer.Size);

			rtSurfaceState.ScaleWidth = destRect.Width / (double)rtSurface.SurfaceWidth;
			rtSurfaceState.ScaleHeight = destRect.Height / (double)rtSurface.SurfaceHeight;

			rtSurfaceState.DrawInstances.Clear();
			rtSurfaceState.DrawInstances.Add(
				new SurfaceDrawInstance(destRect.Location));
			rtSurface.Draw(rtSurfaceState);

			display.DrawBuffer.Flush();
			ctxFrameBuffer.EndRender();
		}

		private void form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			isClosed = true;

			DetachEvents();
			OnClosed();
		}

		private void form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			var cancel = false;

			OnClosing(ref cancel);

			e.Cancel = cancel;
		}

		private void mRenderTarget_Disposed(object sender, EventArgs e)
		{
			isClosed = true;
		}

		private void mRenderTarget_Resize(object sender, EventArgs e)
		{
			ctxFrameBuffer.SetSize(new Size(wfRenderTarget.Width, wfRenderTarget.Height));

			OnResize();
		}

		private void mRenderTarget_DoubleClick(object sender, EventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.MouseDoubleClick(lastMousePoint, MouseButton.Primary));
		}

		private void mRenderTarget_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.MouseWheel(lastMousePoint, -(e.Delta * 100) / 120));
		}

		private void pct_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseUp(lastMousePoint, e.AgateMousebutton()));
		}

		private void pct_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseDown(lastMousePoint, e.AgateMousebutton()));
		}

		private void pct_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseMove(lastMousePoint));
		}

		private void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.KeyUp(e.AgateKeyCode(), e.AgateKeyModifiers()));
		}

		private void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.KeyDown(e.AgateKeyCode(), e.AgateKeyModifiers()));
		}

		private void renderTarget_Disposed(object sender, EventArgs e)
		{
			isClosed = true;
		}

		#region --- X11 imports ---

		[StructLayout(LayoutKind.Sequential)]
		private struct XVisualInfo
		{
			public readonly IntPtr Visual;
			public IntPtr VisualID;
			public readonly int Screen;
			public readonly int Depth;
			public readonly XVisualClass Class;
			public readonly long RedMask;
			public readonly long GreenMask;
			public readonly long blueMask;
			public readonly int ColormapSize;
			public readonly int BitsPerRgb;

			public override string ToString()
			{
				return string.Format("id ({0}), screen ({1}), depth ({2}), class ({3})",
					VisualID, Screen, Depth, Class);
			}
		}

		[DllImport("libX11")]
		public static extern IntPtr XCreateColormap(IntPtr display, IntPtr window, IntPtr visual, int alloc);

		[DllImport("libX11", EntryPoint = "XGetVisualInfo")]
		private static extern IntPtr XGetVisualInfoInternal(IntPtr display, IntPtr vinfo_mask, ref XVisualInfo template,
			out int nitems);

		private static IntPtr XGetVisualInfo(IntPtr display, XVisualInfoMask vinfo_mask, ref XVisualInfo template,
			out int nitems)
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