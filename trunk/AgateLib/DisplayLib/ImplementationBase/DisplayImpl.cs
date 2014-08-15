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
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Utility;

namespace AgateLib.DisplayLib.ImplementationBase
{
	/// <summary>
	/// Abstract base class for implementing the Display object.
	/// </summary>
	public abstract class DisplayImpl : DriverImplBase
	{
		private double mAlphaThreshold = 5.0 / 255.0;
		Point[] mEllipsePoints;
		PointF[] mEllipsePointfs;

		private FrameBuffer mRenderTarget;

		#region --- Capabilities Reporting ---

		/// <summary>
		/// Gets a caps value which should return a logical value.
		/// When implementing this, you should return false for any value 
		/// which you do not explicitly handle.
		/// </summary>
		/// <param name="caps"></param>
		/// <returns></returns>
		public abstract bool CapsBool(DisplayBoolCaps caps);
		/// <summary>
		/// Gets a caps value which should return a Size object.
		/// </summary>
		/// <param name="displaySizeCaps"></param>
		/// <returns></returns>
		public abstract Size CapsSize(DisplaySizeCaps caps);

		/// <summary>
		/// Gets a caps value which should return a floating point value.
		/// </summary>
		/// <param name="caps"></param>
		/// <returns></returns>
		public abstract double CapsDouble(DisplayDoubleCaps caps);

		#endregion

		public abstract IEnumerable<DisplayLib.Shaders.ShaderLanguage> SupportedShaderLanguages { get; }

		private static AgateShader mShader;
		
		/// <summary>
		/// Gets or sets the current render target.
		/// </summary>
		public FrameBuffer RenderTarget
		{
			get
			{
				return mRenderTarget;
			}
			set
			{
				if (value == mRenderTarget)
					return;

				if (mInFrame)
					throw new AgateException("Cannot change render target between BeginFrame and EndFrame");

				FrameBuffer old = mRenderTarget;
				mRenderTarget = value;

				OnRenderTargetChange(old);
			}
		}

		/// <summary>
		/// The pixelformat that created surfaces should use.
		/// </summary>
		public abstract PixelFormat DefaultSurfaceFormat { get; }

		/// <summary>
		/// Event raised when the current render target is changed.
		/// </summary>
		/// <param name="oldRenderTarget"></param>
		protected abstract void OnRenderTargetChange(FrameBuffer oldRenderTarget);
		/// <summary>
		/// Event raised when the render target is resized.
		/// </summary>
		protected abstract void OnRenderTargetResize();

		///// <summary>
		///// Creates a DisplayWindowImpl derived object.
		///// </summary>
		///// <param name="title"></param>
		///// <param name="clientWidth"></param>
		///// <param name="clientHeight"></param>
		///// <param name="allowResize"></param>
		///// <param name="iconFile"></param>
		///// <param name="startFullscreen"></param>
		///// <returns></returns>
		//public abstract DisplayWindowImpl CreateDisplayWindow(string title, int clientWidth, int clientHeight, string iconFile, bool startFullscreen, bool allowResize);
		///// <summary>
		///// Creates a DisplayWindowImpl derived object.
		///// </summary>
		//public abstract DisplayWindowImpl CreateDisplayWindow(System.Windows.Forms.Control renderTarget);

		/// <summary>
		/// Creates a DisplayWindowImpl derived object.
		/// </summary>
		/// <param name="windowParams"></param>
		/// <returns></returns>
		public abstract DisplayWindowImpl CreateDisplayWindow(DisplayWindow owner, CreateWindowParams windowParams);

		/// <summary>
		/// Creates a SurfaceImpl derived object.
		/// </summary>
		/// <param name="provider"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		public virtual SurfaceImpl CreateSurface(IReadFileProvider provider, string filename)
		{
			return CreateSurface(provider.OpenRead(filename));
		}
		/// <summary>
		/// Creates a SurfaceImpl derived object.
		/// </summary>
		public abstract SurfaceImpl CreateSurface(string fileName);
		/// <summary>
		/// Creates a SurfaceImpl derived object from a stream containing 
		/// the file contents.
		/// </summary>
		/// <param name="fileStream"></param>
		/// <returns></returns>
		public abstract SurfaceImpl CreateSurface(Stream fileStream);

