using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryCheckBox : MercurySchemeCommon
	{
		public Surface Image { get; set; }
		public Surface Disabled { get; set; }
		public Surface Check { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public int Spacing { get; set; }
		public int Margin { get; set; }

		public MercuryCheckBox()
		{
			Spacing = 5;
			Margin = 2;
		}
	}
}
