using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetBackgroundModel
	{
		public string Image { get; set; }
		public Color? Color { get; set; }
		public BackgroundRepeat? Repeat { get; set; }
		public BackgroundClip? Clip { get; set; }
		public Point? Position { get; set; }

	}
}