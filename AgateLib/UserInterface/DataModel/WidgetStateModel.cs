using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetStateModel
	{
		public Color? TextColor { get; set; }
		public TextAlign? TextAlign { get; set; }
		public Overflow? Overflow { get; set; }

		public WidgetBackgroundModel Background { get; set; }
		public WidgetBorderModel Border { get; set; } 
	}
}