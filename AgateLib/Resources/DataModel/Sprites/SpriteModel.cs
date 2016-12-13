using System.Collections.Generic;
using AgateLib.Geometry;

namespace AgateLib.Resources.DataModel.Sprites
{
	public class SpriteModel
	{
		public List<SpriteFrameModel> Frames { get; set; }

		public AnimationModel Animation { get; set; }

		public string Image { get; set; }

		public Size Size { get; set; }
	}
}