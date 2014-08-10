using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssRectangle : ICssPropertyFromText
	{
		public CssDistance Top { get; set; }
		public CssDistance Left { get; set; }
		public CssDistance Right { get; set; }
		public CssDistance Bottom { get; set; }

		public CssDistance Width { get; set; }
		public CssDistance Height { get; set; }

		[CssAlias("min-width")]
		public CssDistance MinWidth { get; set; }
		[CssAlias("min-height")]
		public CssDistance MinHeight { get; set; }

		[CssAlias("max-width")]
		public CssDistance MaxWidth { get; set; }
		[CssAlias("max-height")]
		public CssDistance MaxHeight { get; set; }

		public CssRectangle()
		{
			Top = new CssDistance(true);
			Left = new CssDistance(true);
			Right = new CssDistance(true);
			Bottom = new CssDistance(true);

			Width = new CssDistance(true);
			Height = new CssDistance(true);

			MinWidth = new CssDistance(true);
			MinHeight = new CssDistance(true);

			MaxWidth = new CssDistance(true);
			MaxHeight = new CssDistance(true);
		}

		public void SetValueFromText(string value)
		{
			
		}
	}
}
