using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using OpenTK.Graphics.OpenGL;
using OTKPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace AgateOTK.GL3
{
	/// <summary>
	/// OpenGL 3.1 compatible.
	/// </summary>
	class FrameBuffer : GL_FrameBuffer 
	{
		Size mSize;
		int mFramebufferID;
		int mDepthBuffer;
		GL_Surface mTexture;

		public FrameBuffer(Size size)
		{
			mSize = size;

			//AgateLib.DisplayLib.PixelBuffer pixels = new AgateLib.DisplayLib.PixelBuffer(
			//     AgateLib.DisplayLib.PixelFormat.RGBA8888, mSize);

			mTexture = new GL_Surface(mSize);
			
			// generate the frame buffer
			GL.GenFramebuffers(1, out mFramebufferID);
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt, mFramebufferID);

			// generate a depth buffer to render to
			GL.GenRenderbuffers(1, out mDepthBuffer);
			GL.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, mDepthBuffer);

			// hack here because RenderbufferStorage enum is incomplete.
			GL.RenderbufferStorage(RenderbufferTarget.RenderbufferExt,
				RenderbufferStorage.Depth24Stencil8,
				mSize.Width, mSize.Height);

			// attach the depth buffer
			GL.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt,
				FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt,
				mDepthBuffer);

			// attach the texture
			GL.FramebufferTexture2D(FramebufferTarget.FramebufferExt,
				 FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D,
				 mTexture.GLTextureID, 0);

			FramebufferErrorCode code =
				GL.CheckFramebufferStatus(FramebufferTarget.FramebufferExt);

			if (code != FramebufferErrorCode.FramebufferCompleteExt)
			{
				throw new AgateException(
					"Could not complete framebuffer object.");
			}

		}

		public override SurfaceImpl RenderTarget
		{
			get { return mTexture; }
		}
		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		public override AgateLib.Geometry.Size Size
		{
			get { return mSize; }
		}

		public override void BeginRender()
		{
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt, mFramebufferID);
			GL.PushAttrib(AttribMask.ViewportBit);
		}

		public override void EndRender()
		{
			GL.PopAttrib();
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

			GL.BindTexture(TextureTarget.Texture2D, mTexture.GLTextureID);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}

		public override void MakeCurrent()
		{
			GL.BindFramebuffer(FramebufferTarget.FramebufferExt, mFramebufferID);
		}
	}
}
