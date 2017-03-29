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
using AgateLib.Mathematics.Geometry;

namespace AgateLib.DisplayLib.ImplementationBase
{
	/// <summary>
	/// Base class for implementing a render target.
	/// </summary>
	public abstract class FrameBufferImpl : IDisposable 
	{
		/// <summary>
		/// Constructs a FrameBufferImpl object.
		/// </summary>
		/// <param name="coords"></param>
		public FrameBufferImpl(ICoordinateSystem coords)
		{
			this.CoordinateSystem = coords;
		}

		/// <summary>
		/// Disposes of the unmanaged resources.
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Size in pixels of the render target.
		/// </summary>
		public abstract Size Size { get; }

		/// <summary>
		/// Width in pixels of the render target.
		/// </summary>
		public int Width { get { return Size.Width; } }
		/// <summary>
		/// Height in pixels of the render target.
		/// </summary>
		public int Height { get { return Size.Height; } }

		/// <summary>
		/// Begins rendering to the render target.
		/// </summary>
		public abstract void BeginRender();
		/// <summary>
		/// Ends rendering to the render target.
		/// </summary>
		public abstract void EndRender();

		/// <summary>
		/// Gets whether or not the frame buffer has a depth buffer.
		/// </summary>
		public abstract bool HasDepthBuffer { get; }
		/// <summary>
		/// Gets whether or not the frame buffer has a stencil buffer.
		/// </summary>
		public abstract bool HasStencilBuffer { get; }

		/// <summary>
		/// Return true to indicate that the back buffer can be read and used as a texture.
		/// </summary>
		public virtual bool CanAccessRenderTarget
		{
			get { return false; }
		}
		/// <summary>
		/// Gets the SurfaceImpl which points to the render target, to be used as a texture.
		/// Throw an AgateException if CanAccessBackBuffer is false.
		/// </summary>
		public virtual SurfaceImpl RenderTarget
		{
			get { throw new AgateException("Cannot access the back buffer in this frame buffer."); }
		}

		/// <summary>
		/// Gets the window that this frame buffer is attached to. Returns null
		/// if this FrameBuffer is not attached to any window.
		/// </summary>
		public abstract DisplayWindow AttachedWindow { get; }

		/// <summary>
		/// Gets or sets the coordinate system that is used to map 2d coordinates to pixels in the render target.
		/// </summary>
		public virtual ICoordinateSystem CoordinateSystem { get; set; }
	}
}
