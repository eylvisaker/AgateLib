using AgateLib.Geometry;

namespace AgateLib.UserInterface.Venus
{
	public class WidgetMetrics
	{
		/// <summary>
		/// The minimum dimensions of the content. This represents the absolute minimum width and height separately.
		/// Only one of these will be used for a constraint.
		/// </summary>
		public Size MinContentSize { get; set; }
		/// <summary>
		/// The maximum dimensions of the content.
		/// </summary>
		public Size MaxContentSize { get; set; }

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
	}
}