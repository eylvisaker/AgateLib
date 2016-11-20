using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.DisplayLib.BitmapFont
{
	public class FontModel
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public Dictionary<int, GlyphMetrics> Metrics { get; set; } = new Dictionary<int, GlyphMetrics>();
	}
}
