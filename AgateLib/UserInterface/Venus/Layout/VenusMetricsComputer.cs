using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.Venus.Layout.MetricsCalculators;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout
{
	public class VenusMetricsComputer
	{
		private VenusWidgetAdapter adapter;
		IWidgetMetricsCalculator defaultCalculator = new DefaultMetricsCalculator();

		public VenusMetricsComputer(VenusWidgetAdapter adapter)
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
