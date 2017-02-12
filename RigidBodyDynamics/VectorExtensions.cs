using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace RigidBodyDynamics
{
	public static class VectorExtensions
	{
		public static Vector2 Rotate(this Vector2 parent, float angle)
		{
			return new Vector2(
				Math.Cos(angle) * parent.X + Math.Sin(angle) * parent.Y,
				-Math.Sin(angle) * parent.X + Math.Cos(angle) * parent.Y);
		}
	}
}
