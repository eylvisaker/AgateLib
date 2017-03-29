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
using System.Diagnostics;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Mathematics.Geometry;
using OpenTK.Graphics.OpenGL;
using OTKPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using AgateLib.Mathematics.CoordinateSystems;

namespace AgateLib.OpenGL.Legacy
{
	public class FrameBufferExt : GL_FrameBuffer
	{
		Size mSize;
		int mFramebufferID;
		int mDepthBuffer, mStencilBuffer;
		IGL_Surface mTexture;
		bool first = true;

		static bool sDepthSupported = true;
		static bool sStencilSupported = true;


		public FrameBufferExt(IGL_Surface target)
			: base(new NativeCoordinates())
		{
			mTexture = target;
			mSize = target.SurfaceSize;

			InitializeFramebuffer();

			InitializeDrawBuffer();
		}

		void InitializeFramebuffer()
		{
			mTexture.FlipVertical = true;

			// try to initialize with both depth and stencil buffers.
			if (sDepthSupported && sStencilSupported)
			{
				try
				{
					InitializeFramebuffer(true, true);
					return;
				}
				catch (Exception e)
				{
					Trace.WriteLine("Failed to create FBO with both depth and stencil buffers.");
					Trace.WriteLine(e.Message);
				}
			}
			if (sDepthSupported)
			{
				try
				{
					InitializeFramebuffer(true, false);
					sStencilSupported = false;
					return;
				}
				catch
				{
					Trace.WriteLine("Failed to create FBO with just a depth buffer.");
				}
			}
			if (sStencilSupported)
			{
				try
				{
					InitializeFramebuffer(false, true);
					sDepthSupported = false;
					return;
				}
				catch
				{
					Trace.WriteLine("Failed to create FBO with just a stencil buffer.");
				}
			}

			try
			{
				InitializeFramebuffer(false, false);
			}
			catch
			{
				Trace.WriteLine("Failed to create FBO without either depth or stencil buffer.");
				throw;
			}
			sDepthSupported = false;
			sStencilSupported = false;
		}

		void InitializeFramebuffer(bool depth, bool stencil)
		{
			// generate the frame buffer
			GL.Ext.GenFramebuffers(1, out mFramebufferID);
			GL.Ext.BindFramebuffer(FramebufferTarget.Framebuffer, mFramebufferID);

			if (depth)
			{
				// generate a depth buffer to render to
				GL.Ext.GenRenderbuffers(1, out mDepthBuffer);
				GL.Ext.BindRenderbuffer(RenderbufferTarget.Renderbuffer, mDepthBuffer);

				GL.Ext.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
					RenderbufferStorage.DepthComponent24, mSize.Width, mSize.Height);

				// attach the depth buffer
				GL.Ext.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
					FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer,
					mDepthBuffer);
			}

			if (stencil)
			{
				// generate a stencil buffer
				GL.Ext.GenRenderbuffers(1, out mStencilBuffer);
				GL.Ext.BindRenderbuffer(RenderbufferTarget.Renderbuffer, mStencilBuffer);

				GL.Ext.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
					RenderbufferStorage.StencilIndex8Ext, mSize.Width, mSize.Height);

				// attach it.
				GL.Ext.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
					FramebufferAttachment.StencilAttachment, RenderbufferTarget.Renderbuffer,
					mStencilBuffer);
			}

			// attach the texture
			GL.Ext.FramebufferTexture2D(FramebufferTarget.Framebuffer,
				 FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D,
				 mTexture.GLTextureID, 0);

			FramebufferErrorCode code =
				GL.Ext.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

			if (code != FramebufferErrorCode.FramebufferCompleteExt)
			{
				GL.Ext.DeleteFramebuffers(1, ref mFramebufferID);
				GL.Ext.DeleteRenderbuffers(1, ref mDepthBuffer);
				GL.Ext.DeleteRenderbuffers(1, ref mStencilBuffer);

				mFramebufferID = 0;
				mDepthBuffer = 0;
				mStencilBuffer = 0;

				throw new AgateException(
					"Could not complete framebuffer object.  Received error code "
					+ code.ToString());
			}

			hasDepth = depth;
			hasStencil = stencil;
		}

		public override SurfaceImpl RenderTarget
		{
			get { return (SurfaceImpl)mTexture; }
		}
		public override void Dispose()
		{

		}

		public override Size Size => mSize;

		public override void BeginRender()
		{
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, mFramebufferID);
			GL.PushAttrib(AttribMask.ViewportBit);

			if (first)
			{
				Display.Clear(Color.FromArgb(0, 0, 0, 0));
				first = false;
			}
		}

		public override void EndRender()
		{
			GL.PopAttrib();
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

			GL.BindTexture(TextureTarget.Texture2D, mTexture.GLTextureID);
			GL.Ext.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			
			OnRenderComplete();
		}

		public override void MakeCurrent()
		{
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, mFramebufferID);
		}

		public override bool HasDepthBuffer => hasDepth;

		public override bool HasStencilBuffer => hasStencil;
	}
}
