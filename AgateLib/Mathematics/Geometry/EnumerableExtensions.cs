using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Mathematics.Geometry
{
	/// <summary>
	/// Provides extension methods for enumerables based on types in AgateLib.Mathematics.Geometry.
	/// </summary>
	public static class EnumerableExtensions
	{
		public static Vector2List ToVector2List(this IEnumerable<Vector2> vectors)
		{
			return new Vector2List(vectors);
		}
	}
}
