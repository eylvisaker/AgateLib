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
using AgateLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using OpenTK.Graphics.OpenGL;
using OTKPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

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

		public FrameBuffer(IGL_Surface surface)
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
					"Could not complete framebuffer object.  Received error code "
					+ code.ToString());
			}

			mHasDepth = true;
			mHasStencil = true;


			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			InitializeDrawBuffer();
		}
		public override void Dispose()
		{
			GL.DeleteFramebuffers(1, ref mFramebufferID);
			GL.DeleteRenderbuffers(1, ref mDepthBuffer);

			// TODO: Should we delete the surface also?
		}

		public override SurfaceImpl RenderTarget
		{
			get { return (SurfaceImpl)mTexture; }
		}
		
		public override AgateLib.Geometry.Size Size
		{
			get { return mSize; }
		}

		public override void BeginRender()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, mFramebufferID);
			//GL.PushAttrib(AttribMask.ViewportBit);
		}

		public override void EndRender()
		{
			//GL.PopAttrib();
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			GL.BindTexture(TextureTarget.Texture2D, mTexture.GLTextureID);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}

		public override void MakeCurrent()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, mFramebufferID);
		}

		public override AgateLib.DisplayLib.DisplayWindow AttachedWindow
		{
			get { return null; }
		}
		public override bool CanAccessRenderTarget
		{
			get { return true; }
		}

		public override bool HasDepthBuffer
		{
			get
			{
				return mHasDepth;
			}
		}
		public override bool HasStencilBuffer
		{
			get
			{
				return mHasStencil;
			}
		}
	}
}
