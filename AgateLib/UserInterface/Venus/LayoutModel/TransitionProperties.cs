using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.Venus.LayoutModel
{
	public class TransitionProperties
	{
		public TransitionDirection? Direction { get; set; }
		public WindowTransitionType? Type { get; set; }
		public double Time { get; set; }
	}
}