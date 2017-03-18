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
