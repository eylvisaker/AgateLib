using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryButton : MercurySchemeCommon
	{
		public Rectangle StretchRegion { get; set; }
		public Surface Image { get; set; }
		public Surface Default { get; set; }
		public Surface Pressed { get; set; }
		public Surface Disabled { get; set; }
		public Surface Hover { get; set; }
		public Surface Focus { get; set; }
		public int TextPadding { get; set; }
		public int Margin { get; set; }
	}

}
