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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.ApplicationModels;

namespace AgateLib.DisplayLib.ImplementationBase
{
	/// <summary>
	/// Base class for implementing a render target.
	/// </summary>
	public abstract class FrameBufferImpl : IDisposable 
	{
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
