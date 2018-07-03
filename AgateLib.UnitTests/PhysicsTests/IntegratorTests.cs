using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;
using AgateLib.Physics.TwoDimensions.Solvers;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using Xunit;

namespace AgateLib.Tests.PhysicsTests
{
    public class IntegratorTests
    {
        private const float tolerance = 0.05f;

        private KinematicsSystem system = new KinematicsSystem();

        [Fact]
        public void Integrator_ImpulseTwoBodiesWithJointConstraint()
        {
            TwoBodyJointConstraint(s => new ImpulseConstraintSolver(s));
        }

        [Fact]
        public void Integrator_PGSTwoBodiesWithJointConstraint()
        {
            TwoBodyJointConstraint(s => new ProjectedGaussSeidelConstraintSolver(s));
        }

        [Fact]
        public void Integrator_ImpulseThreeBodiesWithJointConstraint()
        {
            ThreeBodyJointConstraint(s => new ImpulseConstraintSolver(s));
        }

        [Fact]
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
                MaximumTimeStep = 0.01f,
                MaxStepsPerFrame = 500
            };

            integrator.Integrate(1);

            a.Position.X.Should().BeApproximately(5, tolerance);
            b.Position.X.Should().BeApproximately(7, tolerance);
            a.Velocity.X.Should().BeApproximately(5, tolerance);
            b.Velocity.X.Should().BeApproximately(5, tolerance);
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

            c1.Value(new List<PhysicalParticle> { a1, a2 }).Should().Be(0);
            c2.Value(new List<PhysicalParticle> { a2, a3 }).Should().Be(0);

            system.AddParticles(a1, a2, a3);
            system.AddConstraints(c1, c2);

            var constraintSolver = solver(system);
            var integrator = new KinematicsIntegrator(system, constraintSolver)
            {
                MaximumTimeStep = 0.005f,
                MaxStepsPerFrame = 1000
            };

            integrator.Integrate(1);

            a1.Position.X.Should().BeApproximately(5, tolerance);
            a2.Position.X.Should().BeApproximately(7, tolerance);
            a3.Position.X.Should().BeApproximately(9, tolerance);
            a1.Velocity.X.Should().BeApproximately(5, tolerance);
            a2.Velocity.X.Should().BeApproximately(5, tolerance);
            a3.Velocity.X.Should().BeApproximately(5, tolerance);
        }
    }
}
