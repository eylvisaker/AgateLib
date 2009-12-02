using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.ImplementationBase
{
	/// <summary>
	/// Base class for implementing a render target.
	/// </summary>
	public abstract class FrameBufferImpl : IDisposable 
	{
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
	}
}
