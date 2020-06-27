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
    public class ParticleOnCircleOffCenterExample : IKinematicsExample
    {
        private const float gravity = 1000f;
        private int boxSize;

        private KinematicsSystem system;

        private Texture2D boxImage;

        private Vector2 circlePosition;
        private float circleRadius;

        private PhysicalParticle Box => system.Particles.First();

        public string Name => "Particle corner on a circle";

        public double PotentialEnergy => system.Particles.Sum(p => p.Mass * p.Position.Y * -gravity);

        public KinematicsSystem Initialize(Size area, IContentProvider content)
        {
            InitializeImages(content);

            circleRadius = area.Height * 0.375f;
            circlePosition = new Vector2(area.Width * .5f, area.Height * .375f);

            var particlePosition = circlePosition
                                   + Vector2X.FromPolarDegrees(circleRadius, -60)
                                   + new Vector2(boxSize * .5f, boxSize * .5f)
                ;//+ new Vector2(0, boxSize * .5f);

            system = new KinematicsSystem();

            system.AddParticles(new PhysicalParticle
            {
                Position = particlePosition,
                //Angle = -(float)Math.PI / 4,
                InertialMoment = boxSize * boxSize / 12f,
            });

            system.AddConstraints(new CirclePerimeterOffcenterConstraint(
                system.Particles[0], circlePosition, circleRadius, new Vector2(-boxSize * .5f, -boxSize * .5f)));

            system.AddForceField(new ConstantGravityField());

            return system;
        }

        public void ComputeExternalForces()
        {
            // Gravity force
            Box.Force = new Vector2(0, gravity * Box.Mass);
        }

        public void Draw(ICanvas canvas)
        {
            var box = system.Particles.First();

            var boxScale = boxSize / (float)boxImage.Width;

            canvas.Draw(boxImage,
                box.Position,
                null,
                Color.White,
                -box.Angle,
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

            canvas.DrawLines(LineType.Polygon, Color.Red, ellipsePoints.Points);
        }

        private void InitializeImages(IContentProvider content)
        {
            boxImage = content.Load<Texture2D>("bg-bricks");

            boxSize = 40;
        }

        public void AddParticle()
        {
        }

        public void RemoveParticle()
        {
        }
    }

    public class ParticleCornerOnCircleDemo : KinematicsDemo
    {
        public ParticleCornerOnCircleDemo()
            : base(new ParticleOnCircleOffCenterExample())
        {
        }
    }
}