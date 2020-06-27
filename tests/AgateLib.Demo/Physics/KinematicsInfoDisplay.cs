using AgateLib.Display;
using AgateLib.Mathematics;
using AgateLib.Physics.TwoDimensions;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Demo.Physics
{
    public class KinematicsInfoDisplay
    {
        private readonly IKinematicsExample example;
        private readonly IConstraintSolver constraintSolver;
        private readonly KinematicsSystem system;
        private readonly KinematicsHistory history;
        private readonly Font font;

        private int debugParticleIndex;

        public KinematicsInfoDisplay(IKinematicsExample example,
                                     IConstraintSolver constraintSolver,
                                     KinematicsSystem system,
                                     KinematicsHistory history,
                                     IFontProvider fonts)
        {
            this.example = example;
            this.constraintSolver = constraintSolver;
            this.system = system;
            this.history = history; 
            this.font = new Font(fonts.Default, 16, FontStyles.Bold);
        }

        public int DebugInfoPage { get; set; }

        private IReadOnlyList<PhysicalParticle> particles => system.Particles;
        private IReadOnlyList<IPhysicalConstraint> constraints => system.Constraints;

        public void Draw(Canvas canvas)
        {
            canvas.DrawText(font, Vector2.Zero, DebugInfo());
        }

        private string DebugInfo()
        {
            StringBuilder b = new StringBuilder();

            b.AppendLine($"Physics Step: {history.Index}");
            b.AppendLine($"Particle {debugParticleIndex}");

            PrintStateOfBox(b, particles[debugParticleIndex]);

            PrintEnergyState(b);

            return b.ToString();
        }

        private void PrintEnergyState(StringBuilder b)
        {
            if (DebugInfoPage == 0)
            {
                b.AppendLine($"\n  KE: {system.LinearKineticEnergy}");
                b.AppendLine($" RKE: {system.AngularKineticEnergy}");
                b.AppendLine($"  PE: {example.PotentialEnergy}");
                b.AppendLine($"   E: {example.PotentialEnergy + system.LinearKineticEnergy + system.AngularKineticEnergy}");
            }
        }

        private void PrintStateOfBox(StringBuilder b, PhysicalParticle box)
        {
            const float RadsToDegress = 180 / (float)Math.PI;

            b.AppendLine($"   X: {box.Position.X}");
            b.AppendLine($"   Y: {box.Position.Y}");
            b.AppendLine($"   A: {box.Angle}\n");

            if (DebugInfoPage == 0)
            {
                b.AppendLine($" v_x: {box.Velocity.X}");
                b.AppendLine($" v_y: {box.Velocity.Y}");
                b.AppendLine($"   w: {box.AngularVelocity}");

                b.AppendLine($"\n F_x: {box.Force.X}");
                b.AppendLine($" F_y: {box.Force.Y}");
                b.AppendLine($"   T: {box.Torque}");

                b.AppendLine($"\nFC_x: {box.ConstraintForce.X}");
                b.AppendLine($"FC_y: {box.ConstraintForce.Y}");
                b.AppendLine($"  TC: {box.ConstraintTorque}");

                var angle = RadsToDegress * Vector2.Dot(box.ConstraintForce, box.Velocity) /
                            (box.ConstraintForce.Length() * box.Velocity.Length());

                b.AppendLine($"\n DOT: {angle}");
            }
            else if (DebugInfoPage == 1)
            {
                constraintSolver.DebugInfo(b, DebugInfoPage, box);
            }
        }

    }
}
