using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry
{
	public struct LineSegment
	{
		public Vector2 Start { get; set; }
		public Vector2 End { get; set; }

		/// <summary>
		/// Returns the displacement of this vector, equivalent to End - Start.
		/// </summary>
		public Vector2 Displacement => End - Start;

		/// <summary>
		/// The point in the center of the line segment.
		/// </summary>
		public Vector2 Center => (End + Start) / 2;
	}
}
