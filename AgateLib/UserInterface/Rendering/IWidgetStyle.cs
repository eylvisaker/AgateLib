using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
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

		IBackgroundStyle Background { get; }

		IBorderStyle Border { get; }

		ITransitionStyle Transition { get; }
	}
}
