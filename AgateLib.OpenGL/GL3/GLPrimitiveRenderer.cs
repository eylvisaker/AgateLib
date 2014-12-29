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
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;
using OpenTK.Graphics.OpenGL;

namespace AgateLib.OpenGL.GL3
{
	/// <summary>
	/// Not OpenGL 3.1 compatible.
	/// Need replacements for everything.
	/// </summary>
	public class GLPrimitiveRenderer : PrimitiveRenderer
	{
		PositionTextureColor[] mVerts = new PositionTextureColor[12];

		int mBufferID;
		int mVaoID;

		IGL_Display mDisplay;

		public GLPrimitiveRenderer()
		{
			mDisplay = (IGL_Display)Display.Impl;

			GL.GenBuffers(1, out mBufferID);
			GL.GenVertexArrays(1, out mVaoID); 
			
			Debug.Print("GL3 PrimitiveRenderer: Draw buffer ID: {0}", mBufferID);
		}

		IGL_Surface WhiteSurface
		{
			get { return (IGL_Surface) mDisplay.WhiteSurface.Impl; }
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

		public override void DrawLine(Point a, Point b, Color color)
		{
			mVerts[0].Position.X = a.X;
			mVerts[0].Position.Y = a.Y;
			mVerts[0].TexCoord.X = 0;
			mVerts[0].TexCoord.Y = 0;
			mVerts[0].Color = color.ToArgb();

			mVerts[1].Position.X = b.X;
			mVerts[1].Position.Y = b.Y;
			mVerts[1].TexCoord.X = 1;
			mVerts[1].TexCoord.Y = 1;
			mVerts[1].Color = color.ToArgb();

			BufferData();

			Shaders.IGL3Shader shader = (Shaders.IGL3Shader)mDisplay.Shader.Impl;

			shader.SetVertexAttributes(PositionColor.VertexLayout);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, WhiteSurface.GLTextureID);
			shader.SetTexture(0);

			GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Lines, 0, 1);
		}
		public override void DrawRect(RectangleF rect, Color color)
		{
			mVerts[0].Position.X = rect.Left;
			mVerts[0].Position.Y = rect.Top;
			mVerts[0].TexCoord.X = 0;
			mVerts[0].TexCoord.Y = 0;

			mVerts[1].Position.X = rect.Right;
			mVerts[1].Position.Y = rect.Top;
			mVerts[1].TexCoord.X = 1;
			mVerts[1].TexCoord.Y = 0;

			mVerts[2] = mVerts[1];
			
			mVerts[3].Position.X = rect.Right;
			mVerts[3].Position.Y = rect.Bottom;
			mVerts[3].TexCoord.X = 1;
			mVerts[3].TexCoord.Y = 1;
			
			mVerts[4] = mVerts[3];

			mVerts[5].Position.X = rect.Left;
			mVerts[5].Position.Y = rect.Bottom;
			mVerts[5].TexCoord.X = 0;
			mVerts[5].TexCoord.Y = 1;
			
			mVerts[6] = mVerts[3];

			mVerts[7].Position.X = rect.Left;
			mVerts[7].Position.Y = rect.Top;
			mVerts[7].TexCoord.X = 0;
			mVerts[7].TexCoord.Y = 0;
			
			int colorValue = color.ToArgb();
			for (int i = 0; i < 7; i++)
			{
				mVerts[i].Color = colorValue;
			}

			BufferData();

			IGL_Display display = (IGL_Display)Display.Impl;
			Shaders.IGL3Shader shader = (Shaders.IGL3Shader)display.Shader.Impl;

			shader.SetVertexAttributes(PositionColor.VertexLayout);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, WhiteSurface.GLTextureID);
			shader.SetTexture(0);

			GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Lines, 0, 4);
		}
		public override void FillRect(RectangleF rect, Color color)
		{
			mDisplay.WhiteSurface.Color = color;
			mDisplay.WhiteSurface.Draw((Rectangle)rect);
			mDisplay.WhiteSurface.Color = Color.White;
		}
		public override void FillRect(RectangleF rect, Gradient color)
		{
			mDisplay.WhiteSurface.ColorGradient = color;
			mDisplay.WhiteSurface.Draw((Rectangle)rect);
			mDisplay.WhiteSurface.Color = Color.White;
		}

		public override void FillPolygon(PointF[] pts, int startIndex, int length, Color color)
		{
			if (mVerts.Length < pts.Length + 1)
				mVerts = new PositionTextureColor[pts.Length+1];

			for (int i = 0; i < pts.Length; i++)
			{
				mVerts[i].Position.X = pts[i].X;
				mVerts[i].Position.Y = pts[i].Y;
				mVerts[i].TexCoord.X = 0;
				mVerts[i].TexCoord.Y = 0;
				mVerts[i].Color = color.ToArgb();
			}

			mVerts[pts.Length] = mVerts[0];

			Shaders.IGL3Shader shader = (Shaders.IGL3Shader)mDisplay.Shader.Impl;

			shader.SetVertexAttributes(PositionColor.VertexLayout);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, WhiteSurface.GLTextureID);
			shader.SetTexture(0);

			GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Lines, 0, 1);
		}

	}
}
