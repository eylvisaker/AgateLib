using AgateLib.Display;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;
using AgateLib.Physics.TwoDimensions.Forces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Demo.Physics
{
    public class ChainOnCircleExample : IKinematicsExample
    {
        private int boxSize = 40;
        private const float gravity = 1000f;

        private KinematicsSystem system;

        private Texture2D boxImage;

        private Vector2 circlePosition;
        private float circleRadius;

        public string Name => "Chain on a circle";

        public double PotentialEnergy => system.Particles.Sum(p => p.Mass * p.Position.Y * -gravity);

        public int BoxCount { get; set; } = 2;

        public KinematicsSystem Initialize(Size area, IContentProvider content)
        {
            InitializeImages(content);

            circleRadius = area.Height * 0.3f;
            circlePosition = new Vector2(area.Width * .5f, area.Height * .3f);

            var particlePosition = circlePosition + Vector2X.FromPolarDegrees(circleRadius, -90)
                                   + new Vector2(0, boxSize * .5f);

            system = new KinematicsSystem();

            GeometryBuilder.CreateChain(system, BoxCount, boxSize, particlePosition);

            system.AddConstraints(new CirclePerimeterOffcenterConstraint(
                system.Particles.First(), circlePosition, circleRadius, new Vector2(-boxSize * .5f, -boxSize * .5f)));

            system.AddForceField(new ConstantGravityField());

            return system;
        }

        public void ComputeExternalForces()
        {
            foreach (var box in system.Particles)
            {
                // Gravity force
                box.Force = new Vector2(0, gravity * box.Mass);
            }
        }

        public void Draw(ICanvas canvas)
        {
            foreach (var box in system.Particles)
            {
                var boxScale = boxSize / (float)boxImage.Width;

                canvas.Draw(boxImage,
                    box.Position,
                    null,
                    Color.White,
                    -box.Rotation,
                    new Vector2(boxImage.Width / 2, boxImage.Height / 2),
                    boxScale,
                    SpriteEffects.None,
                    0);

                var ellipsePoints = new EllipseBuilder().BuildEllipse(
                    (Rectangle)RectangleF.FromLTRB(
                        (float)circlePosition.X - circleRadius,
                        (float)circlePosition.Y - circleRadius,
                        (float)circlePosition.X + circleRadius,
                        (float)circlePosition.Y + circleRadius));

                canvas.DrawLines(LineType.Polygon, Color.Blue, ellipsePoints.Points);
            }
        }

        private void InitializeImages(IContentProvider content)
        {
            boxImage = content.Load<Texture2D>("bg-bricks");

            boxSize = 40;
        }

        public void AddParticle()
        {
            BoxCount++;
        }

        public void RemoveParticle()
        {
            BoxCount = Math.Max(2, BoxCount - 1);
        }
    }

    public class ChainOnCircleDemo : PhysicsDemo
    {
        public ChainOnCircleDemo()
            : base(new ChainOnCircleExample())
        { }
    }
}
