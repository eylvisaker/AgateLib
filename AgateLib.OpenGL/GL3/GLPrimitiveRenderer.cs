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
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.VertexTypes;
using OpenTK.Graphics.OpenGL;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace AgateLib.OpenGL.GL3
{
	/// <summary>
	/// Not OpenGL 3.1 compatible.
	/// Need replacements for everything.
	/// </summary>
	public class GLPrimitiveRenderer : PrimitiveRenderer, IPrimitiveRenderer
	{
		PositionTextureColor[] mVerts = new PositionTextureColor[12];

		int mBufferID;
		int mVaoID;

		IGL_Display mDisplay;
		private IDrawBufferState drawBuffer;

		public GLPrimitiveRenderer()
		{
			mDisplay = (IGL_Display)Display.Impl;

			GL.GenBuffers(1, out mBufferID);
			GL.GenVertexArrays(1, out mVaoID);

			Debug.Print("GL3 PrimitiveRenderer: Draw buffer ID: {0}", mBufferID);
		}

		public GLPrimitiveRenderer(IDrawBufferState drawBuffer)
		{
			this.drawBuffer = drawBuffer;
		}

		IGL_Surface WhiteSurface
		{
			get { return (IGL_Surface)mDisplay.WhiteSurface.Impl; }
		}

		private void BufferData()
		{
			GL.BindVertexArray(mVaoID);

			int bufferSize = mVerts.Length * Marshal.SizeOf(typeof(PositionColor));

			GL.BindBuffer(BufferTarget.ArrayBuffer, mBufferID);

			unsafe
			{
				GCHandle handle = new GCHandle();

				try
				{
					handle = GCHandle.Alloc(mVerts, GCHandleType.Pinned);

					IntPtr ptr = handle.AddrOfPinnedObject();

					GL.BufferData(BufferTarget.ArrayBuffer,
						(IntPtr)bufferSize, ptr,
						BufferUsageHint.StaticDraw);
				}
				finally
				{
					handle.Free();
				}
			}
		}


		public void DrawLines(LineType lineType, Color color, IEnumerable<Vector2f> points)
		{
			drawBuffer.FlushDrawBuffer();

			var _points = points.ToArray();

			if (mVerts.Length < _points.Length)
				mVerts = new PositionTextureColor[_points.Length];

			for (int i = 0; i < _points.Length; i++)
			{
				mVerts[i].Position.X = _points[i].X;
				mVerts[i].Position.Y = _points[i].Y;
				mVerts[i].Color = color.ToArgb();
			}

			BufferData();

			Shaders.IGL3Shader shader = (Shaders.IGL3Shader)mDisplay.Shader.Impl;

			shader.SetVertexAttributes(PositionColor.VertexLayout);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, WhiteSurface.GLTextureID);
			shader.SetTexture(0);

			GL.DrawArrays(PrimitiveTypeOf(lineType), 0, 1);
		}

		public void FillConvexPolygon(Color color, IEnumerable<Vector2f> points)
		{
			drawBuffer.FlushDrawBuffer();

			var pointArray = points.ToArray();

			if (mVerts.Length < pointArray.Length + 1)
				mVerts = new PositionTextureColor[pointArray.Length + 1];

			for (int i = 0; i < pointArray.Length; i++)
			{
				mVerts[i].Position.X = pointArray[i].X;
				mVerts[i].Position.Y = pointArray[i].Y;
				mVerts[i].TexCoord.X = 0;
				mVerts[i].TexCoord.Y = 0;
				mVerts[i].Color = color.ToArgb();
			}

			mVerts[pointArray.Length] = mVerts[0];

			Shaders.IGL3Shader shader = (Shaders.IGL3Shader)mDisplay.Shader.Impl;

			shader.SetVertexAttributes(PositionColor.VertexLayout);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, WhiteSurface.GLTextureID);
			shader.SetTexture(0);

			GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.TriangleStrip, 0, 1);
		}

	}
}
