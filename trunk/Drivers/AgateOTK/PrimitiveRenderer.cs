using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateOTK
{
	abstract class PrimitiveRenderer
	{
		public abstract void DrawLine(Point a, Point b, Color color);

		public abstract void DrawRect(RectangleF rect, Color color);
		public abstract void FillRect(RectangleF rect, Color color);
		public abstract void FillRect(RectangleF rect, Gradient color);

		public abstract void FillPolygon(PointF[] pts, Color color);

	}
}
