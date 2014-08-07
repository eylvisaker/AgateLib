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
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL.Legacy
{
	public class FrameBufferReadPixels : GL_FrameBuffer 
	{
		Size size;
		IGL_Surface surface;
		SurfaceState s = new SurfaceState();
			
		public FrameBufferReadPixels(IGL_Surface surf)
		{
			surface = surf;
			this.size = surf.SurfaceSize;

			surface.FlipVertical = true;


			InitializeDrawBuffer();
		}

		public override void MakeCurrent()
		{
		}

		public override void Dispose()
		{
			surface.Dispose();
		}

		public override AgateLib.Geometry.Size Size
		{
			get { return size; }
		}

		public override void BeginRender()
		{
			GL.ClearColor(0, 0, 0, 0);
			GL.Clear(ClearBufferMask.ColorBufferBit |
					 ClearBufferMask.DepthBufferBit);

			GL.TexParameter(TextureTarget.Texture2D,
							 TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D,
							 TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			surface.Draw(s);

			DrawBuffer.Flush();
		}

		public override void EndRender()
		{
			DrawBuffer.ResetTexture();

			GL.BindTexture(TextureTarget.Texture2D, surface.GLTextureID);

			GL.CopyTexSubImage2D(TextureTarget.Texture2D,
				0, 0, 0, 0, 0, size.Width, size.Height);

			GL.TexParameter(TextureTarget.Texture2D,
							 TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D,
							 TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		}

		public override bool CanAccessRenderTarget
		{
			get
			{
				return true;
			}
		}

		public override SurfaceImpl RenderTarget
		{
			get
			{
				return (SurfaceImpl)surface;
			}
		}

		public override DisplayWindow AttachedWindow
		{
			get { return null; }
		}
	}
}
