using AgateLib.Display;
using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.Geometry.Builders;
using AgateLib.Physics.TwoDimensions;
using AgateLib.Physics.TwoDimensions.Constraints;
using MathNet.Numerics.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Demo.Physics
{
    public class ParticleOnCircleExample : IKinematicsExample
    {
        private int boxSize = 40;
        private const float gravity = 1000f;

        private KinematicsSystem system;

        private Texture2D boxImage;

        private Vector2 circlePosition;
        private float circleRadius;

        private PhysicalParticle Box => system.Particles.First();

        public string Name => "Particle on a circle";

        public double PotentialEnergy => system.Particles.Sum(p => p.Mass * p.Position.Y * -gravity);

        public KinematicsSystem Initialize(Size area, IContentProvider content)
        {
            InitializeImages(content);

            circleRadius = area.Height * 0.375f;
            circlePosition = new Vector2(area.Width * 0.5f, area.Height * 0.5f);

            var particlePosition = circlePosition + Vector2X.FromPolarDegrees(circleRadius, -60);

            system = new KinematicsSystem();

            system.AddParticles(new PhysicalParticle
            {
                Position = particlePosition,
                Angle = 1.5f,
                AngularVelocity = 20f,
            });

            system.AddConstraints(new CirclePerimeterConstraint(system.Particles[0], circlePosition, circleRadius));

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
        }

        public void AddParticle()
        {
        }

        public void RemoveParticle()
        {
        }
    }

    public class ParticleOnCircleDemo : KinematicsDemo
    {
        public ParticleOnCircleDemo() 
            : base(new ParticleOnCircleExample())
        {
        }
    }
}