using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Layout.MetricsCalculators;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Layout
{
	public class MetricsComputer
	{
		private AgateWidgetAdapter adapter;
		IWidgetMetricsCalculator defaultCalculator = new DefaultMetricsCalculator();

		public MetricsComputer(AgateWidgetAdapter adapter)
		{
			this.adapter = adapter;
		}

		public bool ComputeNaturalSize(Widget item, WidgetStyle style)
		{
			var calculator = FindCalculator(item);

			return calculator.ComputeNaturalSize(style);
		}
		
		private IWidgetMetricsCalculator FindCalculator(Widget item)
		{
			return defaultCalculator;
		}
		

		public bool ComputeBoxSize(WidgetStyle widget, int? maxWidth, int? maxHeight)
		{
			var calculator = FindCalculator(widget.Widget);

			return calculator.ComputeBoxSize(widget, maxWidth, maxHeight);
		}
	}
}
