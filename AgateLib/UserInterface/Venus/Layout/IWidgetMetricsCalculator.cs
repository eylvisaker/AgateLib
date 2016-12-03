using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout
{
	internal interface IWidgetMetricsCalculator
	{
		void ComputeMetrics(Widget item, WidgetMetrics metrics);
		bool ComputeNaturalSize(Widget item, WidgetStyle style);
		bool ComputeBoxSize(WidgetStyle widget, int? maxWidth, int? maxHeight);
	}
}