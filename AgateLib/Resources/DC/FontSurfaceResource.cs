using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AgateLib.Resources.DC
{
	public class FontSurfaceResource
	{
		public FontSurfaceResource()
		{
			FontMetrics = new FontMetrics();
		}

		public string ImageFilename { get; set; }
		public FontMetrics FontMetrics { get; set; }
		public FontSettings FontSettings { get; set; }
	}
}
