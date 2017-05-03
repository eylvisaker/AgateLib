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

		public LineSegment LineSegment { get; private set; }

		public Vector2 Normal { get; private set; }

		internal void SetValue(LineSegment segment, Vector2 normal)
		{
			LineSegment = segment;
			Normal = normal;
		}
	}
}
