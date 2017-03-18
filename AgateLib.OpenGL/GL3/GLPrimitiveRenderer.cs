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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
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

		public void FillPolygon(Color color, IEnumerable<Vector2f> points)
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
