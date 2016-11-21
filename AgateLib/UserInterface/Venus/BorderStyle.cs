using System;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Venus
{
	public class BorderStyle : IBorderStyle
	{
		public string Image { get; set; }

		public LayoutBox ImageSlice { get; set; }

		public BorderSideStyle Left { get; set; } = new BorderSideStyle();

		public BorderSideStyle Right { get; set; } = new BorderSideStyle();

		public BorderSideStyle Top { get; set; } = new BorderSideStyle();

		public BorderSideStyle Bottom { get; set; } = new BorderSideStyle();

		IBorderSideStyle IBorderStyle.Left { get { return Left; } }

		IBorderSideStyle IBorderStyle.Right { get { return Right; } }

		IBorderSideStyle IBorderStyle.Top { get { return Top; } }

		IBorderSideStyle IBorderStyle.Bottom { get { return Bottom; } }
	}
}