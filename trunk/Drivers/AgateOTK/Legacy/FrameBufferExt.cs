using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using OpenTK.Graphics.OpenGL;
using OTKPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace AgateOTK.Legacy
{
	class FrameBufferExt : GL_FrameBuffer 
	{
		Size mSize;
		int mFramebufferID;
		int mDepthBuffer, mStencilBuffer;
		GL_Surface mTexture;

		static bool sDepthSupported = true;
		static bool sStencilSupported = true;

		public FrameBufferExt(Size size)
		{
			mSize = size;

			InitializeFramebuffer();
		}

		void InitializeFramebuffer()
		{
			// try to initialize with both depth and stencil buffers.
			if (sDepthSupported && sStencilSupported)
			{
				try
				{
					InitializeFramebuffer(true, true);
					return;
				}
				catch
				{
					Trace.WriteLine("Failed to create FBO with both depth and stencil buffers.");
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
			mTexture = new GL_Surface(mSize);
			
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
					RenderbufferStorage.StencilIndex8, mSize.Width, mSize.Height);

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

			if (code != FramebufferErrorCode.FramebufferCompleteExt)
			{
				throw new AgateException(
					"Could not complete framebuffer object.  Received error code "
					+ code.ToString());
			}

			mHasDepth = depth;
			mHasStencil = stencil;
		}

		public override SurfaceImpl RenderTarget
		{
			get { return mTexture; }
		}
		public override void Dispose()
		{

		}

		public override AgateLib.Geometry.Size Size
		{
			get { return mSize; }
		}

		public override void BeginRender()
		{
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, mFramebufferID);
			GL.PushAttrib(AttribMask.ViewportBit);
		}

		public override void EndRender()
		{
			GL.PopAttrib();
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

			GL.BindTexture(TextureTarget.Texture2D, mTexture.GLTextureID);
			GL.Ext.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}

		public override void MakeCurrent()
		{
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, mFramebufferID);
		}
	}
}
