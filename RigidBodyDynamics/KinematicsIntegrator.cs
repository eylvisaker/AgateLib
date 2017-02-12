using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;

namespace RigidBodyDynamics
{
	public class KinematicsIntegrator
	{
		private readonly ConstraintSolver constraint;

		private float unusedTime;

		private float minimumTimeStep = 0.0001f;
		private float maximumTimeStep = 0.005f;
		private int maxStepsPerFrame = 1;

		public KinematicsIntegrator(KinematicsSystem system, ConstraintSolver constraint)
		{
			System = system;

			this.constraint = constraint;
		}

		public KinematicsSystem System { get; }

		/// <summary>
		/// The number of simulation steps the integrator has run.
		/// </summary>
		public int StepCount { get; private set; }

		/// <summary>
		/// Sets the minimum amount of time before a time step is executed.
		/// This is to prevent division by dt from blowing up. If dt is
		/// smaller than MinimumTimeStep, that time will be accumulated and the
		/// dynamics update will be delayed until the minimum time has passed.
		/// </summary>
		public TimeSpan MinimumTimeStep
		{
			get { return TimeSpan.FromSeconds(minimumTimeStep); }
			set
			{
				Require.ArgumentInRange(value.TotalSeconds > 0, nameof(MinimumTimeStep), "Minimum time step must be positive.");
				minimumTimeStep = (float)value.TotalSeconds;
			}
		}

		/// <summary>
		/// Sets the maximum time step for the kinematics update. If the elapsed
		/// time is greater than this, up to MaxStepsPerFrame steps in the physics
		/// simulation are done.
		/// </summary>
		public TimeSpan MaximumTimeStep
		{
			get { return TimeSpan.FromDays(maximumTimeStep); }
			set
			{
				Require.ArgumentInRange(value.TotalSeconds > 0, nameof(MinimumTimeStep), "Minimum time step must be positive.");
				maximumTimeStep = (float)value.TotalSeconds;
			}
		}

		public int MaxStepsPerFrame
		{
			get { return maxStepsPerFrame; }
			set
			{
				Require.ArgumentInRange(value > 0, nameof(MinimumTimeStep), "Minimum time step must be positive.");
				maximumTimeStep = value;
			}
		}

		public void Integrate(TimeSpan elapsed)
		{
			float dt = (float)elapsed.TotalSeconds;

			Integrate(dt);
		}

		/// <summary>
		/// Updates the dynamics.
		/// </summary>
		/// <param name="dt">The amount of time in seconds that passed.</param>
		public void Integrate(float dt)
		{
			unusedTime += dt;

			if (unusedTime < minimumTimeStep)
				return;

			if (unusedTime > maximumTimeStep)
			{
				if (unusedTime > maximumTimeStep * MaxStepsPerFrame)
					unusedTime = maximumTimeStep * MaxStepsPerFrame;

				int steps = (int) Math.Ceiling(unusedTime / maximumTimeStep);

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
			constraint.Update();
			constraint.ApplyConstraintForces();

			IntegrateKinematicVariables(dt);

			StepCount++;
		}


		private void IntegrateKinematicVariables(float dt)
		{
			foreach (var item in System.Particles)
			{
				item.AngularVelocity += dt * (item.Torque + item.ConstraintTorque) / item.InertialMoment;
				item.Angle += dt * item.AngularVelocity;

				item.Velocity += dt * (item.Force + item.ConstraintForce) / item.Mass;
				item.Position += dt * item.Velocity;
			}
		}
	}
}
