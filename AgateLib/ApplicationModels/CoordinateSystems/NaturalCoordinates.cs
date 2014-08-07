using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels.CoordinateSystems
{
	/// <summary>
	/// Constructs a coordinate system that provides a one-to-one mapping to the pixels
	/// in the display window.
	/// </summary>
	public class NaturalCoordinates : ICoordinateSystemCreator
	{
		public Rectangle DetermineCoordinateSystem(Size displayWindowSize, double aspectRatio)
		{
			return new Rectangle(0, 0, displayWindowSize.Width, displayWindowSize.Height);
		}
	}
}