		/// <summary>
		/// Creates a SurfaceImpl derived object.
		/// </summary>
		public abstract SurfaceImpl CreateSurface(Size surfaceSize);

		/// <summary>
		/// Creates a SurfaceImpl derived object.
		/// Forwards the call to CreateSurface(Size).
		/// </summary>
		public SurfaceImpl CreateSurface(int width, int height)
		{
			return CreateSurface(new Size(width, height));
		}
		/// <summary>
		/// Creates a FontSurfaceImpl derived object.
		/// </summary>
		/// <param name="fontFamily"></param>
		/// <param name="sizeInPoints"></param>
		/// <param name="style"></param>
		/// <returns></returns>
		public abstract FontSurfaceImpl CreateFont(string fontFamily,
			float sizeInPoints, FontStyles style);

		/// <summary>
		/// Creates a BitmapFontImpl object from the specified options.
		/// </summary>
		/// <param name="bitmapOptions"></param>
		/// <returns></returns>
		public abstract FontSurfaceImpl CreateFont(BitmapFontOptions bitmapOptions);

		/// <summary>
		/// Gets or sets the threshold value for alpha transparency below which
		/// pixels are considered completely transparent, and may not be drawn.
		/// </summary>
		public double AlphaThreshold
		{
			get { return mAlphaThreshold; }
			set
			{
				if (value < 0)
					mAlphaThreshold = 0;
				else if (value > 1)
					mAlphaThreshold = 1;
				else
					mAlphaThreshold = value;
			}
		}

		#region --- BeginFrame / EndFrame and DeltaTime stuff ---

		private bool mInFrame = false;
		private double mDeltaTime;
		private double mLastTime;
		private bool mRanOnce = false;

		private double mFPSStart;
		private int mFrames = 0;
		private double mFPS = 0;

		// BeginFrame and EndFrame must be called at the start and end of each frame.
		/// <summary>
		/// Must be called at the start of each frame.
		/// </summary>
		public void BeginFrame()
		{
			if (mInFrame)
				throw new AgateException(
					"Called BeginFrame() while already inside a BeginFrame..EndFrame block!\n" +
					"Did you forget to call EndFrame()?");
			if (mRenderTarget == null)
				throw new AgateException("BeginFrame was called but the render target has not been set.");

			mInFrame = true;

			OnBeginFrame();
		}

		/// <summary>
		/// A version of EndFrame must be called at the end of each frame.
		/// This version allows the caller to indicate to the implementation whether or
		/// not it is preferred to wait for the vertical blank to do the drawing.
		/// </summary>
		public void EndFrame()
		{
			CheckInFrame("EndFrame");

			OnEndFrame();

			mFrames++;
			mInFrame = false;

			CalcDeltaTime();
		}

		private void CalcDeltaTime()
		{
			double now = Core.GetTime();

			if (mRanOnce)
			{
				mDeltaTime = now - mLastTime;
				mLastTime = now;

				if (now - mFPSStart > 200)
				{
					double time = (now - mFPSStart) * 0.001;

					// average current framerate with that of the last update
					mFPS = (mFrames / time) * 0.8 + mFPS * 0.2;

					mFPSStart = now;
					mFrames = 0;

				}

				// hack to make sure delta time is never zero.
				if (mDeltaTime == 0.0)
				{
					mDeltaTime += 0.0000001;
				}
			}
			else
			{
				mDeltaTime = 0;
				mLastTime = now;

				mFPSStart = now;
				mFrames = 0;

				mRanOnce = true;
			}
		}

		/// <summary>
		/// Called by BeginFrame to let the driver know to do its setup stuff for starting
		/// the next render pass.
		/// </summary>
		protected abstract void OnBeginFrame();
		/// <summary>
		/// Called by EndFrame to let the driver know that it's time to swap buffers or whatever
		/// is required to finish rendering the frame.
		/// </summary>
		protected abstract void OnEndFrame();

		/// <summary>
		/// Checks to see whether or not we are currently inside a
		/// BeginFrame..EndFrame block, and throws an exception if
		/// we are not.  This is only meant to be called
		/// from functions which must operate between these calls.  
		/// </summary>
		/// <param name="functionName">The name of the calling function, 
		/// for debugging purposes.</param>
		public void CheckInFrame(string functionName)
		{
			if (mInFrame)
				return;

			throw new AgateException(
				functionName + " called outside of BeginFrame..EndFrame block!" +
				"Did you forget to call BeginFrame() before doing drawing?");
		}

