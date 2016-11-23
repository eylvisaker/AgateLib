using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus
{
	public class WidgetStyle : IWidgetStyle
	{
		private IFontProperties font;

		public WidgetStyle(Widget widget)
		{
			Widget = widget;
			NeedRefresh = true;
		}

		public Widget Widget { get; set; }

		public BackgroundStyle Background { get; set; } = new BackgroundStyle();
		IBackgroundStyle IWidgetStyle.Background { get { return Background; } }

		public BorderStyle Border { get; set; } = new BorderStyle();
		IBorderStyle IWidgetStyle.Border { get { return Border; } }

		public TransitionStyle Transition { get; set; } = new TransitionStyle();

		public FontProperties Font { get; set; } = new FontProperties();
		IFontProperties IWidgetStyle.Font {  get { return Font; } }

		ITransitionStyle IWidgetStyle.Transition { get { return Transition; } }

		public BoxModel BoxModel { get; set; } = new BoxModel();

		public Overflow Overflow { get; set; }

		public TextAlign TextAlign { get; set; }

		internal bool NeedRefresh { get; set; }
	}
}
