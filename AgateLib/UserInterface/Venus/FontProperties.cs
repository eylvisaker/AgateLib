using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Venus
{
	public class FontProperties : IFontProperties
	{
		public string Family { get; set; }
		public int Size { get; set; }
		public Color Color { get; set; } = Color.Black;
		public FontStyles Style { get; set; }
	}
}