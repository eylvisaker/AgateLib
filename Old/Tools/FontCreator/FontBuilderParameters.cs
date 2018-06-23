using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Quality;

namespace FontCreatorApp
{
	public class FontBuilderParameters
	{
		List<int> fontSizes = new List<int> { 8, 9, 10, 12, 14, 16, 18, 24, 30 };

		public List<int> FontSizes
		{
			get { return fontSizes; }
			set
			{
				Require.ArgumentNotNull(value, nameof(FontSizes));
				fontSizes = value;
			}
		}

		public string Family { get; set; }

		public bool Bold { get; set; }
		public bool Underline { get; set; }
		public bool Italic { get; set; }
		public bool Strikeout { get; set; }
		public int TopMarginAdjust { get; set; }
		public int BottomMarginAdjust { get; set; }
		public Color BorderColor { get; set; }
		public bool CreateBorder { get; set; }
		public BitmapFontEdgeOptions EdgeOptions { get; set; }
		public bool MonospaceNumbers { get; set; } = true;
		public int NumberWidthAdjust { get; set; }
		public TextRenderEngine TextRenderer { get; set; }

		public string SaveName { get; set; }
	}
}
