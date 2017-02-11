using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RigidBodyDynamics
{
	public class KinematicsIntegrator
	{
		private ConstraintSolver constraint;

		public KinematicsIntegrator(KinematicsSystem system, ConstraintSolver constraint)
		{
			System = system;

			this.constraint = constraint;
		}

		public KinematicsSystem System { get; }

		public void Update(TimeSpan elapsed)
		{
			float dt = (float)elapsed.TotalSeconds;

			constraint.Update(dt);

			IntegrateKinematicVariables(dt);
		}


		private void IntegrateKinematicVariables(float dt)
		{
			foreach (var item in System.Particles)
			{
				item.Angle += item.AngularVelocity * dt;

				item.Velocity += dt * item.Force / item.Mass;
				item.Position += dt * item.Velocity;
			}
		}
	}
}
