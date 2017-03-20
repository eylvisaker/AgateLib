using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Physics;
using AgateLib.Physics.Constraints;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AgateLib.UnitTests.PhysicsTests
{
	[TestClass]
	public class IntegratorTests
	{
		KinematicsSystem system = new KinematicsSystem();
		
		[TestMethod]
		public void KinematicsWithJointConstraint()
		{
			var a = new PhysicalParticle();
			var b = new PhysicalParticle();
			var c = new JointConstraint(a, Vector2.UnitX, b, -Vector2.UnitX);

			b.Position = 2 * Vector2.UnitX;
			b.Velocity.X = 10;

			system.AddParticles(a, b);
			system.AddConstraints(c);

			var constraintSolver = new ImpulseConstraintSolver(system);
			var integrator = new KinematicsIntegrator(system, constraintSolver)
			{
				MaximumTimeStep = 0.01,
				MaxStepsPerFrame = 500
			};

			integrator.Integrate(1);

			Assert.AreEqual(5, a.Position.X, 0.1);
			Assert.AreEqual(7, b.Position.X, 0.1);
			Assert.AreEqual(5, a.Velocity.X, 0.1);
			Assert.AreEqual(5, b.Velocity.X, 0.1);
		}
	}
}
