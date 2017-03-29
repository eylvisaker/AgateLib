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
using System.Collections.Generic;
using System.IO;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Drivers;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.VertexTypes;
using AgateLib.Utility;

namespace AgateLib.DisplayLib.ImplementationBase
{
	/// <summary>
	/// Abstract base class for implementing the Display object.
	/// </summary>
	public abstract class DisplayImpl : IDisplayCore
	{
		private double mAlphaThreshold = 5.0 / 255.0;

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
		/// <param name="caps"></param>
		/// <returns></returns>
		public abstract Size CapsSize(DisplaySizeCaps caps);

		/// <summary>
		/// Gets a caps value which should return a floating point value.
		/// </summary>
		/// <param name="caps"></param>
		/// <returns></returns>
		public abstract double CapsDouble(DisplayDoubleCaps caps);

		#endregion

		public abstract IEnumerable<ShaderLanguage> SupportedShaderLanguages { get; }

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
			double now = AgateApp.GetTimeInMilliseconds();

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

				buffer.CopyFrom(pixels, new Rectangle(Point.Zero, surf.SurfaceSize),
					dest.Location, false);
			}

			Surface result = new Surface(buffer);

			foreach (SurfacePacker.RectHolder<Surface> rect in packedRects)
			{
				rect.Tag.SetSourceSurface(result, rect.Rect);
			}

			return result;

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


		public virtual AgateShader Shader
		{
			get { return mShader; }
			set
			{
				FlushDrawBuffer();

				mShader?.EndInternal();

				if (value is IShader2D && mShader is IShader2D)
					TransferShader2DSettings((IShader2D)value, (IShader2D)mShader);

				mShader = value;
				mShader.BeginInternal();
			}
		}

		public abstract IScreenConfiguration Screens { get; }
		public abstract IPrimitiveRenderer Primitives { get; }

		private void TransferShader2DSettings(IShader2D target, IShader2D src)
		{
			if (target.CoordinateSystem.IsEmpty)
				target.CoordinateSystem = src.CoordinateSystem;
		}


		protected internal abstract bool GetRenderState(RenderStateBool renderStateBool);
		protected internal abstract void SetRenderState(RenderStateBool renderStateBool, bool value);

		protected internal virtual VertexBufferImpl CreateVertexBuffer(
			VertexLayout layout, int vertexCount)
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

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		public abstract void Initialize();
	}

	public interface IDisplayCore : IDriverCore
	{
	}
}
