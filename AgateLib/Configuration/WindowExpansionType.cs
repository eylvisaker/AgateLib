namespace AgateLib.Configuration
{
	/// <summary>
	/// Enum which is used to indicate how auto-created DisplayWindow
	/// objects have their size scaled to match the monitor's aspect ratio.
	/// </summary>
	public enum WindowExpansionType
	{
		/// <summary>
		/// Indicates that the vertical dimension will remain fixed, and the 
		/// horizontal dimension will be expanded or contracted to match the 
		/// display aspect ratio.
		/// </summary>
		/// <remarks>
		/// For example, if the desired resolution is set to 640x480 but the 
		/// monitor is a widescreen 1920x1080, the actual logical resolution
		/// of the DisplayWindow will be set to 853x480, so that the logical
		/// resolution has the same aspect ratio as the physical monitor.
		/// </remarks>
		VerticalSizeFixed,

		/// <summary>
		/// Indicates that the horizontal dimension will remain fixed, and the 
		/// vertical dimension will be expanded or contracted to match the 
		/// display aspect ratio.
		/// </summary>
		HorizontalSizeFixed,

		/// <summary>
		/// Indicates that the logical dimensions will remain fixed, and the
		/// display will be scaled without regard to the physical aspect ratio
		/// of the monitor (not recommended).
		/// </summary>
		Scale,
	}
}