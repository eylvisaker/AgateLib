namespace AgateLib.Physics.TwoDimensions
{
	/// <summary>
	/// Compute the derivative of your constraint, with respect to each of the particle's values.
	/// </summary>
	public struct ConstraintDerivative
	{
		/// <summary>
		/// Compute the derivative of your constraint, with respect to each of the particle's values.
		/// </summary>
		/// <param name="respectToX">The derivative of your constraint with respect to the particle's 
		/// Position.X variable. Evaluate the derivative numerically at the particle's current position.</param>
		/// <param name="respectToY">The derivative of your constraint with respect to the particle's 
		/// Position.Y variable. Evaluate the derivative numerically at the particle's current position.</param>
		/// <param name="respectToAngle">The derivative of your constraint with respect to the particle's 
		/// angle variable. Evaluate the derivative numerically at the particle's current position.</param>
		public ConstraintDerivative(double respectToX, double respectToY, double respectToAngle)
		{
			RespectToX = respectToX;
			RespectToY = respectToY;
			RespectToAngle = respectToAngle;
		}

		/// <summary>
		/// The derivative of your constraint with respect to the particle's 
		/// Position.X variable. Evaluate the derivative numerically at the particle's current position.
		/// </summary>
		public double RespectToX;

		/// <summary>
		/// The derivative of your constraint with respect to the particle's 
		/// Position.Y variable. Evaluate the derivative numerically at the particle's current position.
		/// </summary>
		public double RespectToY;

		/// <summary>
		/// The derivative of your constraint with respect to the particle's 
		/// angle variable. Evaluate the derivative numerically at the particle's current position.
		/// </summary>
		public double RespectToAngle;

		/// <summary>
		/// Scales the values of the constraint.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static ConstraintDerivative operator *(float a, ConstraintDerivative b)
		{
			return new ConstraintDerivative(a * b.RespectToX, a * b.RespectToY, a * b.RespectToAngle);
		}

		/// <summary>
		/// Adds the value of two derivatives together.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static ConstraintDerivative operator +(ConstraintDerivative a, ConstraintDerivative b)
		{
			return new ConstraintDerivative(a.RespectToX + b.RespectToX, a.RespectToY + b.RespectToY, a.RespectToAngle + b.RespectToAngle);
		}
	}
}