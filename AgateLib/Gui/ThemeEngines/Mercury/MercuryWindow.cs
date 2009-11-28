using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui.ThemeEngines.Mercury
{
	public class MercuryWindow : MercurySchemeCommon
	{
		public Surface NoTitle { get; set; }
		public Surface WithTitle { get; set; }
		public Surface TitleBar { get; set; }
		public Rectangle NoTitleStretchRegion { get; set; }
		public Rectangle WithTitleStretchRegion { get; set; }
		public Rectangle TitleBarStretchRegion { get; set; }

		public bool CenterTitle { get; set; }

		public Surface CloseButton { get; set; }
		public Surface CloseButtonHover { get; set; }
		public Surface CloseButtonInactive { get; set; }

		public MercuryWindow()
		{
			CenterTitle = true;
		}
	}
}
