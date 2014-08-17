using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels.CoordinateSystems
{
	/// <summary>
	/// Constructs a coordinate system which matches the pixels coordinates of the display window,
	/// up to an optional maximum height and width.
	/// </summary>
	public class NaturalCoordinates : ICoordinateSystemCreator
	{
		public Rectangle DetermineCoordinateSystem(Size displayWindowSize)
		{
			Rectangle retval = new Rectangle(Point.Empty, displayWindowSize);

			if (MaxSize != null)
			{
				retval.Width = Math.Min(retval.Width, MaxSize.Value.Width);
				retval.Height = Math.Min(retval.Height, MaxSize.Value.Height);
			}

			return retval;
		}

		public Size? MaxSize { get; set; }
	}
}
