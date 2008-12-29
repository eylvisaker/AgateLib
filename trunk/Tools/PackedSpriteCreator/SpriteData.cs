using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace PackedSpriteCreator
{
	class SpriteData
	{
		public SpriteData()
		{
			Frames = new List<SpriteFrameData>();
		}

		public List<SpriteFrameData> Frames { get; private set; }
		public Size FrameSize { get; set; }
	}
}
