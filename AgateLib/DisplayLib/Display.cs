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
using System.Linq;
using AgateLib.Configuration.State;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib.DefaultAssets;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using AgateLib.Quality;
using AgateLib.Utility;

namespace AgateLib.DisplayLib
{
	/// <summary>
	///     Static class which contains all basic functions for drawing onto the Display.
	///     This class is most central to game rendering.  At the beginning and end of each frame
	///     Display.BeginFrame() and Display.EndFrame() must be called.  All drawing calls must
	///     occur between BeginFrame and EndFrame.
	///     Display.Dispose() must be called before the program exits.
	/// </summary>
	/// <example>
	///     This example shows how a basic render loop works.
	///     <code>
	/// // These usings should be at the top.
	/// using AgateLib;
	/// using AgateLib.DisplayLib;
	/// 
	/// void MyRenderLoop()
	/// {
	///     Display.BeginFrame();
	///     Display.Clear(Color.Black);
	/// 
	///     Display.DrawRect(new Rectangle(10, 10, 30, 30), Color.Red);
	/// 
	///     Display.EndFrame();
	///     AgateApp.KeepAlive();
	/// }
	/// </code>
	/// </example>
	public static class Display
	{
		/// <summary>
		///     Initializes the display with the passed object implementing DisplayImpl.
		/// </summary>
		/// <param name="impl"></param>
		public static void Initialize(DisplayImpl impl)
		{
			Require.ArgumentNotNull(impl, nameof(impl), "Cannot initialize Display with a null object.");

			Impl = impl;
			Impl.Initialize();

			SurfacePacker = new SurfacePacker();

			AgateBuiltInShaders.InitializeShaders();

			InitializeDefaultResources();
		}

		/// <summary>
		///     Disposes of the Display.
		/// </summary>
		public static void Dispose()
		{
			if (State == null)
				return;

			State.DisposeDisplay?.Invoke();

			// Release any items which are subscribed to events, so that they are
			// eligible for garbage collection.
			State.DisposeDisplay = null;
			State.PackAllSurfacesEvent = null;

			State.DefaultResources?.Dispose();
			State.DefaultResources = null;

			AgateBuiltInShaders.DisposeShaders();

			if (Impl != null)
			{
				Impl.Dispose();
				Impl = null;
			}
		}

		internal static event EventHandler BeforeEndFrame
		{
			add { State.BeforeEndFrame += value; }
			remove { State.BeforeEndFrame -= value; }
		}

		/// <summary>
		///     Event fired when PackAllSurfacesEvent
		/// </summary>
		internal static event EventHandler PackAllSurfacesEvent
		{
			add { State.PackAllSurfacesEvent += value; }
			remove { State.PackAllSurfacesEvent -= value; }
		}

		/// <summary>
		///     Event that is called when Display.Dispose() is invoked, to shut down the
		///     display system and release all resources.
		/// </summary>
		public static event Action DisposeDisplay
		{
			add { State.DisposeDisplay += value; }
			remove { State.DisposeDisplay -= value; }
		}

		private static DisplayState State => AgateApp.State?.Display;

		/// <summary>
		///     Gets the object which handles all of the actual calls to Display functions.
		///     This may be cast to a surface object in whatever rendering library
		///     is being used (eg. if using the MDX_1_1 library, this can be cast
		///     to an MDX1_Display object).  You only need to use this if you
		///     want to access features which are specific to the graphics library
		///     you're using.
		/// </summary>
		public static DisplayImpl Impl
		{
			get { return State.Impl; }
			private set { State.Impl = value; }
		}

		/// <summary>
		/// Gets information about the display screens on the system.
		/// </summary>
		public static IScreenConfiguration Screens => Impl.Screens;

		/// <summary>
		///     Gets the RenderStateAdapter object which is used to set and retrieve render
		///     states for the display device.
		/// </summary>
		public static RenderStateAdapter RenderState => State?.RenderState;

