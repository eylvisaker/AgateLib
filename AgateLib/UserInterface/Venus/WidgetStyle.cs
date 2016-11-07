using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus
{
	public class WidgetStyle : IWidgetStyle
	{
		IBackgroundStyle IWidgetStyle.Background { get { return Background; } }

		IBorderStyle IWidgetStyle.Border { get { return Border; } }

		ITransitionStyle IWidgetStyle.Transition { get { return Transition; } }

		public BackgroundStyle Background { get; set; }

		public BorderStyle Border { get; set; }

		public TransitionStyle Transition { get; set; }

		public BoxModel BoxModel { get; set; }

		public Color FontColor { get; set; }

		public Overflow Overflow { get; set; }

		public TextAlign TextAlign { get; set; }

		public Widget Widget { get; set; }
	}
}
