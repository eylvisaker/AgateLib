using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
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

		TextAlign TextAlign { get; }

		Color FontColor { get; }

		BackgroundClip BackgroundClip { get; }

		Color BackgroundColor { get; }

		BackgroundRepeat BackgroundRepeat { get; }

		string BackgroundImage { get; }

		Point BackgroundPosition { get; }

		CssBorder Border { get; }

		CssTransition Transition { get; }
	}
}
