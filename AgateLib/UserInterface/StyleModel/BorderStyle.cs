﻿using System;
using AgateLib.Geometry;
using AgateLib.UserInterface.DataModel;

namespace AgateLib.UserInterface.StyleModel
{
	public class BorderStyle
	{
		public string Image { get; set; }

		public LayoutBox ImageSlice { get; set; }

		public BorderSideStyle Left { get; set; } = new BorderSideStyle();

		public BorderSideStyle Right { get; set; } = new BorderSideStyle();

		public BorderSideStyle Top { get; set; } = new BorderSideStyle();

		public BorderSideStyle Bottom { get; set; } = new BorderSideStyle();
	}
}