using System.Collections.Generic;

namespace AgateLib.Mathematics.Geometry
{
	public interface IPolygonConvexDecompositionAlgorithm
	{
		IReadOnlyList<Polygon> BuildConvexDecomposition(Polygon polygon);
	}
}