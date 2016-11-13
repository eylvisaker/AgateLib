using AgateLib.Geometry;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Venus.LayoutModel
{
	public class StyleProperties
	{
		public Color? TextColor { get; set; }
		public TextAlign? TextAlign { get; set; }
		public Overflow? Overflow { get; set; }
		public BoxModel BoxModel { get; set; }

		public BorderStyle Border { get; set; }
		public BackgroundStyle Background { get; set; }
		public TransitionProperties Transition { get; set; }
	}
}