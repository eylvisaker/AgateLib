using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Css;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Rendering
{
	public interface IWidgetStyle
	{
		Widget Widget { get; }

		BoxModel BoxModel { get; }

		Overflow Overflow { get; }
		CssText Text { get; }
		CssFont Font { get; }
		CssBackground Background { get; }
		CssBorder Border { get; }
		CssTransition Transition { get; }
	}
}
