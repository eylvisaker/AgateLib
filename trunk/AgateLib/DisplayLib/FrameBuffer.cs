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
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.DisplayLib.ImplementationBase;

namespace AgateLib.DisplayLib
{
	/// <summary>
	/// Class which represents a render target.  This can either be a render target for
	/// an area on screen with an associated DisplayWindow object, 
	/// or a render target in memory which can be used as a Surface object
	/// after rendering to it.  For the most part, FrameBuffers which are associated with
	/// a DisplayWindow cannot be used as Surfaces.
	/// </summary>
	public class FrameBuffer : IDisposable, AgateLib.DisplayLib.IFrameBuffer 
	{
		FrameBufferImpl impl;
		Surface mRenderTarget;
		Rectangle mCoordinateSystem;

		/// <summary>
		/// Constructs a frame buffer to be used as a render target.  FrameBuffers constructed
		/// with this constructor can be used as surfaces after drawing to them is complete.
		/// </summary>
		/// <param name="size"></param>
		public FrameBuffer(Size size)
		{
			impl = Display.Impl.CreateFrameBuffer(size);
			CoordinateSystem = new Rectangle(Point.Empty, size);
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
			CoordinateSystem = new Rectangle(Point.Empty, impl.Size);
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
		public FrameBufferImpl Impl
		{
			get { return impl; }
		}

		/// <summary>
		/// Size in pixels of the render target.
		/// </summary>
		public Size Size
		{
			get { return Impl.Size; }
		}
		/// <summary>
		/// Width in pixels of the render target.
		/// </summary>
		public int Width
		{
			get { return Impl.Width; }
		}
		/// <summary>
		/// Height in pixels of the render target.
		/// </summary>
		public int Height
		{
			get { return Impl.Height; }
		}
		/// <summary>
		/// Gets whether or not the frame buffer has a depth buffer.
		/// </summary>
		public bool HasDepthBuffer
		{
			get { return Impl.HasDepthBuffer; }
		}
		/// <summary>
		/// Gets whether or not the frame buffer has a stencil buffer.
		/// </summary>
		public bool HasStencilBuffer
		{
			get { return Impl.HasStencilBuffer; }
		}

		/// <summary>
		/// Gets the window that this frame buffer is attached to. Returns null
		/// if this FrameBuffer is not attached to any window.
		/// </summary>
		public DisplayWindow AttachedWindow
		{
			get { return Impl.AttachedWindow; }
		}

		/// <summary>
		/// Returns true if the RenderTarget property is readable, and this surface that is
		/// rendered to can be used to draw from.
		/// </summary>
		public bool CanAccessRenderTarget
		{
			get { return Impl.CanAccessRenderTarget; }
		}

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
		public Rectangle CoordinateSystem
		{
			get { return mCoordinateSystem; }
			set
			{
				mCoordinateSystem = value;
			}
		}
	}
}
