using AgateLib.Geometry;

namespace AgateLib.UserInterface.Rendering
{
	public interface IBackgroundStyle
	{
		BackgroundClip Clip { get; }

		Color Color { get; }

		BackgroundRepeat Repeat { get; }

		string Image { get; }

		Point Position { get; }

	}
}