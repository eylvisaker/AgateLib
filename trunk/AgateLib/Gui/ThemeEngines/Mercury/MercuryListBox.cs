using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryListBox : MercuryWidget
	{
		public Surface Image { get; set; }
		public Surface Disabled { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public Rectangle StretchRegion { get; set; }

		public MercuryListBox(MercuryScheme scheme)
			:base(scheme)
		{
			Margin = 3;
		}

		public override void DrawWidget(Widget w)
		{
			DrawListBox((ListBox)w);
		}

		private void DrawListBox(ListBox listBox)
		{
			Surface image = Image;

			if (listBox.Enabled == false)
				image = Disabled;

			Point location = listBox.PointToScreen(new Point(0, 0));
			Size size = listBox.Size;

			DrawStretchImage(location, size, image, StretchRegion);
		}


	}

}
