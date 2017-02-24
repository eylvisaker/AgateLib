using System;
using AgateLib.DisplayLib;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

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

				case ShapeType.FillRect:
					Display.Primitives.FillRect(Color, Rect);
					break;

				case ShapeType.FillEllipse:
					Display.Primitives.FillEllipse(Color, Rect);
					break;

				default:
					throw new NotImplementedException();
			}
		}
	}
}