using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AgateLib.UserInterface.Css
{
	public class CssFont
	{
		public CssFont()
		{
			Color = Color.Black;
		}

		[CssAlias("font-family")]
		public string Family { get; set; }

		[CssAlias("font-size")]
		public CssDistance Size { get; set; }

		public Color Color { get; set; }

	}
}
