using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using OpenTK.Graphics.OpenGL;
using OTKPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace AgateOTK.Legacy
{
	class FrameBufferExt : GL_FrameBuffer 
	{
		Size mSize;
		int mFramebufferID;
		int mDepthBuffer;
		GL_Surface mTexture;

		public FrameBufferExt(Size size)
		{
			mSize = size;

			//AgateLib.DisplayLib.PixelBuffer pixels = new AgateLib.DisplayLib.PixelBuffer(
			//     AgateLib.DisplayLib.PixelFormat.RGBA8888, mSize);

			mTexture = new GL_Surface(mSize);
			
			// generate the frame buffer
			GL.Ext.GenFramebuffers(1, out mFramebufferID);
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, mFramebufferID);

			// generate a depth buffer to render to
			GL.Ext.GenRenderbuffers(1, out mDepthBuffer);
			GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, mDepthBuffer);

			// hack here because RenderbufferStorage enum is incomplete.
			GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt,
				RenderbufferStorage.Depth24Stencil8,
				mSize.Width, mSize.Height);

			// attach the depth buffer
			GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt,
				FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt,
				mDepthBuffer);

			// attach the texture
			GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
				 FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D,
				 mTexture.GLTextureID, 0);

			FramebufferErrorCode code =
				GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);

			if (code != FramebufferErrorCode.FramebufferCompleteExt)
			{
				throw new AgateException(
					"Could not complete framebuffer object.");
			}

		}

		public override AgateLib.ImplementationBase.SurfaceImpl BackBuffer
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
