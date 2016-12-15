using System.Collections.Generic;
using AgateLib.UserInterface.StyleModel;

namespace AgateLib.UserInterface.Venus.Layout
{
	public interface ILayoutAssembler
	{
		/// <summary>
		/// Returns true if this layout assembler can do the layout for the specified widget.
		/// </summary>
		/// <param name="containerStyle">WidgetStyle object for the container.</param>
		/// <returns></returns>
		bool CanDoLayoutFor(WidgetStyle containerStyle);

		/// <summary>
		/// Performs layout fo the specified container and children.
		/// </summary>
		/// <param name="layoutBuilder">The layout builder that will be used to perform layout of child container contents.</param>
		/// <param name="container">The container this object is doing layout for.</param>
		/// <param name="layoutChildren">The container children that participate in layout.</param>
		/// <param name="maxWidth">The maximum width of the container client area.</param>
		/// <param name="maxHeight">The maximum height of the container client area.</param>
		void DoLayout(ILayoutBuilder layoutBuilder, WidgetStyle container, ICollection<WidgetStyle> layoutChildren,
			int? maxWidth = null, int? maxHeight = null);

		/// <summary>
		/// Computes the natural size of the widget and assigns it to the widget.Metrics.NaturalBoxSize property.
		/// Returns true if this value is different from the stored value.
		/// </summary>
		/// <param name="layoutBuilder">The layout builder that will be used to perform layout of child container contents.</param>
		/// <param name="widget">The container layout is being done on.</param>
		/// <returns></returns>
		bool ComputeNaturalSize(ILayoutBuilder layoutBuilder, WidgetStyle widget);
	}
}