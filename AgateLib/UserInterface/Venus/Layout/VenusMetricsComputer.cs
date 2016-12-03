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

			return calculator.ComputeNaturalSize(item, style);
		}

		public void ComputeMetrics(Widget item)
		{
			var container = item as Container;

			if (container != null)
			{
				ComputeContainerMetrics(container);
			}
			else
			{
				var style = adapter.StyleOf(item);

				var calculator = FindCalculator(item);

				calculator.ComputeMetrics(item, style.Metrics);

				AddBoxModel(style);
			}
		}

		private static void AddBoxModel(WidgetStyle style)
		{
			style.Metrics.MaxTotalSize = new Size(
				style.Metrics.MaxContentSize.Width + style.BoxModel.Width,
				style.Metrics.MaxContentSize.Height + style.BoxModel.Height);

			style.Metrics.MinTotalSize = new Size(
				style.Metrics.MinContentSize.Width + style.BoxModel.Width,
				style.Metrics.MinContentSize.Height + style.BoxModel.Height);
		}

		private IWidgetMetricsCalculator FindCalculator(Widget item)
		{
			return defaultCalculator;
		}

		private void ComputeContainerMetrics(Container container)
		{
			var containerStyle = adapter.StyleOf(container);

			Size minContentSize = new Size();
			Size maxContentSize = new Size();

			foreach (var item in container.Children)
			{
				ComputeMetrics(item);

				var style = adapter.StyleOf(item);

				minContentSize.Width += style.Metrics.MinContentSize.Width;
				minContentSize.Height += style.Metrics.MinContentSize.Height;

				maxContentSize.Width += style.Metrics.MaxContentSize.Width;
				maxContentSize.Height += style.Metrics.MaxContentSize.Height;
			}

			containerStyle.Metrics.MinContentSize = minContentSize;
			containerStyle.Metrics.MaxContentSize = maxContentSize;

			AddBoxModel(containerStyle);
		}

		public bool ComputeBoxSize(WidgetStyle widget, int? maxWidth, int? maxHeight)
		{
			var calculator = FindCalculator(widget.Widget);

			return calculator.ComputeBoxSize(widget, maxWidth, maxHeight);
		}
	}
}
