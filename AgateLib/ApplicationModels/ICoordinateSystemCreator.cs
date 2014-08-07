using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels
{
	public interface ICoordinateSystemCreator
	{
		Rectangle DetermineCoordinateSystem(Size displayWindowSize, double aspectRatio);
	}
}
