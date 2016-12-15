using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.StyleModel
{
	public class WidgetFontStyle
	{
		public string Family { get; set; }
		public int Size { get; set; }
		public Color Color { get; set; } = AgateLib.Geometry.Color.Black;
		public FontStyles Style { get; set; }
	}
}