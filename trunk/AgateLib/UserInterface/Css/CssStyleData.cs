using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Parser;
using AgateLib.UserInterface.Css.Selectors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssStyleData : ICssCanSelect
	{
		public CssStyleData()
		{
			Clear();
		}

		public void Clear()
		{
			Selector = "";

			Position = new CssRectangle();
			Font = new CssFont();
			Background = new CssBackground();
			Margin = new CssBoxComponent();
			Padding = new CssBoxComponent();
			Border = new CssBorder();
			Layout = new CssLayout();
			Transition = new CssTransition();

			Display = CssDisplay.Initial;
		}

		public CssSelectorGroup Selector { get; set; }

		[CssPromoteProperties]
		public CssRectangle Position { get; set; }

		[CssPromoteProperties]
		public CssFont Font { get; set; }

		[CssPromoteProperties(prefix: "background")]
		public CssBackground Background { get; set; }

		[CssPromoteProperties(prefix: "margin")]
		public CssBoxComponent Margin { get; set; }

		[CssPromoteProperties(prefix: "padding")]
		public CssBoxComponent Padding { get; set; }

		[CssPromoteProperties(prefix: "border")]
		public CssBorder Border { get; set; }

		[CssPromoteProperties]
		public CssLayout Layout { get; set; }

		public CssDisplay Display { get; set; }

		[CssPromoteProperties(prefix: "transition")]
		public CssTransition Transition { get; set; }
	}
}
