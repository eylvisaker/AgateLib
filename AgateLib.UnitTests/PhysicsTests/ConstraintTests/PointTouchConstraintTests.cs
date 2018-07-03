using AgateLib.Mathematics;
using AgateLib.Physics;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.PhysicsTests.ConstraintTests
{
    public class PointTouchConstraintTests
    {
        float tolerance = 0.000001f;

        private PhysicalParticle particle1 = new PhysicalParticle();
        private PhysicalParticle particle2 = new PhysicalParticle();

        Vector2 point1 = new Vector2(10, 4);
        Vector2 point2 = new Vector2(-10, -4);

        public PointTouchConstraintTests()
        {
            particle1.Velocity = new Vector2(10, 15);
            particle2.Velocity = new Vector2(-2, -6);

            // Initialize the particle to satisfy the constraint.
            particle2.Position = new Vector2(20, 8);
        }

        [Fact]
        public void PointTouchConstraintSatisfiedValue()
        {
            var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

            pointTouchConstraint.Value(new[] { particle1, particle2 }).Should().BeApproximately(0, tolerance);
        }

        [Fact]
        public void PointTouchConstraintSatisfiedDerivative()
        {
            var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

            var derivative = pointTouchConstraint.Derivative(particle1);

            derivative.RespectToX.Should().BeApproximately(0, tolerance);
            derivative.RespectToY.Should().BeApproximately(0, tolerance);
            derivative.RespectToAngle.Should().BeApproximately(0, tolerance);

            derivative = pointTouchConstraint.Derivative(particle2);

            derivative.RespectToX.Should().BeApproximately(0, tolerance);
            derivative.RespectToY.Should().BeApproximately(0, tolerance);
            derivative.RespectToAngle.Should().BeApproximately(0, tolerance);
        }

        [Fact]
        public void PointTouchConstraintUnsatisfiedValue()
        {
            particle2.Position = new Vector2(20, 0);

            var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

            pointTouchConstraint.Value(new[] { particle1, particle2 }).Should().BeApproximately(32, tolerance);
        }

        [Fact]
        public void PointTouchConstraintUnsatisfiedDerivative()
        {
            particle2.Position = new Vector2(20, 0);

            var pointTouchConstraint = new JointConstraint(particle1, point1, particle2, point2);

            var derivative = pointTouchConstraint.Derivative(particle1);

            derivative.RespectToX.Should().BeApproximately(0, tolerance);
            derivative.RespectToY.Should().BeApproximately(8, tolerance);
            derivative.RespectToAngle.Should().BeApproximately(-80, tolerance);

            derivative = pointTouchConstraint.Derivative(particle2);

            derivative.RespectToX.Should().BeApproximately(0, tolerance);
            derivative.RespectToY.Should().BeApproximately(-8, tolerance);
            derivative.RespectToAngle.Should().BeApproximately(-80, tolerance);
        }
    }
}
