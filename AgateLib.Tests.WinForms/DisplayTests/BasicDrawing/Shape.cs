using System;
using System.Collections.Generic;
using AgateLib.DisplayLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms;
using System.Linq;

namespace AgateLib.Tests.DisplayTests.BasicDrawing
{
	class Shape
	{
		public Shape()
		{
		}

		public Shape(ShapeType shapeType, Color color, Rectangle rect)
		{
			FigureType = shapeType;
			Color = color;
			Rect = rect;
		}

		public ShapeType FigureType { get; set; }

		public Color Color { get; set; }

		public Rectangle Rect { get; set; }

		public void Draw()
		{
			switch (FigureType)
			{
				case ShapeType.DrawLine:
					Display.Primitives.DrawLine(Color, new Vector2(Rect.Left, Rect.Top), new Vector2(Rect.Right, Rect.Bottom));
					break;

				case ShapeType.DrawRect:
					Display.Primitives.DrawRect(Color, Rect);
					break;

				case ShapeType.DrawEllipse:
					Display.Primitives.DrawEllipse(Color, Rect);
					break;

				case ShapeType.DrawPolygon:
					Display.Primitives.DrawPolygon(Color, PolygonIn(Rect));
					break;

				case ShapeType.FillRect:
					Display.Primitives.FillRect(Color, Rect);
					break;

				case ShapeType.FillEllipse:
					Display.Primitives.FillEllipse(Color, Rect);
					break;

				case ShapeType.FillPolygon:
					Display.Primitives.FillPolygon(Color, PolygonIn(Rect));
					break;

				default:
					throw new NotImplementedException();
			}
		}

		private Polygon PolygonIn(Rectangle rect)
		{
			var size = rect.Width / 2;
			var innerSize = size *
				Math.Max(0.2, Math.Min(Math.Min(rect.Size.AspectRatio, 1.0 / rect.Size.AspectRatio), 0.7));

			var angle = rect.Height;

			var outerpoints = Pentagon(-90 + angle, size);
			var innerPoints = Pentagon(90 + angle, innerSize);

			var result = new Polygon();

			for(int i = 0; i < 5; i++)
			{
				result.Add(outerpoints[i]);
				result.Add(innerPoints.At(i + 3));
			}

			return result.Translate(
				rect.Location + new Vector2(rect.Width / 2, rect.Width / 2));
		}

		private static Polygon Pentagon(double startAngleDegrees, double size)
		{
			double ap = 72 * Math.PI / 180;
			startAngleDegrees *= Math.PI / 180;

			return new Polygon(new Vector2List
			{
				{ Math.Cos(startAngleDegrees), Math.Sin(startAngleDegrees)},
				{Math.Cos(startAngleDegrees + ap), Math.Sin(startAngleDegrees + ap) },
				{Math.Cos(startAngleDegrees + 2*ap), Math.Sin(startAngleDegrees + 2*ap) },
				{Math.Cos(startAngleDegrees + 3*ap), Math.Sin(startAngleDegrees + 3*ap) },
				{Math.Cos(startAngleDegrees + 4*ap), Math.Sin(startAngleDegrees + 4*ap) },
			}.Select(x => x * size));
		}
	}
}