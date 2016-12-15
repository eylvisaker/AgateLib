using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Venus;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.StyleModel
{
	public class WidgetStyle
	{
		public WidgetStyle(Widget widget)
		{
			Widget = widget;
			NeedRefresh = true;
		}

		public Widget Widget { get; private set; }

		public WidgetProperties WidgetProperties { get; internal set; }

		public WidgetMetrics Metrics { get; private set; } = new WidgetMetrics();

		public ContainerLayout ContainerLayout { get; private set; } = new ContainerLayout();

		public WidgetLayout WidgetLayout { get; private set; } = new WidgetLayout();

		public BackgroundStyle Background { get; set; } = new BackgroundStyle();

		public BorderStyle Border { get; set; } = new BorderStyle();

		public TransitionStyle Transition { get; set; } = new TransitionStyle();

		public WidgetFontStyle Font { get; set; } = new WidgetFontStyle();

		public BoxModel BoxModel { get; set; } = new BoxModel();

		public Overflow Overflow { get; set; }

		public TextAlign TextAlign { get; set; }

		internal bool NeedRefresh { get; set; }

		public void Clear()
		{
			Metrics = new WidgetMetrics();
			ContainerLayout = new ContainerLayout();
			WidgetLayout = new WidgetLayout();
			Background = new BackgroundStyle();
			Border = new BorderStyle();
			Transition = new TransitionStyle();
			BoxModel.Clear();
		}

		public override string ToString()
		{
			return $"WidgetStyle: {Widget.Name} ({Widget.GetType().Name})";
		}

	}
}
