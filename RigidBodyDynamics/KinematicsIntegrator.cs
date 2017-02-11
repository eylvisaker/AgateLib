using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RigidBodyDynamics
{
	public class KinematicsIntegrator
	{
		private ConstraintSolver constraint;

		private float unusedTime;

		private float minimumTimeStep = 0.001f;
		private float maximumTimeStep = 0.01f;

		public KinematicsIntegrator(KinematicsSystem system, ConstraintSolver constraint)
		{
			System = system;

			this.constraint = constraint;
		}

		public KinematicsSystem System { get; }

		/// <summary>
		/// Sets the minimum amount of time before a time step is executed.
		/// This is to prevent division by dt from blowing up. If dt is
		/// smaller than MinimumTimeStep, that time will be accumulated and the
		/// dynamics update will be delayed until the minimum time has passed.
		/// </summary>
		public TimeSpan MinimumTimeStep
		{
			get { return TimeSpan.FromSeconds(minimumTimeStep); }
			set { minimumTimeStep = (float)value.TotalSeconds; }
		}

		public TimeSpan MaximumTimeStep
		{
			get { return TimeSpan.FromDays(maximumTimeStep); }
			set { maximumTimeStep = (float)value.TotalSeconds; }
		}

		public void Update(TimeSpan elapsed)
		{
			float dt = (float)elapsed.TotalSeconds;

			unusedTime += dt;

			if (unusedTime < minimumTimeStep)
				return;

			if (unusedTime > maximumTimeStep)
			{
				if (unusedTime > maximumTimeStep * 5)
					unusedTime = maximumTimeStep * 5;

				int steps = (int)Math.Ceiling(unusedTime / maximumTimeStep);

				for (int i = 0; i < steps; i++)
				{
					UpdateStep(unusedTime / steps);
				}
			}
			else
			{
				UpdateStep(unusedTime);
			}

			unusedTime = 0;

		}

		private void UpdateStep(float dt)
		{
			constraint.Update(dt);

			IntegrateKinematicVariables(dt);
		}


		private void IntegrateKinematicVariables(float dt)
		{
			foreach (var item in System.Particles)
			{
				item.AngularVelocity += dt * item.Torque / item.InertialMoment;
				item.Angle += dt * item.AngularVelocity;

				item.Velocity += dt * item.Force / item.Mass;
				item.Position += dt * item.Velocity;
			}
		}
	}
}
