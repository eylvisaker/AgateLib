using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RigidBodyDynamics;

namespace AgateLib.Physics.UnitTests.ConstraintTests
{
	[TestClass]
	public class PointTouchConstraintTests
	{
		private PhysicalParticle particle1 = new PhysicalParticle();
		private PhysicalParticle particle2 = new PhysicalParticle();

		Vector2 point1 = new Vector2(10, 4);
		Vector2 point2 = new Vector2(-10, -4);

		[TestInitialize]
		public void Initialize()
		{
			particle1.Velocity = new Vector2(10, 15);
			particle2.Velocity = new Vector2(-2, -6);

			// Initialize the particle to satisfy the constraint.
			particle2.Position = new Vector2(20, 8);
		}

		[TestMethod]
		public void PointTouchConstraintSatisfiedValue()
		{
			var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

			Assert.AreEqual(0, pointTouchConstraint.Value, 0.000001);
		}
		
		[TestMethod]
		public void PointTouchConstraintSatisfiedDerivative()
		{
			var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

			var derivative = pointTouchConstraint.Derivative(particle1);

			Assert.AreEqual(0, derivative.RespectToX, 0.000001);
			Assert.AreEqual(0, derivative.RespectToY, 0.000001);
			Assert.AreEqual(0, derivative.RespectToAngle, 0.000001);

			derivative = pointTouchConstraint.Derivative(particle2);

			Assert.AreEqual(0, derivative.RespectToX, 0.000001);
			Assert.AreEqual(0, derivative.RespectToY, 0.000001);
			Assert.AreEqual(0, derivative.RespectToAngle, 0.000001);
		}

		[TestMethod]
		public void PointTouchConstraintSatisfiedMixedPartialDerivative()
		{
			var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

			var derivative = pointTouchConstraint.MixedPartialDerivative(particle1);

			Assert.AreEqual(12, derivative.RespectToX, 0.000001);
			Assert.AreEqual(21, derivative.RespectToY, 0.000001);
			Assert.AreEqual(-162, derivative.RespectToAngle, 0.000001);

			derivative = pointTouchConstraint.MixedPartialDerivative(particle2);

			Assert.AreEqual(-12, derivative.RespectToX, 0.000001);
			Assert.AreEqual(-21, derivative.RespectToY, 0.000001);
			Assert.AreEqual(-162, derivative.RespectToAngle, 0.000001);
		}

		[TestMethod]
		public void PointTouchConstraintUnsatisfiedValue()
		{
			particle2.Position = new Vector2(20, 0);

			var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

			Assert.AreEqual(32, pointTouchConstraint.Value, 0.000001);
		}

		[TestMethod]
		public void PointTouchConstraintUnsatisfiedDerivative()
		{
			particle2.Position = new Vector2(20, 0);

			var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

			var derivative = pointTouchConstraint.Derivative(particle1);

			Assert.AreEqual(0, derivative.RespectToX, 0.000001);
			Assert.AreEqual(8, derivative.RespectToY, 0.000001);
			Assert.AreEqual(-80, derivative.RespectToAngle, 0.000001);

			derivative = pointTouchConstraint.Derivative(particle2);

			Assert.AreEqual(0, derivative.RespectToX, 0.000001);
			Assert.AreEqual(-8, derivative.RespectToY, 0.000001);
			Assert.AreEqual(-80, derivative.RespectToAngle, 0.000001);
		}


		[TestMethod]
		public void PointTouchConstraintUnsatisfiedMixedPartialDerivative()
		{
			particle2.Position = new Vector2(20, 0);

			var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

			var derivative = pointTouchConstraint.MixedPartialDerivative(particle1);

			Assert.AreEqual(12, derivative.RespectToX, 0.000001);
			Assert.AreEqual(21, derivative.RespectToY, 0.000001);
			Assert.AreEqual(-162, derivative.RespectToAngle, 0.000001);

			derivative = pointTouchConstraint.MixedPartialDerivative(particle2);

			Assert.AreEqual(-12, derivative.RespectToX, 0.000001);
			Assert.AreEqual(-21, derivative.RespectToY, 0.000001);
			Assert.AreEqual(-162, derivative.RespectToAngle, 0.000001);
		}

	}
}
