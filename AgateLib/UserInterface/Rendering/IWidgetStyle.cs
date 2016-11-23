using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
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

		IBackgroundStyle Background { get; }

		IBorderStyle Border { get; }

		ITransitionStyle Transition { get; }

		IFontProperties Font { get; }
	}

	public interface IFontProperties
	{
		string Family { get; }
		int Size { get; }
		Color Color { get; }
		FontStyles Style { get; }
	}
}