		/// <summary>
		///     Gets the capabilities of the Display object.
		/// </summary>
		public static DisplayCapsInfo Caps => State.CapsInfo;

		/// <summary>
		///     Gets the shader that is currently in use.
		/// </summary>
		public static AgateShader Shader
		{
			get { return Impl.Shader; }
			internal set { Impl.Shader = value; }
		}

		/// <summary>
		///     Returns the PixelFormat of Surfaces which are created to be compatible
		///     with the display mode.  If you want to create a PixelBuffer which does
		///     not require a conversion when written to a Surface, use this format.
		/// </summary>
		public static PixelFormat DefaultSurfaceFormat => Impl.DefaultSurfaceFormat;

		/// <summary>
		///     Gets or sets the current render target.
		///     Must be called outside of BeginFrame..EndFrame blocks
		///     (usually just before BeginFrame).
		/// </summary>
		public static FrameBuffer RenderTarget
		{
			get { return Impl.RenderTarget; }
			set
			{
				Require.ArgumentNotNull(value, nameof(RenderTarget),
					"RenderTarget cannot be set to null.");

				Impl.RenderTarget = value;

				if (value.AttachedWindow != null)
					CurrentWindow = value.AttachedWindow;
			}
		}

		/// <summary>
		///     Gets the coordinate system for the current render target.
		/// </summary>
		public static Rectangle Coordinates => RenderTarget.CoordinateSystem.Coordinates;

		/// <summary>
		///     Gets the last render target used which was a DisplayWindow.
		/// </summary>
		public static DisplayWindow CurrentWindow
		{
			get { return AgateApp.State.Display.CurrentWindow; }
			internal set
			{
				AgateApp.State.Display.CurrentWindow = value;
				Impl.RenderTarget.CoordinateSystem.RenderTargetSize = Impl.RenderTarget.Size;
			}
		}

		/// <summary>
		///     Gets or sets the threshold value for alpha transparency below which
		///     pixels are considered completely transparent, and may not be drawn.
		///     Acceptable values are within the range of 0 to 1.
		/// </summary>
		public static double AlphaThreshold
		{
			get { return Impl.AlphaThreshold; }
			set { Impl.AlphaThreshold = value; }
		}
		
		/// <summary>
		///     Gets the object which handles packing of all surfaces.
		/// </summary>
		public static SurfacePacker SurfacePacker
		{
			get { return AgateApp.State.Display.SurfacePacker; }
			private set { AgateApp.State.Display.SurfacePacker = value; }
		}

		/// <summary>
		///     Gets the framerate
		/// </summary>
		public static double FramesPerSecond
		{
			get { return Impl.FramesPerSecond; }
		}

		/// <summary>
		/// Gets an object which handles rendering primitive shapes.
		/// </summary>
		public static IPrimitiveRenderer Primitives => Impl.Primitives;

		/// <summary>
		///     Clears the buffer to black.
		/// </summary>
		public static void Clear()
		{
			Clear(Color.Black);
		}

		/// <summary>
		///     Clears the buffer to the specified color.
		/// </summary>
		/// <param name="a">Alpha value, between 0 and 255.</param>
		/// <param name="b">Blue value, between 0 and 255.</param>
		/// <param name="g">Green value, between 0 and 255.</param>
		/// <param name="r">Red value, between 0 and 255.</param>
		public static void Clear(byte a, byte r, byte g, byte b)
		{
			Clear(Color.FromArgb(a, r, g, b));
		}

		/// <summary>
		///     Clears the buffer to the specified color.
		/// </summary>
		/// <param name="color"></param>
		public static void Clear(Color color)
		{
			Impl.Clear(color);
		}

		/// <summary>
		///     Clears the buffer to the specified color.
		/// </summary>
		/// <param name="color">32-bit integer indicating the color.  The color will be constructed from Color.FromArgb.</param>
		public static void Clear(int color)
		{
			Impl.Clear(Color.FromArgb(color));
		}

