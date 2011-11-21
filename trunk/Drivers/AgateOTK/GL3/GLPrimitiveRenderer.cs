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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
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

namespace AgateOTK.GL3
{
	/// <summary>
	/// Not OpenGL 3.1 compatible.
	/// Need replacements for everything.
	/// </summary>
	class GLPrimitiveRenderer : PrimitiveRenderer
	{
		PositionColor[] mVerts = new PositionColor[6];

		int mBufferID;
		int mVaoID;

		public GLPrimitiveRenderer()
		{
			GL.GenBuffers(1, out mBufferID);
			GL.GenVertexArrays(1, out mVaoID); 
			
			Debug.Print("GL3 PrimitiveRenderer: Draw buffer ID: {0}", mBufferID);
		}

		public void SetGLColor(Color color)
		{
			GL.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

		public override void DrawLine(Point a, Point b, Color color)
		{
			mVerts[0].Position.X = a.X;
			mVerts[0].Position.Y = a.Y;
			mVerts[0].Color = color.ToArgb();

			mVerts[1].Position.X = b.X;
			mVerts[1].Position.Y = b.Y;
			mVerts[1].Color = color.ToArgb();

			BufferData();

			GL_Display display = (GL_Display)Display.Impl;
			Shaders.IGL3Shader shader = (Shaders.IGL3Shader)display.Shader.Impl;

			shader.SetVertexAttributes(PositionColor.VertexLayout);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, 0);
			shader.SetTexture(0);

			GL.DrawArrays(BeginMode.Lines, 0, 1);

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

		public override void DrawRect(RectangleF rect, Color color)
		{
			SetGLColor(color);

			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Lines);

			GL.Vertex2(rect.Left, rect.Top);
			GL.Vertex2(rect.Right, rect.Top);

			GL.Vertex2(rect.Right, rect.Top);
			GL.Vertex2(rect.Right, rect.Bottom);

			GL.Vertex2(rect.Left, rect.Bottom);
			GL.Vertex2(rect.Right, rect.Bottom);

			GL.Vertex2(rect.Left, rect.Top);
			GL.Vertex2(rect.Left, rect.Bottom);

			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}
		public override void FillRect(RectangleF rect, Color color)
		{
			SetGLColor(color);

			GL.Disable(EnableCap.Texture2D);

			GL.Begin(BeginMode.Quads);
			GL.Vertex3(rect.Left, rect.Top, 0);                                        // Top Left
			GL.Vertex3(rect.Right, rect.Top, 0);                                         // Top Right
			GL.Vertex3(rect.Right, rect.Bottom, 0);                                        // Bottom Right
			GL.Vertex3(rect.Left, rect.Bottom, 0);                                       // Bottom Left
			GL.End();                                                         // Done Drawing The Quad

			GL.Enable(EnableCap.Texture2D);
		}
		public override void FillRect(RectangleF rect, Gradient color)
		{
			GL.Disable(EnableCap.Texture2D);

			GL.Begin(BeginMode.Quads);
			SetGLColor(color.TopLeft);
			GL.Vertex3(rect.Left, rect.Top, 0);                                        // Top Left

			SetGLColor(color.TopRight);
			GL.Vertex3(rect.Right, rect.Top, 0);                                         // Top Right

			SetGLColor(color.BottomRight);
			GL.Vertex3(rect.Right, rect.Bottom, 0);                                        // Bottom Right

			SetGLColor(color.BottomLeft);
			GL.Vertex3(rect.Left, rect.Bottom, 0);                                       // Bottom Left
			GL.End();                                                         // Done Drawing The Quad

			GL.Enable(EnableCap.Texture2D);
		}

		public override void FillPolygon(PointF[] pts, int startIndex, int length, Color color)
		{
			GL.Disable(EnableCap.Texture2D);

			SetGLColor(color);

			GL.Begin(BeginMode.TriangleFan);
			for (int i = 0; i < length; i++)
			{
				GL.Vertex3(pts[startIndex + i].X, pts[startIndex + i].Y, 0);
			}
			GL.End();                                                         // Done Drawing The Quad

			GL.Enable(EnableCap.Texture2D);
		}

	}
}
