using System.Collections.Generic;

namespace AgateLib.Resources.DataModel.Sprites
{
	public class SpriteModel
	{
		public List<SpriteFrameModel> Frames { get; set; }

		public AnimationModel Animation { get; set; }
	}
}