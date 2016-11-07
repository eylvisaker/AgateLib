using System;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Venus
{
	public class BorderSideStyle : IBorderSideStyle
	{
		public Color Color { get; set; }

		public int Width { get; set; }
	}
}