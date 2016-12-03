using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout.MetricsCalculators
{
	public class DefaultMetricsCalculator : IWidgetMetricsCalculator
	{
		public void ComputeMetrics(Widget item, WidgetMetrics metrics)
		{
			metrics.MinTotalSize = new Size(10, 10);
			metrics.MaxTotalSize = new Size(1000, 1000);
		}

		public bool ComputeNaturalSize(Widget item, WidgetStyle style)
		{
			throw new NotImplementedException();
		}
	}
}
