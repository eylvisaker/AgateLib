using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels
{
	public interface ICoordinateSystemCreator
	{
		Rectangle DetermineCoordinateSystem(Size displayWindowSize);
	}

	public static class CoordinateSystemCreatorExtensions
	{
		public static Rectangle DetermineCoordinateSystem(
			this ICoordinateSystemCreator csc, Size displayWindowSize)
		{
			return csc.DetermineCoordinateSystem(displayWindowSize);
		}
	}
}
