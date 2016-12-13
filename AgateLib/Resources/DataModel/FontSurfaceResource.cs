using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;

namespace AgateLib.Resources.DataModel
{
	public class FontSurfaceResource
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public int Size { get; set; }
		public FontStyles Style { get; set; }

		public FontMetrics Metrics { get; set; } = new FontMetrics();

		public List<KerningPairModel> Kerning { get; set; } = new List<KerningPairModel>();
	}
}
