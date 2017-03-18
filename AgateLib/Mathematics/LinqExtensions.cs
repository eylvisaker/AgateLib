using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics
{
	/// <summary>
	/// LINQ-like extensions for enumerables of mathematics types.
	/// </summary>
	public static class LinqExtensions
	{
		/// <summary>
		/// Sums the vectors in the enumerable.
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public static Vector2 Sum(this IEnumerable<Vector2> points)
		{
			Vector2 result = Vector2.Zero;

			foreach (var point in points)
				result += point;

			return result;
		}

		/// <summary>
		/// Sums the vectors in the enumerable.
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public static Vector2f Sum(this IEnumerable<Vector2f> points)
		{
			Vector2f result = Vector2f.Zero;

			foreach (var point in points)
				result += point;

			return result;
		}

		/// <summary>
		/// Averages the vectors in the enumerable.
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public static Vector2 Average(this IEnumerable<Vector2> points)
		{
			Vector2 sum = Vector2.Zero;
			long count = 0;

			foreach (var point in points)
			{
				sum += point;
				count++;
			}

			return sum / count;
		}

		/// <summary>
		/// Averages the vectors in the enumerable.
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public static Vector2f Average(this IEnumerable<Vector2f> points)
		{
			Vector2f sum = Vector2f.Zero;
			long count = 0;

			foreach (var point in points)
			{
				sum += point;
				count++;
			}

			return sum / count;
		}
	}
}
