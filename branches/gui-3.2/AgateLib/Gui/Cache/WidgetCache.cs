using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Gui.Cache
{
	public class WidgetCache
	{
		public WidgetCache()
		{
			Dirty = true;
		}

		public bool Dirty { get; set; }
	}
}
