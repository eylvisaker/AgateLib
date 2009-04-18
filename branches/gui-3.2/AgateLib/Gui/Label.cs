using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
	public class Label : Widget
	{
		public Label() { Name = "Label"; }
		public Label(string text)
		{
			Name = text;
			Text = text;

			TextAlignment = AgateLib.DisplayLib.OriginAlignment.CenterLeft;
		}

		public DisplayLib.OriginAlignment TextAlignment { get; set; }
	}
}
