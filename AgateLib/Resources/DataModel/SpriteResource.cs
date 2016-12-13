using System.Collections.Generic;
using AgateLib.Geometry;

namespace AgateLib.Resources.DataModel
{
	public class SpriteResource
	{
		public List<SpriteFrameResource> Frames { get; set; }

		public SpriteResourceAnimationData Animation { get; set; }

		public string Image { get; set; }

		public Size Size { get; set; }
	}
}