		/// <summary>
		/// Gets the amount of time in milliseconds that has passed between this frame
		/// and the last one.
		/// </summary>
		public double DeltaTime
		{
			get
			{
				return mDeltaTime;
			}
		}
		/// <summary>
		/// Provides a means to set the value returned by DeltaTime.
		/// </summary>
		/// <param name="deltaTime"></param>
		public void SetDeltaTime(double deltaTime)
		{
			mDeltaTime = deltaTime;
		}
		/// <summary>
		/// Gets the framerate
		/// </summary>
		public double FramesPerSecond
		{
			get { return mFPS; }
		}

		#endregion

		/// <summary>
		/// Obsolete.  Use Display.Caps.MaxSurfaceSize instead.
		/// </summary>
		[Obsolete("Use Display.Caps instead.", true)]
		public virtual Size MaxSurfaceSize { get { return Display.Caps.MaxSurfaceSize; } }

		#region --- SetClipRect ---

		/// <summary>
		/// Set the current clipping rect.
		/// </summary>
		/// <param name="newClipRect"></param>
		public abstract void SetClipRect(Rectangle newClipRect);

		#endregion
		#region --- Direct modification of the back buffer ---

		/// <summary>
		/// Clears the buffer to black.
		/// </summary>
		public virtual void Clear()
		{
			Clear(Color.Black);
		}
		/// <summary>
		/// Clears the buffer to the specified color.
		/// </summary>
		/// <param name="color"></param>
		public abstract void Clear(Color color);
		/// <summary>
		/// Clears a region of the buffer to the specified color.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="destRect"></param>
		public abstract void Clear(Color color, Rectangle destRect);

		
		/// <summary>
		/// Draws an ellipse by making a bunch of connected lines.
		/// 
		/// Info for developers:
		/// The base class implements this by calculating points on the circumference of
		/// the ellipse, then making a call to DrawLines.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public virtual void DrawEllipse(Rectangle rect, Color color)
		{
			Point center = new Point(rect.Left + rect.Width / 2,
									 rect.Top + rect.Height / 2);

			double radiusX = rect.Width / 2;
			double radiusY = rect.Height / 2;
			double h = Math.Pow(radiusX - radiusY, 2) / Math.Pow(radiusX + radiusY, 2);

			//Ramanujan's second approximation to the circumference of an ellipse.
			double circumference =
				Math.PI * (radiusX + radiusY) * (1 + 3 * h / (10 + Math.Sqrt(4 - 3 * h)));

			// we will take the circumference as being the number of points to draw
			// on the ellipse.
			int ptCount = (int)Math.Ceiling(circumference * 2);
			if (mEllipsePoints == null || mEllipsePoints.Length < ptCount)
				mEllipsePoints = new Point[ptCount];

			double step = 2 * Math.PI / (ptCount - 1);

			for (int i = 0; i < ptCount; i++)
			{
				mEllipsePoints[i] = new Point((int)(center.X + radiusX * Math.Cos(step * i) + 0.5),
								   (int)(center.Y + radiusY * Math.Sin(step * i) + 0.5));
			}

			DrawLines(mEllipsePoints, 0, ptCount, color);
		}

		/// <summary>
		/// Draws a solid ellipse.  
		/// </summary>
		/// <param name="rect">The rectangle in which the ellipse should be inscribed.</param>
		/// <param name="color">The color of the ellipse.</param>
		public virtual void FillEllipse(RectangleF rect, Color color)
		{
			PointF center = new PointF(rect.Left + rect.Width / 2,
									   rect.Top + rect.Height / 2);

			double radiusX = rect.Width / 2;
			double radiusY = rect.Height / 2;
			double h = Math.Pow(radiusX - radiusY, 2) / Math.Pow(radiusX + radiusY, 2);

			//Ramanujan's second approximation to the circumference of an ellipse.
			double circumference =
				Math.PI * (radiusX + radiusY) * (1 + 3 * h / (10 + Math.Sqrt(4 - 3 * h)));

			// we will take the circumference as being the number of points to draw
			// on the ellipse.
			int ptCount = (int)Math.Ceiling(circumference * 2);
			if (mEllipsePointfs == null || mEllipsePointfs.Length < ptCount)
				mEllipsePointfs = new PointF[ptCount];

			double step = 2 * Math.PI / (ptCount - 1);

			for (int i = 0; i < ptCount; i++)
			{
				mEllipsePointfs[i] = new PointF((float)(center.X + radiusX * Math.Cos(step * i) + 0.5),
									(float)(center.Y + radiusY * Math.Sin(step * i) + 0.5));
			}

			FillPolygon(mEllipsePointfs, 0, ptCount, color);
		}

