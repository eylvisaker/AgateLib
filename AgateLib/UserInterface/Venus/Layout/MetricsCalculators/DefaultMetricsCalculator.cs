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
		public bool ComputeBoxSize(WidgetStyle widget, int? maxWidth, int? maxHeight)
		{
			var newBoxSize = widget.Metrics.NaturalBoxSize;

			if (newBoxSize.Width > maxWidth)
				newBoxSize.Width = maxWidth.Value;
			if (newBoxSize.Height > maxHeight)
				newBoxSize.Height = maxHeight.Value;

			if (newBoxSize == widget.Metrics.BoxSize)
				return false;

			widget.Metrics.BoxSize = newBoxSize;

			return true;
		}

		public void ComputeMetrics(Widget item, WidgetMetrics metrics)
		{
			metrics.MinTotalSize = new Size(10, 10);
			metrics.MaxTotalSize = new Size(1000, 1000);
		}

		public bool ComputeNaturalSize(Widget item, WidgetStyle style)
		{
			style.Metrics.MinTotalSize = new Size(
				style.WidgetLayout.MinWidth ?? 0 + style.BoxModel.Margin.Left + style.BoxModel.Border.Right,
				style.WidgetLayout.MinHeight ?? 0 + style.BoxModel.Margin.Top + style.BoxModel.Border.Bottom);
			style.Metrics.MaxTotalSize = new Size(
				style.WidgetLayout.MaxWidth ?? int.MaxValue,
				style.WidgetLayout.MaxHeight ?? int.MaxValue);

			var size = new Size(100, 10);

			if (style.Metrics.NaturalBoxSize == size)
				return false;

			style.Metrics.NaturalBoxSize = size;

			return true;
		}
	}
}
