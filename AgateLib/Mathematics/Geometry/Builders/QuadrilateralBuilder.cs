using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry.Builders
{
	/// <summary>
	/// Contains methods used to build lists of points that represent
	/// rectangles, squares and other quadrilaterals.
	/// </summary>
	public class QuadrilateralBuilder
	{
		/// <summary>
		/// Returns a list of points that can be used to trace the perimeter
		/// of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public IEnumerable<Vector2> BuildRectangle(Rectangle rect)
		{
			return BuildRectangle((RectangleF)rect);
		}

		/// <summary>
		/// Returns a list of points that can be used to trace the perimeter
		/// of a rectangle.
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public IEnumerable<Vector2> BuildRectangle(RectangleF rect)
		{
			return new[]
			{
				new Vector2(rect.Left, rect.Top),
				new Vector2(rect.Right, rect.Top),
				new Vector2(rect.Right, rect.Bottom),
				new Vector2(rect.Left, rect.Bottom)
			};
		}
	}
}
