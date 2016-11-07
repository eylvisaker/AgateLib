using System;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Venus
{
	public class BorderStyle : IBorderStyle
	{

		public string Image { get; set; }

		public Rectangle ImageSlice { get; set; }

		public BorderSideStyle Left { get; set; }

		public BorderSideStyle Right { get; set; }

		public BorderSideStyle Top { get; set; }

		public BorderSideStyle Bottom { get; set; }

		IBorderSideStyle IBorderStyle.Left { get { return Left; } }

		IBorderSideStyle IBorderStyle.Right { get { return Right; } }

		IBorderSideStyle IBorderStyle.Top { get { return Top; } }

		IBorderSideStyle IBorderStyle.Bottom { get { return Bottom; } }
	}
}