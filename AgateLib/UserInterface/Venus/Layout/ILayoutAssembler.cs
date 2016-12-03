using System.Collections.Generic;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Venus.Layout
{
	internal interface ILayoutAssembler
	{
		bool CanDoLayoutFor(WidgetStyle containerStyle);

		void DoLayout(ILayoutBuilder layoutBuilder, WidgetStyle container, ICollection<WidgetStyle> layoutChildren);

		/// <summary>
		/// Computes the natural size of the widget and assigns it to the widget.Metrics.NaturalBoxSize property.
		/// Returns true if this value is different from the stored value.
		/// </summary>
		/// <param name="venusLayoutEngine"></param>
		/// <param name="containerStyle"></param>
		/// <returns></returns>
		bool ComputeNaturalSize(ILayoutBuilder layoutBuilder, WidgetStyle widget);
	}
}