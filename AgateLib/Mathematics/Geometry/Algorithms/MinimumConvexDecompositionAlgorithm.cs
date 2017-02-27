﻿using System;
using System.Collections.Generic;

namespace AgateLib.Mathematics.Geometry
{
	public class MinimumConvexDecompositionAlgorithm : IPolygonConvexDecompositionAlgorithm
	{
		public IReadOnlyList<Polygon> Decompose(Polygon polygon)
		{
			// TODO: Implement this algorithm.
			return new List<Polygon> { polygon };
		}
	}
}