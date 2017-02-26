using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using AgateLib.InputLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.CoordinateSystems;
using AgateLib.Mathematics.Geometry;
using AgateLib.OpenGL;
using AgateLib.Quality;
using OpenTK.Platform;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	public sealed class GL_DisplayWindowFullDesktop : DisplayWindowImpl
	{
		class ScreenData
		{
			public ScreenInfo Screen { get; set; }

			public ContextFrameBuffer Context { get; set; }

			public frmFullScreen Form { get; set; }

			public IWindowInfo WindowInfo { get; set; }

			public Rectangle SourceRect { get; set; }
		}

		private OpenTkAdapter openTk = new OpenTkAdapter();

		List<ScreenData> screenFrameBuffers
			= new List<ScreenData>();

		private IResolution resolution;
		private GL_FrameBuffer backBuffer;
		private readonly SurfaceState rtSurfaceState = new SurfaceState();
		private DesktopGLDisplay display;
		private DisplayWindow owner;
		private bool isClosed;
		private Mathematics.Geometry.Point lastMousePoint;

		public GL_DisplayWindowFullDesktop(DesktopGLDisplay display, DisplayWindow owner, CreateWindowParams windowParams)
		{
			const string invalidMessage =
				"GL_DisplayWindowFullDesktop is the wrong type for this CreateWindowParams object. Use GL_DisplayWindowWindowed instead.";

			Require.False<ArgumentException>(windowParams.RenderToControl, invalidMessage);
			Require.True<ArgumentException>(windowParams.IsFullScreen, invalidMessage);

			this.display = display;
			this.owner = owner;

			var graphicsMode = openTk.CreateGraphicsMode();

			System.Drawing.Icon icon = null;

			if (string.IsNullOrEmpty(windowParams.IconFile) == false)
				icon = new System.Drawing.Icon(windowParams.IconFile);

			foreach (var screen in display.Screens.AllScreens)
			{
				var data = new ScreenData
				{
					Screen = screen,
					Form = display.EventThread.Invoke(() => new frmFullScreen
					{
						ClientSize = screen.Bounds.Size.ToDrawing(),
						Location = screen.Bounds.Location.ToDrawing(),
						Text = windowParams.Title,
						Icon = icon,
						TopLevel = true
					})
				};

				display.EventThread.Invoke(() => data.Form.Show());

				data.WindowInfo = openTk.CreateWindowInfo(graphicsMode, data.Form);

				data.Context = new ContextFrameBuffer(
					owner, graphicsMode, data.WindowInfo, screen.Bounds.Size, true, false, new NativeCoordinates());

				screenFrameBuffers.Add(data);
			}

			Resolution = windowParams.Resolution;

			foreach (var screenData in screenFrameBuffers)
			{
				AttachEvents(screenData);
			}
		}

		protected override void Dispose(bool disposing)
		{
			foreach (var screenData in screenFrameBuffers)
			{
				// This odd hack seems to be required if we have full screen
				// display windows on all monitors. Without this code, "something"
				// gets left behind and one of the monitors retains its full screen
				// context, rendering it annoyingly useless for the user.
				screenData.Form.Invoke(new Action(() =>
				{
					screenData.Form.Size = new System.Drawing.Size(100, 100);
					screenData.Form.TopLevel = false;
					System.Windows.Forms.Application.DoEvents();
				}));
			}


			foreach (var screenData in screenFrameBuffers)
			{
				screenData.Context.MakeCurrent();
				screenData.Context.Dispose();

				screenData.Form.Invoke(new Action(() => screenData.Form.Dispose()));
			}

			base.Dispose(disposing);
		}

		public override bool IsFullScreen => true;

		public override FrameBufferImpl FrameBuffer => backBuffer;

		public override IResolution Resolution
		{
			get { return resolution; }
			set
			{
				resolution = value.Clone(
					new Size((int)(display.Screens.DesktopBounds.Size.AspectRatio * value.Height),
					         value.Height));

				RecreateTargetFrameBuffer();
			}
		}

		public override bool IsClosed => isClosed;

		public override string Title { get; set; }

		public override Size PhysicalSize => display.Screens.DesktopBounds.Size;

		public override ScreenInfo Screen => display.Screens.PrimaryScreen;

		private void RecreateTargetFrameBuffer()
		{
			Vector2 sizeRatio = new Vector2(
				resolution.Width / (double)display.Screens.DesktopBounds.Width,
				resolution.Height / (double)display.Screens.DesktopBounds.Height);
			
			var context = screenFrameBuffers.First().Context;
			context.MakeCurrent();

			backBuffer = new OpenGL.GL3.FrameBuffer((IGL_Surface)new Surface(Resolution.Size).Impl)
			{
				ParentContext = context
			};

			backBuffer.RenderComplete += BackBuffer_RenderComplete;
			backBuffer.MyAttachedWindow = owner;

			foreach (var data in screenFrameBuffers)
			{
				var screen = data.Screen;

				data.SourceRect = new Rectangle(
					(int)(screen.Bounds.X * sizeRatio.X),
					(int)(screen.Bounds.Y * sizeRatio.Y),
					(int)(screen.Bounds.Width * sizeRatio.X),
					(int)(screen.Bounds.Height * sizeRatio.Y));
			}
		}

		private void BackBuffer_RenderComplete(object sender, EventArgs e)
		{
			BlitBufferImage();
		}

		private void BlitBufferImage()
		{
			foreach (var screenData in screenFrameBuffers)
			{
				var screen = screenData.Screen;

				var screenContext = screenData.Context;

				// The window context needs to be set current before we do 
				// any of the other things in this method.
				screenContext.BeginRender();

				screenContext.CoordinateSystem.RenderTargetSize = screenContext.Size;
				AgateBuiltInShaders.Basic2DShader.CoordinateSystem = screenContext.CoordinateSystem.Coordinates;
				AgateBuiltInShaders.Basic2DShader.Activate();

				display.Clear(Color.Black);

				var destRect = Resolution.RenderMode.DestRect(
					screenData.SourceRect.Size, screenContext.Size);
				var sourceRect = screenData.SourceRect;

				rtSurfaceState.ScaleWidth = destRect.Width / (double)sourceRect.Width;
				rtSurfaceState.ScaleHeight = destRect.Height / (double)sourceRect.Height;

				rtSurfaceState.DrawInstances.Clear();
				rtSurfaceState.DrawInstances.Add(
					new SurfaceDrawInstance((PointF)destRect.Location, sourceRect));
				backBuffer.RenderTarget.Draw(rtSurfaceState);

				display.DrawBuffer.Flush();
				screenContext.EndRender();
			}
		}
		private void AttachEvents(ScreenData screenData)
		{
			var form = screenData.Form;

			form.FormClosing += Form_FormClosing;
			form.MouseMove += (sender, e) => Form_MouseMove(screenData, e);

		}

		private void Form_MouseMove(ScreenData screenData, MouseEventArgs e)
		{
			lastMousePoint = PixelToLogicalCoords(screenData, new Mathematics.Geometry.Point(e.X, e.Y));

			OnInputEvent(owner, AgateInputEventArgs.MouseMove(lastMousePoint));
		}

		private Mathematics.Geometry.Point PixelToLogicalCoords(ScreenData screenData, Mathematics.Geometry.Point point)
		{
			var desktopPoint = (Vector2)screenData.Screen.Bounds.Location + (Vector2) point;

			Vector2 desktopRelativePoint = new Vector2(
				desktopPoint.X / display.Screens.DesktopBounds.Width,
				desktopPoint.Y / display.Screens.DesktopBounds.Height);

			Vector2 backBufferPoint = new Vector2(
				desktopRelativePoint.X * backBuffer.Width,
				desktopRelativePoint.Y * backBuffer.Height);

			return (Point)backBufferPoint;
		}

		private void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;

			AgateApp.QueueWorkItem(() =>
			{
				var cancel = false;

				OnClosing(owner, ref cancel);

				if (!cancel)
				{
					OnClosed(owner);

					Dispose();
				}
			});
		}
	}
}