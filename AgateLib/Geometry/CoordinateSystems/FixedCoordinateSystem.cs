using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Geometry.CoordinateSystems
{
	public class FixedCoordinateSystem : ICoordinateSystemCreator
	{
		public FixedCoordinateSystem(Rectangle coords)
		{
			Coordinates = coords;
		}

		public Rectangle Coordinates { get; set; }

		public Rectangle DetermineCoordinateSystem(Size displayWindowSize)
		{
			return Coordinates;
		}
	}
}
