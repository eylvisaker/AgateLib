using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Layout
{
	public class CssBoxModel
	{
		public CssBox Margin { get; set; }
		public CssBox Padding { get; set; }
		public CssBox Border { get; set; }

		public int Top { get { return Margin.Top + Padding.Top + Border.Top; } }
		public int Left { get { return Margin.Left + Padding.Left + Border.Left; } }
		public int Right { get { return Margin.Right + Padding.Right + Border.Right; } }
		public int Bottom { get { return Margin.Bottom + Padding.Bottom + Border.Bottom; } }
	}
}
