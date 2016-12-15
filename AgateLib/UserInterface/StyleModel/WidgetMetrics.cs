using AgateLib.Geometry;

namespace AgateLib.UserInterface.StyleModel
{
	public class WidgetMetrics
	{
		/// <summary>
		/// The minimum dimensions of the widget, including borders, padding and margins.
		/// </summary>
		public Size MinTotalSize { get; set; }
		/// <summary>
		/// The maximum dimensions of the widget, including borders, padding and margins.
		/// </summary>
		public Size MaxTotalSize { get; set; }

		/// <summary>
		/// The actual size of the content area.
		/// </summary>
		public Size ContentSize { get; set; }
		
		/// <summary>
		/// The size of the control, including margins, borders and padding.
		/// </summary>
		public Size BoxSize { get; set; }

		/// <summary>
		/// The size of the control, including margins, borders and padding in 
		/// the absence of any constraints.
		/// </summary>
		public Size NaturalBoxSize { get; set; }
	}
}