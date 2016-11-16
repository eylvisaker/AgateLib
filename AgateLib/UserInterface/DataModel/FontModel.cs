using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;

namespace AgateLib.UserInterface.DataModel
{
	public class FontModel
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public Dictionary<int, GlyphMetrics> Metrics { get; set; } = new Dictionary<int, GlyphMetrics>();
	}
}
