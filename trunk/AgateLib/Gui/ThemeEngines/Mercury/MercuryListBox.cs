using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryListBox : MercurySchemeCommon
	{
		public Surface Image { get; set; }
		public Surface Disabled { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public Rectangle StretchRegion { get; set; }
		public int Margin { get; set; }

		public MercuryListBox()
		{
			Margin = 3;
		}
	}

}
