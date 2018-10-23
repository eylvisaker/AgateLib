//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System.Collections.Generic;

namespace AgateLib.Physics.TwoDimensions
{
	/// <summary>
	/// A constraint equation for the kinematics solver to incorporate in the computation of forces.
	/// </summary>
	public interface IPhysicalConstraint
	{
		float MultiplierMin { get; }
		float MultiplierMax { get; }

		/// <summary>
		/// Return true to have the constraint applied on ly if its value is positive.
		/// </summary>
		ConstraintType ConstraintType { get; }

		/// <summary>
		/// Returns the current value of the constraint equation.
		/// </summary>
		float Value(IReadOnlyList<PhysicalParticle> particles);

		/// <summary>
		/// Returns true if the constraint applies to the specified object.
		/// </summary>
		/// <param name="particle"></param>
		/// <returns></returns>
		bool AppliesTo(PhysicalParticle particle);

		/// <summary>
		/// Compute the derivative of your constraint with respect to each coordinate of the particle object.
		/// </summary>
		/// <param name="particle"></param>
		/// <returns></returns>
		ConstraintDerivative Derivative(PhysicalParticle particle);

		IEnumerable<IReadOnlyList<PhysicalParticle>> ApplyTo(KinematicsSystem system);

	}

	public enum ConstraintType
	{
		Equality,
		Inequality,
	}
}