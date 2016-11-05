namespace AgateLib.UserInterface.Rendering
{
	public interface ITransitionStyle
	{
		WindowTransitionType Type { get; }
		TransitionDirection Direction { get; }
		double Time { get; }
	}
}