using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.Gui.Cache;

namespace AgateLib.Gui.ThemeEngines.Mercury.Cache
{
	class ScrollBarCache : WidgetCache
	{
		public bool DownInDecrease { get; set; }
		public bool DownInIncrease { get; set; }
		public bool DownInPageDecrease { get; set; }
		public bool DownInPageIncrease { get; set; }

		public double LastUpdate { get; set; }

		public bool DragThumb { get; set; }
		public Point ThumbGrabSpot { get; set; }
	}
}
