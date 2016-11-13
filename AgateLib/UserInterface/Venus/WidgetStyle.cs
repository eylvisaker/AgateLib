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
		public WidgetStyle(Widget widget)
		{
			Widget = widget;
			NeedRefresh = true;
		}

		public Widget Widget { get; set; }

		public BackgroundStyle Background { get; set; }
		IBackgroundStyle IWidgetStyle.Background { get { return Background; } }

		public BorderStyle Border { get; set; }
		IBorderStyle IWidgetStyle.Border { get { return Border; } }

		public TransitionStyle Transition { get; set; }
		ITransitionStyle IWidgetStyle.Transition { get { return Transition; } }

		public BoxModel BoxModel { get; set; }

		public Color FontColor { get; set; }

		public Overflow Overflow { get; set; }

		public TextAlign TextAlign { get; set; }


		internal bool NeedRefresh { get; set; }
	}
}
