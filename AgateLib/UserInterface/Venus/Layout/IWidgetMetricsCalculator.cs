using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout
{
	internal interface IWidgetMetricsCalculator
	{
		/// <summary>
		/// Calculates the size of the item, in the absense of 
		/// any constraints.
		/// </summary>
		/// <param name="style"></param>
		/// 
		/// <returns></returns>
		bool ComputeNaturalSize(WidgetStyle style);

		/// <summary>
		/// Computes the size of the item given the constraints.
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="maxWidth"></param>
		/// <param name="maxHeight"></param>
		/// <returns></returns>
		bool ComputeBoxSize(WidgetStyle widget, int? maxWidth, int? maxHeight);
	}
}