		/// <summary>
		///     Clears a region of the buffer to the specified color.
		///     Should be essentially the same as DrawRect(dest, color), except
		///     that alpha is not significant in the use of Clear.
		/// </summary>
		/// <param name="color">Color to clear to.</param>
		/// <param name="dest">Destination rectangle to clear.</param>
		public static void Clear(Color color, Rectangle dest)
		{
			Impl.Clear(color, dest);
		}

		/// <summary>
		///     Clears a region of the buffer to the specified color.
		///     Should be essentially the same as DrawRect(dest, color), except
		///     that alpha is not significant in the use of Clear.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="dest"></param>
		public static void Clear(int color, Rectangle dest)
		{
			Impl.Clear(Color.FromArgb(color), dest);
		}

		// BeginFrame and EndFrame must be called at the start and end of each frame.
		/// <summary>
		///     Must be called at the start of each frame.
		/// </summary>
		public static void BeginFrame()
		{
			if (CurrentWindow == null)
				throw new AgateException("A display window must be created before beginning to render.");
			if (RenderTarget == null)
				throw new AgateException("A render target must be set before beginning to render.");
			if (CurrentWindow.IsClosed)
				throw new AgateException(
					"The current window has been closed, and a new render target has not been set. " +
					"Did you forget to use .QuitOnClose() on your DisplayWindowBuilder?");
			if (AgateApp.IsAlive == false)
				throw new AgateException(
					"The user has closed the game window - all game loops should check AgateApp.IsAlive and terminate immediately.");

			Impl.BeginFrame();

			RenderTarget.CoordinateSystem.RenderTargetSize = RenderTarget.Size;
			AgateBuiltInShaders.Basic2DShader.CoordinateSystem = RenderTarget.CoordinateSystem.Coordinates;
			AgateBuiltInShaders.Basic2DShader.Activate();

			AgateApp.State.Display.CurrentClipRect = new Rectangle(Point.Zero, RenderTarget.Size);

			RenderState.AlphaBlend = true;
		}

		/// <summary>
		///     EndFrame must be called at the end of each frame.
		///     By default, this waits for the vertical blank before rendering.
		///     However, some renderers (ie. System.Drawing) may not support that.
		/// </summary>
		public static void EndFrame()
		{
			State.BeforeEndFrame?.Invoke(null, EventArgs.Empty);
			Impl.EndFrame();
		}

		/// <summary>
		///     Provides a means to set the value returned by DeltaTime.
		/// </summary>
		/// <param name="deltaTime"></param>
		public static void SetDeltaTime(double deltaTime)
		{
			Impl.SetDeltaTime(deltaTime);
		}

		/// <summary>
		///     Set the current clipping rect.
		/// </summary>
		/// <param name="newClipRect"></param>
		public static void SetClipRect(Rectangle newClipRect)
		{
			FlushDrawBuffer();
			Impl.SetClipRect(newClipRect);
		}

		/// <summary>
		///     Pushes a clip rect onto the clip rect stack.
		/// </summary>
		/// <param name="newClipRect"></param>
		public static void PushClipRect(Rectangle newClipRect)
		{
			AgateApp.State.Display.ClipRects.Push(AgateApp.State.Display.CurrentClipRect);
			SetClipRect(newClipRect);
		}

		/// <summary>
		///     Pops the clip rect and restores the previous clip rect.
		/// </summary>
		public static void PopClipRect()
		{
			if (AgateApp.State.Display.ClipRects.Count == 0)
				throw new AgateException("You have popped the cliprect too many times.");
			SetClipRect(AgateApp.State.Display.ClipRects.Pop());
		}

		/// <summary>
		///     Takes all surfaces and packs them into a large surface.
		///     This should minimize swapping of surfaces, and may result in a performance
		///     increase when using Direct3D or OpenGL.
		///     If you use this, it is best to load all your surfaces into memory,
		///     mark any you don't want packed (surfaces which may be used as render targets,
		///     for example), then call Display.PackAllSurfaces().
		/// </summary>
		public static void PackAllSurfaces()
		{
			SurfacePacker.ClearQueue();

			AgateApp.State.Display.PackAllSurfacesEvent?.Invoke(null, EventArgs.Empty);

			SurfacePacker.PackQueue();

			GC.Collect();
		}

