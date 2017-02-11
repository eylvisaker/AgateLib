using System;
using System.Collections.Generic;

namespace RigidBodyDynamics
{
	internal class KinematicsSolver
	{
		private List<Physical> PhysicalObjects = new List<Physical>();
		private List<PhysicalConstraint> Constraints = new List<PhysicalConstraint>();

		public void Update(TimeSpan elapsed)
		{
			var dt = elapsed.TotalSeconds;

			foreach (var item in PhysicalObjects)
			{
				item.Angle += item.AngularVelocity * dt;

				item.Velocity += dt * item.Force / item.Mass;
				item.Position += dt * item.Velocity;
			}
		}

		public void AddObjects(params Physical[] items)
		{
			PhysicalObjects.AddRange(items);
		}

		public void AddConstraints(List<PhysicalConstraint> constraints)
		{
			Constraints.AddRange(constraints);
		}
	}
}