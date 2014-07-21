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
		IGL_Surface mTexture;
		bool first = true;

		static bool sDepthSupported = true;
		static bool sStencilSupported = true;


		public FrameBufferExt(IGL_Surface target)
		{
			mTexture = target;
			mSize = target.SurfaceSize;

			InitializeFramebuffer();
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

			mHasDepth = depth;
			mHasStencil = stencil;
		}

		public override SurfaceImpl RenderTarget
		{
			get { return (SurfaceImpl)mTexture; }
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

			if (first)
			{
				AgateLib.DisplayLib.Display.Clear(Color.FromArgb(0, 0, 0, 0));
				first = false;
			}
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

		public override AgateLib.DisplayLib.DisplayWindow AttachedWindow
		{
			get { return null; }
		}
		public override bool HasDepthBuffer
		{
			get { return mHasDepth; }
		}
		public override bool HasStencilBuffer
		{
			get { return mHasStencil; }
		}
	}
}
