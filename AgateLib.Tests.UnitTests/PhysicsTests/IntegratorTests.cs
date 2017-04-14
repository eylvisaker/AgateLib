using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Physics;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AgateLib.UnitTests.PhysicsTests
{
	[TestClass]
	public class IntegratorTests
	{
		private const double tolerance = 0.05;

		private KinematicsSystem system = new KinematicsSystem();

		[TestMethod]
		public void Integrator_ImpulseTwoBodiesWithJointConstraint()
		{
			TwoBodyJointConstraint(s => new ImpulseConstraintSolver(s));
		}

		[TestMethod]
		public void Integrator_PGSTwoBodiesWithJointConstraint()
		{
			TwoBodyJointConstraint(s => new ProjectedGaussSeidelConstraintSolver(s));
		}

		[TestMethod]
		public void Integrator_ImpulseThreeBodiesWithJointConstraint()
		{
			ThreeBodyJointConstraint(s => new ImpulseConstraintSolver(s));
		}

		[TestMethod]
		public void Integrator_PGSThreeBodiesWithJointConstraint()
		{
			ThreeBodyJointConstraint(s => new ProjectedGaussSeidelConstraintSolver(s));
		}

		private void TwoBodyJointConstraint(Func<KinematicsSystem, IConstraintSolver> solver)
		{
			var a = new PhysicalParticle();
			var b = new PhysicalParticle();
			var c = new JointConstraint(a, Vector2.UnitX, b, -Vector2.UnitX);

			b.Position = 2 * Vector2.UnitX;
			b.Velocity.X = 10;

			system.AddParticles(a, b);
			system.AddConstraints(c);

			var constraintSolver = solver(system);
			var integrator = new KinematicsIntegrator(system, constraintSolver)
			{
				MaximumTimeStep = 0.01,
				MaxStepsPerFrame = 500
			};

			integrator.Integrate(1);

			Assert.AreEqual(5, a.Position.X, tolerance);
			Assert.AreEqual(7, b.Position.X, tolerance);
			Assert.AreEqual(5, a.Velocity.X, tolerance);
			Assert.AreEqual(5, b.Velocity.X, tolerance);
		}

		private void ThreeBodyJointConstraint(Func<KinematicsSystem, IConstraintSolver> solver)
		{
			var a1 = new PhysicalParticle();
			var a2 = new PhysicalParticle();
			var a3 = new PhysicalParticle();
			var c1 = new JointConstraint(a1, Vector2.UnitX, a2, -Vector2.UnitX);
			var c2 = new JointConstraint(a2, Vector2.UnitX, a3, -Vector2.UnitX);

			a2.Position = 2 * Vector2.UnitX;
			a3.Position = 4 * Vector2.UnitX;
			a2.Velocity.X = 15;

			Assert.AreEqual(0, c1.Value(new List<PhysicalParticle> { a1, a2 }));
			Assert.AreEqual(0, c2.Value(new List<PhysicalParticle> { a2, a3 }));

			system.AddParticles(a1, a2, a3);
			system.AddConstraints(c1, c2);

			var constraintSolver = solver(system);
			var integrator = new KinematicsIntegrator(system, constraintSolver)
			{
				MaximumTimeStep = 0.005,
				MaxStepsPerFrame = 1000
			};

			integrator.Integrate(1);

			Assert.AreEqual(5, a1.Position.X, tolerance);
			Assert.AreEqual(7, a2.Position.X, tolerance);
			Assert.AreEqual(9, a3.Position.X, tolerance);
			Assert.AreEqual(5, a1.Velocity.X, tolerance);
			Assert.AreEqual(5, a2.Velocity.X, tolerance);
			Assert.AreEqual(5, a3.Velocity.X, tolerance);
		}
	}
}
