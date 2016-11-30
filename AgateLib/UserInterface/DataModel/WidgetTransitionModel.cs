using AgateLib.UserInterface.Rendering;

namespace AgateLib.UserInterface.DataModel
{
	public class WidgetTransitionModel
	{
		public WindowTransitionType? Type { get; set; }
		public TransitionDirection? Direction { get; set; }
		public double? Time { get; set; }
	}
}