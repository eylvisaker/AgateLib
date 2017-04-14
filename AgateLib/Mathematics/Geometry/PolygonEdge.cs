using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry
{
	public class PolygonEdge
	{
		public PolygonEdge(LineSegment lineSegment, Vector2 normal)
		{
			LineSegment = lineSegment;
			Normal = normal;
		}

		public LineSegment LineSegment { get; }

		public Vector2 Normal { get; }
	}
}
