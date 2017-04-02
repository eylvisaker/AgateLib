using System.Collections;
using System.Collections.Generic;

namespace AgateLib.Mathematics.Geometry.Algorithms.CollisionDetection
{
	public class MinkowskiSimplex : IEnumerable<Vector2>
	{
		public Polygon Simplex = new Polygon();
		public double DistanceFromOrigin;

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<Vector2> GetEnumerator()
		{
			return Simplex.GetEnumerator();
		}
	}
}