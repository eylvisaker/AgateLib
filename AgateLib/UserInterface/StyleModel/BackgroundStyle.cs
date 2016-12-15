using System;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.StyleModel
{
	public class BackgroundStyle
	{
		public BackgroundClip Clip { get; set; }

		public Color Color { get; set; }

		public string Image { get; set; }

		public Point Position { get; set; }

		public BackgroundRepeat Repeat { get; set; }
	}
}