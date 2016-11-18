using AgateLib.Geometry;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetStateModel
	{
		public WidgetBackgroundModel Background { get; set; }
		public WidgetBorderModel Border { get; set; } 
		public Color? TextColor { get; set; }
	}
}