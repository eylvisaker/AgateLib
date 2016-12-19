using AgateLib.DisplayLib.Sprites;

namespace AgateLib.Resources.DataModel
{
	public class SpriteResourceAnimationData
	{
		/// <summary>
		/// The amount of time in milliseconds that each frame should be displayed.
		/// </summary>
		public int FrameTime { get; set; }

		public SpriteAnimType Type { get; set; }
	}
}