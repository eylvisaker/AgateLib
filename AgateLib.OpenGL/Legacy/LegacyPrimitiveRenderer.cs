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
using AgateLib.DisplayLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using OpenTK.Graphics.OpenGL;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace AgateLib.OpenGL.Legacy
{
	public class LegacyPrimitiveRenderer : PrimitiveRenderer, IPrimitiveRenderer
	{
		private IDrawBufferState drawBuffer;

		public LegacyPrimitiveRenderer(IDrawBufferState drawBuffer)
		{
			this.drawBuffer = drawBuffer;
		}

		public void DrawLines(LineType lineType, Color color, IEnumerable<Vector2f> points)
		{
			drawBuffer.FlushDrawBuffer();
			SetGLColor(color);

			GL.Disable(EnableCap.Texture2D);
			GL.Begin(PrimitiveTypeOf(lineType));

			foreach (var point in points)
			{
				GL.Vertex2(point.X, point.Y);
			}

			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}

		public void FillConvexPolygon(Color color, IEnumerable<Vector2f> points)
		{
			drawBuffer.FlushDrawBuffer();

			SetGLColor(color);

			GL.Disable(EnableCap.Texture2D);
			GL.Begin(PrimitiveType.TriangleFan);

			Vector2f? first = null;

			foreach (var point in points)
			{
				first = first ?? point;
				GL.Vertex2(point.X, point.Y);
			}

			if (first.HasValue)
			{
				GL.Vertex2(first.Value.X, first.Value.Y);
			}

			GL.End();
			GL.Enable(EnableCap.Texture2D);
		}

		public void SetGLColor(Color color)
		{
			GL.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}
	}
}
