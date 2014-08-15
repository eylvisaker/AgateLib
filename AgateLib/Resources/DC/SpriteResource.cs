using AgateLib.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AgateLib.Resources.DC
{
	public class SpriteResource : AgateResource
	{
		public SpriteResource()
		{
			Frames = new List<SpriteFrameResource>();

			TimePerFrame = 60;
			AnimType = SpriteAnimType.Looping;

		}

		public IList<SpriteFrameResource> Frames { get; set; }

		public double TimePerFrame { get; set; }
		public SpriteAnimType AnimType { get; set; }
	}
}
