using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui.ThemeEngines.Mercury.Cache
{
	class TextBoxCache : Gui.Cache.WidgetCache
	{
		public Surface TextBoxSurface { get; set; }

		public Point Origin;
	}
}
