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
	public class FrameBuffer : IDisposable 
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
			impl = Display.Impl.CreateFrameBuffer(size);
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
				if (mRenderTarget != null && mRenderTarget.Impl != Impl.RenderTarget)
				{
					mRenderTarget.Dispose();
					mRenderTarget = null;
				}

				if (mRenderTarget == null)
					mRenderTarget = new Surface(Impl.RenderTarget);

				return mRenderTarget;
			}
		}
	}
}
