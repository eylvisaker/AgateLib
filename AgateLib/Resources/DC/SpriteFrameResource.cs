using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Resources.DC
{
	public class SpriteFrameResource
	{
		public SpriteFrameResource()
		{
			CollisionRegions = new Dictionary<string, Polygon>();
		}

		public string ImageFilename { get; set; }
		public Rectangle SourceRect { get; set; }
		public Point Anchor { get; set; }

		public IDictionary<string, Polygon> CollisionRegions { get; set; }
	}
}
