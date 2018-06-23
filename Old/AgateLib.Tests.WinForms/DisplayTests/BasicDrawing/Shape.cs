using System;
using System.Collections.Generic;
using AgateLib.DisplayLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Algorithms;
using System.Linq;
using AgateLib.Mathematics.Geometry.Builders;

namespace AgateLib.Tests.DisplayTests.BasicDrawing
{
	class Shape
	{
		public Shape()
		{
		}

		public Shape(ShapeType shapeType, Color color, Rectangle bounds)
		{
			FigureType = shapeType;
			Color = color;

			switch (shapeType)
			{
				case ShapeType.DrawRect:
				case ShapeType.FillRect:
				case ShapeType.DrawLine:
					Points = bounds.ToPolygon();
					break;

				case ShapeType.FillEllipse:
				case ShapeType.DrawEllipse:
					Points = new EllipseBuilder().BuildEllipse(bounds);
					break;

				case ShapeType.DrawPolygon:
				case ShapeType.FillPolygon:
					Random rnd = new Random();
					var pointCount = rnd.Next(3, 13);
					var size = bounds.Width / 2;
					var innerSize = size * (0.2 + rnd.NextDouble() * 0.5);
					var angle = rnd.NextDouble() * 2 * Math.PI;

					Points = new StarBuilder().BuildStar(pointCount, size, innerSize, bounds.CenterPoint, angle);
					break;
			}
		}

		public ShapeType FigureType { get; set; }

		public Color Color { get; set; }

		public Polygon Points { get; set; }

		public void Draw()
		{
			switch (FigureType)
			{
				case ShapeType.DrawLine:
					Display.Primitives.DrawLine(Color, Points[0], Points[3]);
					break;

				case ShapeType.DrawRect:
				case ShapeType.DrawEllipse:
				case ShapeType.DrawPolygon:
					Display.Primitives.DrawPolygon(Color, Points);
					break;

				case ShapeType.FillRect:
				case ShapeType.FillEllipse:
				case ShapeType.FillPolygon:
					Display.Primitives.FillPolygon(Color, Points);
					break;

				default:
					throw new NotImplementedException();
			}
		}
	}
}