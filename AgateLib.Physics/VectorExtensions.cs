using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;

namespace AgateLib.Physics
{
	public static class VectorExtensions
	{
		public static Vector2 RotationDerivative(this Vector2 parent, double angle)
		{
			return new Vector2(
				-Math.Sin(angle) * parent.X + Math.Cos(angle) * parent.Y,
				-Math.Cos(angle) * parent.X - Math.Sin(angle) * parent.Y);
		}
	}
}
