using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class ImageDisplay : Widget
	{
		public Surface Surface { get; set; }

		public ImageDisplay()
		{
			Width = 96;
			Height = 96;
		}

		public override void DrawImpl()
		{
			if (Surface == null)
				return;

			Surface.Draw(ClientToScreen(new Point(X, Y)));
		}

		internal override Size ComputeSize(int? minWidth, int? minHeight, int? maxWidth, int? maxHeight)
		{
			if (Surface == null)
				return new Size(96, 96);

			return Surface.DisplaySize;
		}
	}
}
