using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public Widget Widget { get; private set; }

		public WidgetMetrics Metrics { get; private set; } = new WidgetMetrics();

		public ContainerLayout ContainerLayout { get; private set; } = new ContainerLayout();

		public WidgetLayout WidgetLayout { get; private set; } = new WidgetLayout();

		public BackgroundStyle Background { get; set; } = new BackgroundStyle();
		IBackgroundStyle IWidgetStyle.Background { get { return Background; } }

		public BorderStyle Border { get; set; } = new BorderStyle();
		IBorderStyle IWidgetStyle.Border { get { return Border; } }

		public TransitionStyle Transition { get; set; } = new TransitionStyle();

		public FontProperties Font { get; set; } = new FontProperties();
		IFontProperties IWidgetStyle.Font { get { return Font; } }

		ITransitionStyle IWidgetStyle.Transition { get { return Transition; } }

		public BoxModel BoxModel { get; set; } = new BoxModel();

		public Overflow Overflow { get; set; }

		public TextAlign TextAlign { get; set; }

		internal bool NeedRefresh { get; set; }

		public override string ToString()
		{
			return $"WidgetStyle: {Widget.Name} ({Widget.GetType().Name})";
		}
	}
}