		/// <summary>
		/// Draws a filled polygon of the specified color.
		/// </summary>
		/// <param name="pts">The array of points which make up the boundary of the polygon.</param>
		/// <param name="color">The color to fill the polygon.</param>
		public void FillPolygon(PointF[] pts, Color color)
		{
			FillPolygon(pts, 0, pts.Length, color);
		}

		/// <summary>
		/// Draws a filled polygon of the specified color.
		/// </summary>
		/// <param name="pts">An array containing the points which make up the boundary of the polygon.</param>
		/// <param name="startIndex">The index of the first point in the boundary.</param>
		/// <param name="length">The number of points to use.</param>
		/// <param name="color">The color to fill the polygon.</param>
		public abstract void FillPolygon(PointF[] pts, int startIndex, int length, Color color);

		/// <summary>
		/// Draws a line between the two specified endpoints.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="color"></param>
		public abstract void DrawLine(Point a, Point b, Color color);
		/// <summary>
		/// Draws a bunch of connected points.
		/// 
		/// Info for developers:
		/// The base class implements this by making several calls to DrawLine.
		/// You may want to override this one to minimize state changes.
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="color"></param>
		public virtual void DrawLines(Point[] pt, Color color)
		{
			for (int i = 0; i < pt.Length - 1; i++)
				DrawLine(pt[i], pt[i + 1], color);
		}
		/// <summary>
		/// Draws a bunch of connected points.
		/// 
		/// Info for developers:
		/// The base class implements this by making several calls to DrawLine.
		/// You may want to override this one to minimize state changes.
		/// </summary>
		/// <param name="pt">An array containing the points to draw.</param>
		/// <param name="startIndex">Index of the first point in the array.</param>
		/// <param name="length">Number of points to use in the array.</param>
		/// <param name="color"></param>
		public virtual void DrawLines(Point[] pt, int startIndex, int length, Color color)
		{
			for (int i = startIndex; i < startIndex + length - 1; i++)
				DrawLine(pt[i], pt[i + 1], color);
		}
		/// <summary>
		/// Draws a bunch of unconnected lines.
		/// <para>
		/// Info for developers:
		/// pt should be an array whose length is even.</para>
		/// </summary>
		/// <param name="pt"></param>
		/// <param name="color"></param>
		public virtual void DrawLineSegments(Point[] pt, Color color)
		{
			for (int i = 0; i < pt.Length; i += 2)
				DrawLine(pt[i], pt[i + 1], color);
		}

		/// <summary>
		/// Draws the outline of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public abstract void DrawRect(Rectangle rect, Color color);
		/// <summary>
		/// Draws the outline of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public abstract void DrawRect(RectangleF rect, Color color);
		/// <summary>
		/// Draws a filled rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public abstract void FillRect(Rectangle rect, Color color);
		/// <summary>
		/// Draws a filled rectangle with a gradient.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public abstract void FillRect(Rectangle rect, Gradient color);
		/// <summary>
		/// Draws a filled rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public abstract void FillRect(RectangleF rect, Color color);
		/// <summary>
		/// Draws a filled rectangle with a gradient.
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="color"></param>
		public abstract void FillRect(RectangleF rect, Gradient color);

		#endregion