		/// <summary>
		///     Returns an array containing information about all available full-screen modes.
		///     If full screen mode switching is not supported, the array returned has a
		///     Length of zero.
		/// </summary>
		/// <returns>An array of available full-screen modes.</returns>
		public static ScreenMode[] EnumScreenModes()
		{
			return Impl.EnumScreenModes();
		}

		internal static Surface BuildPackedSurface(Size size, SurfacePacker.RectPacker<Surface> packedRects)
		{
			var result = Impl.BuildPackedSurface(size, packedRects);
			result.ShouldBePacked = false;

			return result;
		}

		/// <summary>
		///     When using Direct3D or OpenGL, calls to Surface.Draw are cached to be sent to
		///     the 3D API all as a batch.  Calling Display.FlushDrawBuffer forces all cached
		///     vertices to be sent to the rendering system.  This method should only be called
		///     between BeginFrame..EndFrame.  You should not need to call this
		///     function in normal operation of your application.  If you find that this is necessary
		///     for proper functioning of your program, there is probably a bug in AgateLib somewhere,
		///     and please report it at http://www.agatelib.org/
		/// </summary>
		public static void FlushDrawBuffer()
		{
			Impl.FlushDrawBuffer();
		}

		#region --- Drawing Functions ---

		/// <summary>
		///     Draws an ellispe within the specified rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawEllipse(Rectangle rect, Color color)
		{
			var ellipsePoints = new EllipseBuilder().BuildEllipse(
				(RectangleF) rect);

			Primitives.DrawLines(LineType.Polygon, color, ellipsePoints);
		}

		/// <summary>
		///     Draws a line between the two points specified.
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawLine(int x1, int y1, int x2, int y2, Color color)
		{
			Primitives.DrawLines(LineType.LineSegments, color,
				new [] { new Vector2f(x1, y1), new Vector2f(x2, y2) });
		}

		/// <summary>
		///     Draws a line between the two points specified.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawLine(Point a, Point b, Color color)
		{
			Primitives.DrawLines(LineType.LineSegments, color,
				new[] {(Vector2f) a, (Vector2f) b});
		}

		/// <summary>
		///     Draws a series of connected lines.  The last point and the
		///     first point are not connected.
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawLines(Point[] pts, Color color)
		{
			Primitives.DrawLines(LineType.Path, color,
				pts.Cast<Vector2f>());
		}

		/// <summary>
		///     Draws a series of connected lines.  The last point and the
		///     first point are connected.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="points">The points that make up the polygon</param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawPolygon(Color color, Point[] points)
		{
			Primitives.DrawLines(LineType.Polygon, color, 
				points.Cast<Vector2f>());
		}

		/// <summary>
		/// Draws a series of connected lines. The last point and the first
		/// point are not connected.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="points"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawLines(Color color, params Point[] points)
		{
			Primitives.DrawLines(LineType.Path, color, points.Cast<Vector2f>());
		}

		/// <summary>
		///     Draws a series of line segments.  Each pair of points represents
		///     a line segment which is drawn.  No connections between the line segments
		///     are made, so there must be an even number of points.
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawLineSegments(Point[] points, Color color)
		{
			Require.True<ArgumentException>(points.Length % 2 == 0,
				"pts argument is not an even number of points!");

			Primitives.DrawLines(LineType.LineSegments, color, points.Cast<Vector2f>());
		}

		/// <summary>
		///     Draws the outline of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawRect(Rectangle rect, Color color)
		{
			Primitives.DrawLines(LineType.Polygon, color,
				new QuadrilateralBuilder().BuildRectangle(rect));
		}

