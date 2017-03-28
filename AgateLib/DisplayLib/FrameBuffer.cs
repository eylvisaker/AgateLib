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
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Mathematics.CoordinateSystems;
using AgateLib.Mathematics.Geometry;
using AgateLib.Quality;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which represents a render target.  This can either be a render target for
	/// an area on screen with an associated DisplayWindow object, 
	/// or a render target in memory which can be used as a Surface object
	/// after rendering to it.  For the most part, FrameBuffers which are associated with
	/// a DisplayWindow cannot be used as Surfaces.
	/// </summary>
	public class FrameBuffer : IFrameBuffer 
	{
		FrameBufferImpl impl;
		Surface mRenderTarget;

		/// <summary>
		/// Constructs a frame buffer to be used as a render target.  FrameBuffers constructed
		/// with this constructor can be used as surfaces after drawing to them is complete.
		/// </summary>
		/// <param name="size"></param>
		public FrameBuffer(Size size)
		{
			Require.ArgumentInRange(size.Width > 0, "width", "Width must be positive.");
			Require.ArgumentInRange(size.Height > 0, "height", "Height must be positive.");

			impl = AgateApp.State.Factory.DisplayFactory.CreateFrameBuffer(size);
			CoordinateSystem = new NativeCoordinates();
		}

		/// <summary>
		/// Constructs a frame buffer to be used as a render target.  FrameBuffers constructed
		/// with this constructor can be used as surfaces after drawing to them is complete.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public FrameBuffer(int width, int height)
			: this(new Size(width, height))
		{ }
		/// <summary>
		/// Constructs a FrameBuffer from a FrameBufferImpl object.  Application code
		/// should not need to use this constructor ever.
		/// </summary>
		/// <param name="impl"></param>
		internal FrameBuffer(FrameBufferImpl impl)
		{
			this.impl = impl;
		}
		/// <summary>
		/// Disposes of unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			impl.Dispose();
		}

		/// <summary>
		/// Gets the implementation object of the FrameBuffer.
		/// </summary>
		public FrameBufferImpl Impl => impl;

		/// <summary>
		/// Size in pixels of the render target.
		/// </summary>
		public Size Size => Impl.Size;

		/// <summary>
		/// Width in pixels of the render target.
		/// </summary>
		public int Width => Impl.Width;

		/// <summary>
		/// Height in pixels of the render target.
		/// </summary>
		public int Height => Impl.Height;

		/// <summary>
		/// Gets whether or not the frame buffer has a depth buffer.
		/// </summary>
		public bool HasDepthBuffer => Impl.HasDepthBuffer;

		/// <summary>
		/// Gets whether or not the frame buffer has a stencil buffer.
		/// </summary>
		public bool HasStencilBuffer => Impl.HasStencilBuffer;

		/// <summary>
		/// Gets the window that this frame buffer is attached to. Returns null
		/// if this FrameBuffer is not attached to any window.
		/// </summary>
		public DisplayWindow AttachedWindow => Impl.AttachedWindow;

		/// <summary>
		/// Returns true if the RenderTarget property is readable, and this surface that is
		/// rendered to can be used to draw from.
		/// </summary>
		public bool CanAccessRenderTarget => Impl.CanAccessRenderTarget;

		/// <summary>
		/// Gets the Surface object that was rendered to, if it is available.
		/// If the CanAccessRenderTarget property is false, then this will throw 
		/// an exception.
		/// </summary>
		public Surface RenderTarget
		{
			get
			{
				if (mRenderTarget == null)
					mRenderTarget = new Surface(Impl.RenderTarget);

				return mRenderTarget;
			}
		}

		/// <summary>
		/// Gets or sets the default coordinate system that is used for
		/// this framebuffer. When Display.BeginFrame is called with this
		/// framebuffer as a render target, this coordinate system is automatically
		/// loaded.
		/// </summary>
		public ICoordinateSystem CoordinateSystem
		{
			get { return Impl.CoordinateSystem; }
			set { Impl.CoordinateSystem = value; }
		}
	}
}
