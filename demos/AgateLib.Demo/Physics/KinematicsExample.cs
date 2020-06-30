using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.Physics.TwoDimensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Demo.Physics
{
    public interface IKinematicsExample
    {
        string Name { get; }
        double PotentialEnergy { get; }

        KinematicsSystem Initialize(Size area, IContentProvider content);

        void Draw(ICanvas canvas);

        void ComputeExternalForces();

        void AddParticle();
        void RemoveParticle();
    }
}
