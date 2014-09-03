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
using System.IO;
using System.Text;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Utility;
using AgateLib.Diagnostics;
using AgateLib.ApplicationModels;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Static class which contains all basic functions for drawing onto the Display.
	/// This class is most central to game rendering.  At the beginning and end of each frame
	/// Display.BeginFrame() and Display.EndFrame() must be called.  All drawing calls must
	/// occur between BeginFrame and EndFrame.
	/// 
	/// Display.Dispose() must be called before the program exits.
	/// 
	/// </summary>
	/// 
	/// <example> This example shows how a basic render loop works.
	/// <code>
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
	///     Core.KeepAlive();
	/// }
	/// </code>
	/// </example>
	public static class Display
	{
		private static DisplayImpl sImpl;
		private static DisplayWindow sCurrentWindow;
		private static SurfacePacker sSurfacePacker;
		private static Rectangle sCurrentClipRect;
		private static Stack<Rectangle> sClipRects = new Stack<Rectangle>();
		private static RenderStateAdapter sRenderState = new RenderStateAdapter();
		private static DisplayCapsInfo sCapsInfo = new DisplayCapsInfo();

		/// <summary>
		/// Gets the object which handles all of the actual calls to Display functions.
		/// This may be cast to a surface object in whatever rendering library
		/// is being used (eg. if using the MDX_1_1 library, this can be cast
		/// to an MDX1_Display object).  You only need to use this if you
		/// want to access features which are specific to the graphics library
		/// you're using.
		/// </summary>
		public static DisplayImpl Impl
		{
			get { return sImpl; }
		}
		/// <summary>
		/// Initializes the display with the passed object implementing DisplayImpl.
		/// </summary>
		/// <param name="displayType"></param>
		public static void Initialize(DisplayImpl impl)
		{
			sImpl = impl;
			sImpl.Initialize();

			sSurfacePacker = new SurfacePacker();

			Shaders.AgateBuiltInShaders.InitializeShaders();
		}

		/// <summary>
		/// Gets the RenderStateAdapter object which is used to set and retrieve render
		/// states for the display device.
		/// </summary>
		public static RenderStateAdapter RenderState
		{
			get { return sRenderState; }
		}

		/// <summary>
		/// Disposes of the Display.
		/// </summary>
		public static void Dispose()
		{
			OnDispose();

			if (sImpl != null)
			{
				sImpl.Dispose();
				sImpl = null;
			}

			Shaders.AgateBuiltInShaders.DisposeShaders();
		}

		internal static bool IsAppIdle
		{
			get
			{
				if (sImpl == null)
					return false;

				return sImpl.IsAppIdle;
			}
		}

		/// <summary>
		/// Gets the shader that is currently in use.
		/// </summary>
		public static Shaders.AgateShader Shader
		{
			get { return sImpl.Shader; }
			internal set { sImpl.Shader = value; }
		}

		/// <summary>
		/// Delegate type for functions which are called when Display.Dispose is called
		/// at the end of execution of the program.
		/// </summary>
		public delegate void DisposeDisplayHandler();
		/// <summary>
		/// Event that is called when Display.Dispose() is invoked, to shut down the
		/// display system and release all resources.
		/// </summary>
		public static event DisposeDisplayHandler DisposeDisplay;

		private static void OnDispose()
		{
			if (DisposeDisplay != null)
				DisposeDisplay();
		}

		/// <summary>
		/// Returns the PixelFormat of Surfaces which are created to be compatible
		/// with the display mode.  If you want to create a PixelBuffer which does
		/// not require a conversion when written to a Surface, use this format.
		/// </summary>
		public static PixelFormat DefaultSurfaceFormat
		{
			get { return sImpl.DefaultSurfaceFormat; }
		}

		/// <summary>
		/// Gets or sets the current render target.
		/// Must be called outside of BeginFrame..EndFrame blocks
		/// (usually just before BeginFrame).
		/// </summary>
		public static FrameBuffer RenderTarget
		{
			get
			{
				return sImpl.RenderTarget;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("RenderTarget", "RenderTarget cannot be null.");

				sImpl.RenderTarget = value;

				if (value.AttachedWindow != null)
					CurrentWindow = value.AttachedWindow;

			}
		}

		/// <summary>
		/// Gets the coordinate system for the current render target. 
		/// </summary>
		public static Rectangle Coordinates
		{
			get { return RenderTarget.CoordinateSystem.Coordinates; }
		}

		/// <summary>
		/// Gets the last render target used which was a DisplayWindow.
		/// </summary>
		public static DisplayWindow CurrentWindow
		{
			get { return sCurrentWindow; }
			internal set
			{
				sCurrentWindow = value;
				sImpl.RenderTarget.CoordinateSystem.RenderTargetSize = sImpl.RenderTarget.Size;
			}
		}

		/// <summary>
		/// Gets or sets the threshold value for alpha transparency below which
		/// pixels are considered completely transparent, and may not be drawn.
		/// Acceptable values are within the range of 0 to 1.
		/// </summary>
		public static double AlphaThreshold
		{
			get { return sImpl.AlphaThreshold; }
			set { sImpl.AlphaThreshold = value; }
		}

		/// <summary>
		/// Clears the buffer to black.
		/// </summary>
		public static void Clear()
		{
			Clear(Color.Black);
		}
		/// <summary>
		/// Clears the buffer to the specified color.
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
		/// Clears the buffer to the specified color.
		/// </summary>
		/// <param name="color"></param>
		public static void Clear(Color color)
		{
			sImpl.Clear(color);
		}
		/// <summary>
		/// Clears the buffer to the specified color.
		/// </summary>
		/// <param name="color">32-bit integer indicating the color.  The color will be constructed from Color.FromArgb.</param>
		public static void Clear(int color)
		{
			sImpl.Clear(Color.FromArgb(color));
		}
		/// <summary>
		/// Clears a region of the buffer to the specified color.
		/// Should be essentially the same as DrawRect(dest, color), except
		/// that alpha is not significant in the use of Clear.
		/// </summary>
		/// <param name="color">Color to clear to.</param>
		/// <param name="dest">Destination rectangle to clear.</param>
		public static void Clear(Color color, Rectangle dest)
		{
			sImpl.Clear(color, dest);
		}
		/// <summary>
		/// Clears a region of the buffer to the specified color.
		/// Should be essentially the same as DrawRect(dest, color), except
		/// that alpha is not significant in the use of Clear.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="dest"></param>
		public static void Clear(int color, Rectangle dest)
		{
			sImpl.Clear(Color.FromArgb(color), dest);
		}
		// BeginFrame and EndFrame must be called at the start and end of each frame.
		/// <summary>
		/// Must be called at the start of each frame.
		/// </summary>
		public static void BeginFrame()
		{
			if (CurrentWindow == null)
				throw new AgateException("A display window must be created before beginning to render.");
			if (RenderTarget == null)
				throw new AgateException("A render target must be set before beginning to render.");
			if (CurrentWindow.IsClosed)
				throw new ExitGameException("The current window has been closed, and a new render target has not been set.  A render target must be set to continue rendering.");

			sImpl.BeginFrame();

			RenderTarget.CoordinateSystem.RenderTargetSize = RenderTarget.Size;
			AgateBuiltInShaders.Basic2DShader.CoordinateSystem = RenderTarget.CoordinateSystem.Coordinates;
			AgateBuiltInShaders.Basic2DShader.Activate();

			sCurrentClipRect = new Rectangle(Point.Empty, RenderTarget.Size);

			RenderState.AlphaBlend = true;
		}
		/// <summary>
		/// EndFrame must be called at the end of each frame.
		/// By default, this waits for the vertical blank before rendering.
		/// However, some renderers (ie. System.Drawing) may not support that. 
		/// </summary>
		public static void EndFrame()
		{
			if (AgateConsole.IsVisible)
				AgateConsole.Draw();

			sImpl.EndFrame();
		}

		/// <summary>
		/// Gets the amount of time in milliseconds that has passed between this frame
		/// and the last one.
		/// </summary>
		public static double DeltaTime { get { return sImpl.DeltaTime; } }
		/// <summary>
		/// Provides a means to set the value returned by DeltaTime.
		/// </summary>
		/// <param name="deltaTime"></param>
		public static void SetDeltaTime(double deltaTime)
		{
			sImpl.SetDeltaTime(deltaTime);
		}

		/// <summary>
		/// Gets the framerate
		/// </summary>
		public static double FramesPerSecond { get { return sImpl.FramesPerSecond; } }

		/// <summary>
		/// Set the current clipping rect.
		/// </summary>
		/// <param name="newClipRect"></param>
		public static void SetClipRect(Rectangle newClipRect)
		{
			FlushDrawBuffer();
			sImpl.SetClipRect(newClipRect);
		}
		/// <summary>
		/// Pushes a clip rect onto the clip rect stack.
		/// </summary>
		/// <param name="newClipRect"></param>
		public static void PushClipRect(Rectangle newClipRect)
		{
			sClipRects.Push(sCurrentClipRect);
			SetClipRect(newClipRect);
		}
		/// <summary>
		/// Pops the clip rect and restores the previous clip rect.
		/// </summary>
		public static void PopClipRect()
		{
			if (sClipRects.Count == 0)
			{
				throw new AgateException("You have popped the cliprect too many times.");
			}
			else
			{
				SetClipRect(sClipRects.Pop());
			}
		}

		/// <summary>
		/// Returns the maximum size a surface object can be.
		/// </summary>
		public static Size MaxSurfaceSize
		{
			get { return Caps.MaxSurfaceSize; }
		}
		/// <summary>
		/// Gets the object which handles packing of all surfaces.
		/// </summary>
		public static SurfacePacker SurfacePacker
		{
			get { return sSurfacePacker; }
		}

		/// <summary>
		/// Takes all surfaces and packs them into a large surface.
		/// This should minimize swapping of surfaces, and may result in a performance
		/// increase when using Direct3D or OpenGL.  
		/// 
		/// If you use this, it is best to load all your surfaces into memory, 
		/// mark any you don't want packed (surfaces which may be used as render targets,
		/// for example), then call Display.PackAllSurfaces().
		/// </summary>
		public static void PackAllSurfaces()
		{
			sSurfacePacker.ClearQueue();

			if (PackAllSurfacesEvent != null)
				PackAllSurfacesEvent(null, EventArgs.Empty);

			sSurfacePacker.PackQueue();

			GC.Collect();
		}

		/// <summary>
		/// Returns an array containing information about all available full-screen modes.
		/// If full screen mode switching is not supported, the array returned has a
		/// Length of zero.
		/// </summary>
		/// <returns>An array of available full-screen modes.</returns>
		public static ScreenMode[] EnumScreenModes()
		{
			return sImpl.EnumScreenModes();
		}

		/// <summary>
		/// Event fired when PackAllSurfacesEvent
		/// </summary>
		internal static event EventHandler PackAllSurfacesEvent;

		internal static Surface BuildPackedSurface(Size size, SurfacePacker.RectPacker<Surface> packedRects)
		{
			Surface retval = sImpl.BuildPackedSurface(size, packedRects);
			retval.ShouldBePacked = false;

			return retval;

		}

		/// <summary>
		/// Gets or sets the VSync flag.  If VSync is off, tearing might occur.
		/// If VSync is on, the framerate will be capped at the monitor's refresh rate.
		/// </summary>
		[Obsolete("Use Display.RenderState.WaitForVerticalBlank instead.", true)]
		public static bool VSync
		{
			get { return RenderState.WaitForVerticalBlank; }
			set { RenderState.WaitForVerticalBlank = value; }
		}

		/// <summary>
		/// When using Direct3D or OpenGL, calls to Surface.Draw are cached to be sent to 
		/// the 3D API all as a batch.  Calling Display.FlushDrawBuffer forces all cached
		/// vertices to be sent to the rendering system.  This method should only be called
		/// between BeginFrame..EndFrame.  You should not need to call this
		/// function in normal operation of your application.  If you find that this is necessary
		/// for proper functioning of your program, there is probably a bug in AgateLib somewhere,
		/// and please report it at http://www.agatelib.org/
		/// </summary>
		public static void FlushDrawBuffer()
		{
			sImpl.FlushDrawBuffer();
		}

		/// <summary>
		/// Sets the orthogonal projection for rendering.  This allows redefinition of the
		/// coordinates used to address pixels in the window.  
		/// </summary>
		/// <remarks>
		/// [Experimental - This member is not fully tested and may have bugs.
		/// The API is also likely to change in the future.]
		/// <para>Normally, the orthogonal projection used is 
		/// set to (left, top) = (0,0), (right,bottom) = (RenderTarget.Width, RenderTarget.Height).
		/// This function provides a handy way to program an application for multiple resolutions.
		/// For example, if you set the OrthoProjection to (0,0,640,480), then all drawing calls
		/// can be made as if you were drawing to a 640x480 window, but they would be scaled to fill
		/// the entire window.</para>
		/// </remarks>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		[Obsolete("Use a AgateBuiltInShaders.Basic2DShader.CoordinateSystem instead.", true)]
		public static void SetOrthoProjection(int left, int top, int right, int bottom)
		{
			SetOrthoProjection(Rectangle.FromLTRB(left, top, right, bottom));
		}

		/// <summary>
		/// Sets the orthogonal projection for rendering.  This allows redefinition of the
		/// coordinates used to address pixels in the window.  
		/// </summary>
		/// <remarks>
		/// [Experimental - This member is not fully tested and may have bugs.
		/// The API is also likely to change in the future.]
		/// <para>Normally, the orthogonal projection used is 
		/// set to (left, top) = (0,0), (right,bottom) = (RenderTarget.Width, RenderTarget.Height).
		/// This function provides a handy way to program an application for multiple resolutions.
		/// For example, if you set the OrthoProjection to (0,0,640,480), then all drawing calls
		/// can be made as if you were drawing to a 640x480 window, but they would be scaled to fill
		/// the entire window.</para>
		/// </remarks>
		/// <param name="region"></param>
		[Obsolete("Use a AgateBuiltInShaders.Basic2DShader.CoordinateSystem instead.", true)]
		public static void SetOrthoProjection(Rectangle region)
		{
			throw new NotImplementedException();
		}

		#region --- Drawing Functions ---

		/// <summary>
		/// Draws an ellispe within the specified rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public static void DrawEllipse(Rectangle rect, Color color)
		{
			sImpl.DrawEllipse(rect, color);
		}
		/// <summary>
		/// Draws a line between the two points specified.
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <param name="color"></param>
		public static void DrawLine(int x1, int y1, int x2, int y2, Color color)
		{
			sImpl.DrawLine(new Point(x1, y1), new Point(x2, y2), color);
		}
		/// <summary>
		/// Draws a line between the two points specified.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="color"></param>
		public static void DrawLine(Point a, Point b, Color color)
		{
			sImpl.DrawLine(a, b, color);
		}
		/// <summary>
		/// Draws a bunch of connected lines.  The last point and the
		/// first point are not connected.
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="color"></param>
		public static void DrawLines(Point[] pts, Color color)
		{
			sImpl.DrawLines(pts, color);
		}
		public static void DrawLines(Color color, params Point[] pts)
		{
			sImpl.DrawLines(pts, color);
		}
		/// <summary>
		/// Draws a bunch of line segments.  Each pair of points represents
		/// a line segment which is drawn.  No connections between the line segments
		/// are made, so there must be an even number of points.
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="color"></param>
		public static void DrawLineSegments(Point[] pts, Color color)
		{
			if (pts.Length % 2 == 1)
				throw new ArgumentException("pts argument is not an even number of points!");
			sImpl.DrawLineSegments(pts, color);
		}
		/// <summary>
		/// Draws the outline of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public static void DrawRect(Rectangle rect, Color color)
		{
			sImpl.DrawRect(rect, color);
		}
		/// <summary>
		/// Draws the outline of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public static void DrawRect(RectangleF rect, Color color)
		{
			sImpl.DrawRect(rect, color);
		}
		/// <summary>
		/// Draws the outline of a rectangle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="color"></param>
		public static void DrawRect(int x, int y, int width, int height, Color color)
		{
			sImpl.DrawRect(new Rectangle(x, y, width, height), color);
		}

		/// <summary>
		/// Draws a filled ellipse inscribed in the specified rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public static void FillEllipse(Rectangle rect, Color color)
		{
			sImpl.FillEllipse((RectangleF)rect, color);
		}
		/// <summary>
		/// Draws a filled ellipse inscribed in the specified rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public static void FillEllipse(RectangleF rect, Color color)
		{
			sImpl.FillEllipse(rect, color);
		}
		/// <summary>
		/// Draws a filled polygon.  The last point will be connected to the first point.
		/// </summary>
		/// <param name="pts"></param>
		/// <param name="color"></param>
		public static void FillPolygon(PointF[] pts, Color color)
		{
			sImpl.FillPolygon(pts, color);
		}
		/// <summary>
		/// Draws a filled rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public static void FillRect(Rectangle rect, Color color)
		{
			sImpl.FillRect(rect, color);
		}
		/// <summary>
		/// Draws a filled rectangle.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="color"></param>
		public static void FillRect(int x, int y, int width, int height, Color color)
		{
			sImpl.FillRect(new Rectangle(x, y, width, height), color);
		}
		/// <summary>
		/// Draws a filled rectangle with a gradient.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public static void FillRect(Rectangle rect, Gradient color)
		{
			sImpl.FillRect(rect, color);
		}
		/// <summary>
		/// Draws a filled rectangle with a gradient.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="color"></param>
		public static void FillRect(int x, int y, int width, int height, Gradient color)
		{
			sImpl.FillRect(new Rectangle(x, y, width, height), color);
		}
		/// <summary>
		/// Draws a filled rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public static void FillRect(RectangleF rect, Color color)
		{
			sImpl.FillRect(rect, color);
		}
		/// <summary>
		/// Draws a filled rectangle with a gradient.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public static void FillRect(RectangleF rect, Gradient color)
		{
			sImpl.FillRect(rect, color);
		}


		#endregion

		/// <summary>
		/// Gets the capabilities of the Display object.
		/// </summary>
		public static DisplayCapsInfo Caps
		{
			get { return sCapsInfo; }
		}

		internal static void SavePixelBuffer(PixelBuffer pixelBuffer, string filename, ImageFileFormat format)
		{
			sImpl.SavePixelBuffer(pixelBuffer, filename, format);
		}

		internal static void HideCursor()
		{
			sImpl.HideCursor();
		}
		internal static void ShowCursor()
		{
			sImpl.ShowCursor();
		}

	}

}