		/// <summary>
		///     Draws the outline of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawRect(RectangleF rect, Color color)
		{
			Primitives.DrawLines(LineType.Polygon, color,
				new QuadrilateralBuilder().BuildRectangle(rect));
		}

		/// <summary>
		///     Draws the outline of a rectangle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void DrawRect(int x, int y, int width, int height, Color color)
		{
			DrawRect(new Rectangle(x, y, width, height), color);
		}

		/// <summary>
		///     Draws a filled ellipse inscribed in the specified rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void FillEllipse(Rectangle rect, Color color)
		{
			Primitives.FillPolygon(color, new EllipseBuilder().BuildEllipse((RectangleF)rect).ToArray());
		}

		/// <summary>
		///     Draws a filled ellipse inscribed in the specified rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void FillEllipse(RectangleF rect, Color color)
		{
			Primitives.FillPolygon(color, new EllipseBuilder().BuildEllipse(rect).ToArray());
		}

		/// <summary>
		///     Draws a filled polygon.  The last point will be connected to the first point.
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void FillPolygon(PointF[] pts, Color color)
		{
			Primitives.FillConvexPolygon(color, pts.Cast<Vector2f>());
		}

		/// <summary>
		///     Draws a filled rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void FillRect(Rectangle rect, Color color)
		{
			Primitives.FillPolygon(color, new QuadrilateralBuilder()
				.BuildRectangle(rect));
		}

		/// <summary>
		///     Draws a filled rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void FillRect(int x, int y, int width, int height, Color color)
		{
			Primitives.FillPolygon(color, new QuadrilateralBuilder()
				.BuildRectangle(new Rectangle(x, y, width, height)));
		}

		/// <summary>
		///     Draws a filled rectangle with a gradient.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		[Obsolete("If you want a Gradient, you must create a surface of it.", true)]
		public static void FillRect(Rectangle rect, Gradient color)
		{
		}

		/// <summary>
		///     Draws a filled rectangle with a gradient.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="color"></param>
		[Obsolete("If you want a Gradient, you must create a surface of it.", true)]
		public static void FillRect(int x, int y, int width, int height, Gradient color)
		{
		}

		/// <summary>
		///     Draws a filled rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		[Obsolete("Use methods on Display.Primitives instead.")]
		public static void FillRect(RectangleF rect, Color color)
		{
			Primitives.FillPolygon(color, new QuadrilateralBuilder()
				.BuildRectangle(rect));
		}

		/// <summary>
		///     Draws a filled rectangle with a gradient.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		[Obsolete("If you want a Gradient, you must create a surface of it.", true)]
		public static void FillRect(RectangleF rect, Gradient color)
		{
		}

		#endregion

		internal static void SavePixelBuffer(PixelBuffer pixelBuffer, string filename, ImageFileFormat format)
		{
			Impl.SavePixelBuffer(pixelBuffer, filename, format);
		}

		/// <summary>
		///     Hides the OS mouse cursor.
		/// </summary>
		public static void HideCursor()
		{
			Impl.HideCursor();
		}

		/// <summary>
		///     Shows the OS mouse cursor.
		/// </summary>
		public static void ShowCursor()
		{
			Impl.ShowCursor();
		}

		private static void InitializeDefaultResources()
		{
			var res = new DefaultResources();

			var task = AgateApp.State.Factory.DisplayFactory.InitializeDefaultResourcesAsync(res);
			AgateApp.State.Display.DefaultResources = res;

			task.GetAwaiter().GetResult();
		}

		public static void PushRenderTarget(FrameBuffer renderTarget)
		{
			AgateApp.State.Display.RenderTargetStack.Push(RenderTarget);

			RenderTarget = renderTarget;
		}

		public static void PopRenderTarget()
		{
			if (AgateApp.State.Display.RenderTargetStack.Count == 0)
				throw new AgateException("You have popped the render target too many times.");

			RenderTarget = AgateApp.State.Display.RenderTargetStack.Pop();
		}

	}
}