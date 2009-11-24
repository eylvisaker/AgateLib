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
		public FrameBuffer TextBoxFrameBuffer { get; set; }
		public Surface TextBoxSurface
		{
			get
			{
				if (TextBoxFrameBuffer == null) 
					return null;
				else
					return TextBoxFrameBuffer.BackBuffer;
			}
		}

		public Point Origin;
	}
}
