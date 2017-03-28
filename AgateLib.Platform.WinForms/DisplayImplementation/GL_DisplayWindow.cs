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
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.CoordinateSystems;
using AgateLib.InputLib;
using AgateLib.OpenGL;
using AgateLib.OpenGL.GL3;
using AgateLib.Platform.WinForms.Controls;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.X11;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	///     No OpenGL code here.
	/// </summary>
	public abstract class GL_DisplayWindow : DisplayWindowImpl
	{
		protected readonly OpenTkAdapter openTk = new OpenTkAdapter();

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

		protected Point lastMousePoint;

		protected ContextFrameBuffer ctxFrameBuffer;

		private readonly SurfaceState rtSurfaceState = new SurfaceState();

		protected GL_FrameBuffer rtFrameBuffer;

		/// <summary>
		/// Thread sync flag 
		/// </summary>
		private bool initializationComplete = false;

		protected GL_DisplayWindow(DesktopGLDisplay display, DisplayWindow owner, CreateWindowParams windowParams)
		{
			this.owner = owner;
			choosePosition = windowParams.WindowPosition;

			this.display = display;

			if (string.IsNullOrEmpty(windowParams.IconFile) == false)
				icon = new System.Drawing.Icon(windowParams.IconFile);

			title = windowParams.Title;
			chooseResolution = windowParams.Resolution?.Clone();
			chooseResize = windowParams.IsResizable;
			hasFrame = windowParams.HasFrame;

			Screen = windowParams.TargetScreen ?? display.Screens?.PrimaryScreen;
		}

		public override ScreenInfo Screen { get; }

		protected override void Dispose(bool disposing)
		{
			if (wfForm != null && ctxFrameBuffer != null && !wfForm.IsDisposed)
			{
				// This odd hack seems to be required if we have full screen
				// display windows on all monitors. Without this code, "something"
				// gets left behind and one of the monitors retains its full screen
				// context, rendering it annoyingly useless for the user.
				wfForm.Invoke(new Action(() =>
				{
					wfForm.Size = new System.Drawing.Size(100, 100);
					wfForm.TopLevel = false;
					System.Windows.Forms.Application.DoEvents();
				}));

				ctxFrameBuffer.MakeCurrent();
			}

			SafeDispose(ref rtFrameBuffer);

			SafeDispose(ref ctxFrameBuffer);

			SafeDispose(ref windowInfo);

			if (wfForm != null && !wfForm.IsDisposed)
			{
				wfForm.Invoke(new Action(wfForm.Dispose));

				wfForm = null;
			}

			base.Dispose(disposing);
		}

		private void SafeDispose<T>(ref T disposable) where T : class, IDisposable
		{
			if (disposable == null)
				return;

			disposable.Dispose();
			disposable = null;
		}

		protected System.Windows.Forms.Form TopLevelForm
			=> (System.Windows.Forms.Form)wfRenderTarget.TopLevelControl;

		/// <summary>
		/// Gets the size (dimensions) of the OpenGL context - the physical area of the screen pixels are rendered to.
		/// </summary>
		protected abstract Size ContextSize { get; }

		public override Size PhysicalSize => wfRenderTarget.Size.ToGeometry();

		public override FrameBufferImpl FrameBuffer => rtFrameBuffer ?? ctxFrameBuffer;

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

		public Rectangle DesktopBounds => new Rectangle(
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

		public void CreateContextForCurrentThread()
		{
			ctxFrameBuffer.CreateContextForThread();
		}

		public override Point PixelToLogicalCoords(Point point)
		{
			var bufferPoint = chooseResolution.RenderMode
				?.MousePoint(point, rtFrameBuffer?.Size ?? ctxFrameBuffer.Size, ctxFrameBuffer.Size) ?? point;

			return base.PixelToLogicalCoords(bufferPoint);
		}

		protected static GraphicsMode CreateGraphicsMode()
		{
			var newMode = new GraphicsMode(
				GraphicsMode.Default.ColorFormat, GraphicsMode.Default.Depth,
				0, 0, new ColorFormat(0), 2, false);

			return newMode;
		}

		private void CreateContextFrameBuffer(ICoordinateSystem fbCoords)
		{
			using (new ResourceDisposer(ctxFrameBuffer))
			{
				ctxFrameBuffer = new ContextFrameBuffer(owner, CreateGraphicsMode(), windowInfo, ContextSize, true, false, fbCoords);
				ctxFrameBuffer.MakeCurrent();
			}
		}

		private IWindowInfo CreateWindowInfo(GraphicsMode mode)
		{
			var wfRenderTarget = this.wfRenderTarget;

			return openTk.CreateWindowInfo(mode, wfRenderTarget);
		}

		protected void AttachEvents()
		{
			if (wfRenderTarget == null)
				return;

			wfRenderTarget.Resize += mRenderTarget_Resize;
			wfRenderTarget.Disposed += mRenderTarget_Disposed;

			wfRenderTarget.MouseWheel += mRenderTarget_MouseWheel;
			wfRenderTarget.MouseMove += mRenderTarget_MouseMove;
			wfRenderTarget.MouseDown += mRenderTarget_MouseDown;
			wfRenderTarget.MouseUp += mRenderTarget_MouseUp;
			wfRenderTarget.DoubleClick += mRenderTarget_DoubleClick;

			var form = TopLevelForm;

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
			wfRenderTarget.MouseMove -= mRenderTarget_MouseMove;
			wfRenderTarget.MouseDown -= mRenderTarget_MouseDown;
			wfRenderTarget.MouseUp -= mRenderTarget_MouseUp;
			wfRenderTarget.DoubleClick -= mRenderTarget_DoubleClick;

			var form = TopLevelForm;

			form.FormClosing -= form_FormClosing;
			form.FormClosed -= form_FormClosed;
		}

		protected void InitializeContexts()
		{
			windowInfo = CreateWindowInfo(CreateGraphicsMode());

			if (windowInfo.Handle == IntPtr.Zero)
				throw new InvalidOperationException("WindowInfo Handle should not be null. Has form initialization completed?");

			AttachEvents();

			// The context framebuffer must be created before the target framebuffer,
			// or this may cause the dreaded black screen on some configurations
			// (Intel hardware seems to be notorious here).
			CreateContextFrameBuffer(new NativeCoordinates());
			CreateTargetFrameBuffer(chooseResolution.Size);
			CreateContextForCurrentThread();
		}

		protected void ShowOwnedForm()
		{
			if (display.EventThread.InvokeRequired)
			{
				display.EventThread.Invoke(ShowOwnedForm);
				return;
			}

			if (icon != null)
				wfForm.Icon = icon;

			wfForm.Show();
		}

		protected void CreateTargetFrameBuffer(Size size)
		{
			using (new ResourceDisposer(rtFrameBuffer))
			{
				rtFrameBuffer = new OpenGL.GL3.FrameBuffer(
					(IGL_Surface)new Surface(size).Impl) { ParentContext = ctxFrameBuffer };
				rtFrameBuffer.RenderComplete += RtFrameBuffer_RenderComplete;
				rtFrameBuffer.MyAttachedWindow = owner;
			}
		}

		private void RtFrameBuffer_RenderComplete(object sender, EventArgs e)
		{
			BlitBufferImage();
		}

		private void BlitBufferImage()
		{
			// The window context needs to be set current before we do 
			// any of the other things in this method.
			ctxFrameBuffer.BeginRender();

			ctxFrameBuffer.CoordinateSystem.RenderTargetSize = ctxFrameBuffer.Size;
			AgateBuiltInShaders.Basic2DShader.CoordinateSystem = ctxFrameBuffer.CoordinateSystem.Coordinates;
			AgateBuiltInShaders.Basic2DShader.Activate();

			display.Clear(Color.Black);

			var destRect = chooseResolution.RenderMode.DestRect(
				rtFrameBuffer.Size, ctxFrameBuffer.Size);

			rtSurfaceState.ScaleWidth = destRect.Width / (double) rtFrameBuffer.Width;
			rtSurfaceState.ScaleHeight = destRect.Height / (double) rtFrameBuffer.Height;

			rtSurfaceState.DrawInstances.Clear();
			rtSurfaceState.DrawInstances.Add(
				new SurfaceDrawInstance(destRect.Location));
			rtFrameBuffer.RenderTarget.Draw(rtSurfaceState);

			display.DrawBuffer.Flush();
			ctxFrameBuffer.EndRender();
		}

		private async void form_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
		{
			isClosed = true;

			DetachEvents();

			if (wfForm != null)
			{
				AgateApp.QueueWorkItem(() =>
				{
					OnClosed(owner);
				});

				await Task.Delay(250);
			}
		}

		private void form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			if (wfForm == null)
				return;

			e.Cancel = true;

			AgateApp.QueueWorkItem(() =>
			{
				var cancel = false;

				OnClosing(owner, ref cancel);

				if (!cancel)
				{
					OnClosed(owner);

					display.EventThread.Invoke(() => wfForm?.Dispose());
				}
			});
		}

		private void mRenderTarget_Disposed(object sender, EventArgs e)
		{
			isClosed = true;
		}

		private void mRenderTarget_Resize(object sender, EventArgs e)
		{
			AgateApp.QueueWorkItem(() =>
			{
				ctxFrameBuffer.SetSize(new Size(wfRenderTarget.Width, wfRenderTarget.Height));

				OnResize(owner);
			});
		}

		private void mRenderTarget_DoubleClick(object sender, EventArgs e)
		{
			if (!initializationComplete)
				return;

			OnInputEvent(owner, AgateInputEventArgs.MouseDoubleClick(lastMousePoint, MouseButton.Primary));
		}

		private void mRenderTarget_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!initializationComplete)
				return;

			OnInputEvent(owner, AgateInputEventArgs.MouseWheel(lastMousePoint, -(e.Delta * 100) / 120));
		}

		private void mRenderTarget_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!initializationComplete)
				return;

			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(owner, AgateInputEventArgs.MouseUp(lastMousePoint, e.AgateMouseButton()));
		}

		private void mRenderTarget_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!initializationComplete)
				return;

			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(owner, AgateInputEventArgs.MouseDown(lastMousePoint, e.AgateMouseButton()));
		}

		private void mRenderTarget_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (!initializationComplete)
				return;

			lastMousePoint = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(owner, AgateInputEventArgs.MouseMove(lastMousePoint));
		}

		private void renderTarget_Disposed(object sender, EventArgs e)
		{
			isClosed = true;
		}

		protected void InitializationComplete()
		{
			initializationComplete = true;
		}
	}
}