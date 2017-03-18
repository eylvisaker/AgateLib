namespace AgateLib.Mathematics.Geometry.Algorithms
{
	public static class PolygonExtensions
	{
		/// <summary>
		/// Returns the vertex of a polygon with wrap-around in the vertex index.
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static Vector2 At(this Polygon polygon, int index)
		{
			while (index < 0)
				index += polygon.Count;

			index %= polygon.Count;

			return polygon[index];
		}

		/// <summary>
		/// Indicates if the vertices are in counter clockwise order.
		/// Warning: If the area of the polygon is 0, it is unable to determine the winding.
		/// </summary>
		public static bool IsCounterClockWise(this Polygon polygon)
		{
			if (polygon.Count < 3)
				return false;

			return (polygon.GetSignedArea() > 0.0f);
		}

		/// <summary>
		/// Gets the signed area, which is positive if the winding of the polygon is counterclockwise.
		/// </summary>
		public static double GetSignedArea(this Polygon polygon)
		{
			if (polygon.Count < 3)
				return 0;

			int i;
			double area = 0;

			for (i = 0; i < polygon.Count; i++)
			{
				int j = (i + 1) % polygon.Count;

				Vector2 vi = polygon[i];
				Vector2 vj = polygon[j];

				area += vi.X * vj.Y;
				area -= vi.Y * vj.X;
			}

			area /= 2.0;
			return area;
		}

		/// <summary>
		/// Copies a range of vertices from a polygon with wrap-around on the indices.
		/// </summary>
		/// <param name="polygon"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <returns></returns>
		public static Polygon CopyRange(this Polygon polygon, int start, int end)
		{
			Polygon p = new Polygon();
			while (end < start)
				end += polygon.Count;

			for (; start <= end; ++start)
			{
				p.Add(polygon.At(start));
			}

			return p;
		}

	}
}