		/// <summary>
		/// Builds a surface of the specified size, using the information
		/// generated by the SurfacePacker.
		/// </summary>
		/// <param name="size"></param>
		/// <param name="packedRects"></param>
		/// <returns></returns>
		public virtual Surface BuildPackedSurface(Size size, SurfacePacker.RectPacker<Surface> packedRects)
		{
			PixelBuffer buffer = new PixelBuffer(Display.DefaultSurfaceFormat, size);

			foreach (SurfacePacker.RectHolder<Surface> rect in packedRects)
			{
				Surface surf = rect.Tag;
				Rectangle dest = rect.Rect;

				PixelBuffer pixels = surf.ReadPixels();

				buffer.CopyFrom(pixels, new Rectangle(Point.Empty, surf.SurfaceSize),
					dest.Location, false);
			}

			Surface retval = new Surface(buffer);

			foreach (SurfacePacker.RectHolder<Surface> rect in packedRects)
			{
				rect.Tag.SetSourceSurface(retval, rect.Rect);
			}

			return retval;

		}

		/// <summary>
		/// Enumerates a list of screen modes.
		/// </summary>
		/// <returns></returns>
		public virtual ScreenMode[] EnumScreenModes()
		{
			return new ScreenMode[] { };
		}

		/// <summary>
		/// Flushes the 2D draw buffer, if applicable.
		/// </summary>
		public abstract void FlushDrawBuffer();

		/// <summary>
		/// Sets the boundary coordinates of the window.
		/// </summary>
		/// <param name="region"></param>
		[Obsolete]
		public virtual void SetOrthoProjection(Rectangle region)
		{ }

		/// <summary>
		/// Returns true if the application is idle and processing of events can be skipped.
		/// Base method just returns false to force processing of events at every frame.
		/// </summary>
		protected internal virtual bool IsAppIdle
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pixelBuffer"></param>
		/// <param name="filename"></param>
		/// <param name="format"></param>
		protected internal virtual void SavePixelBuffer(PixelBuffer pixelBuffer, string filename, ImageFileFormat format)
		{
			throw new AgateException("Display driver does not support saving pixel buffers.");
		}

		/// <summary>
		/// Makes the OS mouse pointer visible.
		/// </summary>
		protected internal abstract void ShowCursor();
		/// <summary>
		/// Hides the OS mouse pointer.
		/// </summary>
		protected internal abstract void HideCursor();


		protected internal virtual VertexBufferImpl CreateVertexBuffer(
			Geometry.VertexTypes.VertexLayout layout, int vertexCount)
		{
			throw new AgateException("Cannot create a vertex buffer with a driver that does not support 3D.");
		}

		protected internal virtual IndexBufferImpl CreateIndexBuffer(IndexBufferType type, int size)
		{
			throw new AgateException("Cannot create an index buffer with a driver that does not support 3D.");
		}


		/// <summary>
		/// Creates one of the build in shaders in AgateLib.  Implementers should 
		/// return null for any built in shader that is not supported.
		/// Basic2DShader must have an implementation, but any other shader can be unsupported.
		/// </summary>
		/// <param name="builtInShaderType"></param>
		/// <returns></returns>
		protected internal abstract AgateShaderImpl CreateBuiltInShader(AgateLib.DisplayLib.Shaders.Implementation.BuiltInShader builtInShaderType);

		/// <summary>
		/// Creates a 
		/// </summary>
		/// <param name="size"></param>
		protected internal abstract FrameBufferImpl CreateFrameBuffer(Size size);

		/// <summary>
		/// Override this method if shaders are supported.
		/// Only call the base class method if shaders aren't supported, as it throws a NotSupportedException.
		/// </summary>
		/// <returns></returns>
		[Obsolete]
		protected internal virtual ShaderCompilerImpl CreateShaderCompiler()
		{
			throw new NotSupportedException("The current driver does not support shaders.");
		}

		[Obsolete]
		public virtual Effect Effect
		{
			get { return null; }
			set { throw new NotSupportedException("The current driver does not support shaders."); }
		}
		public virtual AgateShader Shader
		{
			get { return mShader; }
			set
			{
				FlushDrawBuffer();

				if (mShader != null)
					mShader.EndInternal();

				if (value is IShader2D && mShader is IShader2D)
					TransferShader2DSettings((IShader2D)value, (IShader2D)mShader);

				mShader = value;
				mShader.BeginInternal();
			}
		}

		private void TransferShader2DSettings(IShader2D target, IShader2D src)
		{
			if (target.CoordinateSystem.IsEmpty)
				target.CoordinateSystem = src.CoordinateSystem;
		}


		protected internal abstract bool GetRenderState(RenderStateBool renderStateBool);
		protected internal abstract void SetRenderState(RenderStateBool renderStateBool, bool value);

	}
}
