using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using OpenTK.Graphics.OpenGL;

namespace AgateOTK.GL3
{
	/// <summary>
	/// Not OpenGL 3.1 compatible.
	/// Need replacements for everything.
	/// </summary>
	class GLPrimitiveRenderer :PrimitiveRenderer 
	{
		public void SetGLColor(Color color)
		{
			GL.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

		public override void DrawLine(Point a, Point b, Color color)
		{
			SetGLColor(color);

			GL.Disable(EnableCap.Texture2D);
			GL.Begin(BeginMode.Lines);
			GL.Vertex2(a.X, a.Y);
			GL.Vertex2(b.X, b.Y);

			GL.End();
			GL.Enable(EnableCap.Texture2D);
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

		public override void FillPolygon(PointF[] pts, Color color)
		{
			GL.Disable(EnableCap.Texture2D);

			SetGLColor(color);

			GL.Begin(BeginMode.TriangleFan);
			for (int i = 0; i < pts.Length; i++)
			{
				GL.Vertex3(pts[i].X, pts[i].Y, 0);
			}
			GL.End();                                                         // Done Drawing The Quad

			GL.Enable(EnableCap.Texture2D);
		}

	}
}
