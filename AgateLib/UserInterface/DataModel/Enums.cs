namespace AgateLib.UserInterface.DataModel
{
	public enum WidgetDock
	{
		None,
		Fill,
	}

	public enum LayoutDirection
	{
		Column,
		Row,
	}

	public enum LayoutWrap
	{
		None,
		Wrap,
	}

	public enum WidgetLayoutType
	{
		/// <summary>
		/// Indicates the widget is part of the layout.
		/// </summary>
		Flow,

		/// <summary>
		/// Indicates the widget does not participate in layout.
		/// </summary>
		Fixed,
	}
}