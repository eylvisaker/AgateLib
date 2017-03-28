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
using AgateLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Mathematics.Geometry;
using OpenTK.Graphics.OpenGL;
using OTKPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using AgateLib.Mathematics.CoordinateSystems;

namespace AgateLib.OpenGL.GL3
{
	/// <summary>
	/// OpenGL 3.1 compatible.
	/// </summary>
	public class FrameBuffer : GL_FrameBuffer 
	{
		Size mSize;
		int mFramebufferID;
		int mDepthBuffer;
		IGL_Surface mTexture;

		public FrameBuffer(IGL_Surface surface) : base(new NativeCoordinates())
		{
			mTexture = surface;
			mSize = surface.SurfaceSize;

			mTexture.FlipVertical = true;

			// generate the frame buffer
			GL.GenFramebuffers(1, out mFramebufferID);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, mFramebufferID);

			// attach the texture
			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
				 FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D,
				 mTexture.GLTextureID, 0);

			// generate a depth buffer to render to
			GL.GenRenderbuffers(1, out mDepthBuffer);
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, mDepthBuffer);

			// hack here because RenderbufferStorage enum is incomplete.
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
				RenderbufferStorage.Depth24Stencil8,
				mSize.Width, mSize.Height);

			// attach the depth buffer
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
				FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer,
				mDepthBuffer);

			FramebufferErrorCode code =
				GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

			if (code != FramebufferErrorCode.FramebufferComplete)
			{
				throw new AgateException(
					$"Could not complete framebuffer object.  Received error code {code}");
			}

			hasDepth = true;
			hasStencil = true;

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			InitializeDrawBuffer();
		}

		public override void Dispose()
		{
			GL.DeleteFramebuffers(1, ref mFramebufferID);
			GL.DeleteRenderbuffers(1, ref mDepthBuffer);

			// TODO: Should we delete the surface also?
		}

		public override bool CanAccessRenderTarget => true;

		public override bool HasDepthBuffer => hasDepth;

		public override bool HasStencilBuffer => hasStencil;

		public override SurfaceImpl RenderTarget => (SurfaceImpl)mTexture;

		public override Size Size => mSize;

		/// <summary>
		/// ParentContext property is set when this is used as a backing render target
		/// for a control context. This is needed because on Intel drivers, FrameBuffers
		/// are not shared between contexts and the glBindFrameBuffer call requires that
		/// context which was active when the FBO was created be the current context.
		/// Without that it fails. Note that this does not happen with AMD hardware.
		/// </summary>
		public ContextFrameBuffer ParentContext { get; set; }

		public override void BeginRender()
		{
			ParentContext?.MakeCurrent();

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, mFramebufferID);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Viewport(0, 0, Width, Height);
		}

		public override void EndRender()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			GL.BindTexture(TextureTarget.Texture2D, mTexture.GLTextureID);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			OnRenderComplete();
		}

		public override void MakeCurrent()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, mFramebufferID);
		}
	}
}
