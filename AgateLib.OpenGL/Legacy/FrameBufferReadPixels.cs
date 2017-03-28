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
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Mathematics.Geometry;
using OpenTK.Graphics.OpenGL;
using AgateLib.Mathematics.CoordinateSystems;

namespace AgateLib.OpenGL.Legacy
{
	public class FrameBufferReadPixels : GL_FrameBuffer 
	{
		Size size;
		IGL_Surface surface;
		SurfaceState s = new SurfaceState();
			
		public FrameBufferReadPixels(IGL_Surface surf) : base(new NativeCoordinates())
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

		public override AgateLib.Mathematics.Geometry.Size Size => size;

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

			OnRenderComplete();
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
