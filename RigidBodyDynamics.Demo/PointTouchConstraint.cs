using System;
using AgateLib.Geometry;

namespace RigidBodyDynamics
{
	public class PointTouchConstraint : IPhysicalConstraint
	{
		private Physical physical1;
		private Physical physical2;
		private Vector2 point1;
		private Vector2 point2;

		public PointTouchConstraint(Physical physical1, Vector2 point1, Physical physical2, Vector2 point2)
		{
			this.physical1 = physical1;
			this.physical2 = physical2;
			this.point1 = point1;
			this.point2 = point2;
		}

		public bool AppliesTo(Physical obj)
		{
			if (physical1 == obj)
				return true;
			if (physical2 == obj)
				return true;

			return false;
		}

		/// <summary>
		/// Returns the current value of the constraint equation. This can be used to measure the amount of error in the
		/// algorithm.
		/// </summary>
		/// <returns></returns>
		public double Evaluate()
		{
			var abs1 = ComputeAbsPoint(physical1, point1);
			var abs2 = ComputeAbsPoint(physical2, point2);

			return (abs1 - abs2).MagnitudeSquared;
		}

		public ConstraintDerivative Derivative(Physical obj)
		{
			int sign = (obj == physical1) ? 1 : -1;

			var abs1 = ComputeAbsPoint(physical1, point1);
			var abs2 = ComputeAbsPoint(physical2, point2);

			return sign * new ConstraintDerivative(
				2 * abs1.X,
				2 * abs2.Y,
				-2 * abs1.X * (float)Math.Sin(obj.Angle) + 2 * abs2.Y * (float)Math.Cos(obj.Angle));
		}

		private Vector2 ComputeAbsPoint(Physical obj, Vector2 pt)
		{
			return obj.Position +
						   new Vector2(Math.Cos(obj.Angle) * pt.X, Math.Sign(obj.Angle) * pt.Y);
		}
	}
}