using System;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Quality;

namespace AgateLib.Physics.TwoDimensions
{
	public class KinematicsIntegrator
	{
		private readonly IConstraintSolver constraint;

		private double unusedTime;

		private double minimumTimeStep = 0.0001;
		private double maximumTimeStep = 0.005;
		private int maxStepsPerFrame = 50;

		public KinematicsIntegrator(KinematicsSystem system, IConstraintSolver constraint)
		{
			System = system;

			this.constraint = constraint;
		}

		/// <summary>
		/// Event raised after all non-friction forces have been computed.
		/// </summary>
		public event EventHandler ForcesComputed;

		/// <summary>
		/// The system of particles, forces and constraints to use.
		/// </summary>
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
		public double MinimumTimeStep
		{
			get { return minimumTimeStep; }
			set
			{
				Require.ArgumentInRange(value > 0, nameof(MinimumTimeStep), "Minimum time step must be positive.");
				minimumTimeStep = value;
			}
		}

		/// <summary>
		/// Sets the maximum time step for the kinematics update. If the elapsed
		/// time is greater than this, up to MaxStepsPerFrame steps in the physics
		/// simulation are done.
		/// </summary>
		public double MaximumTimeStep
		{
			get { return maximumTimeStep; }
			set
			{
				Require.ArgumentInRange(value > 0, nameof(MaximumTimeStep), "Maximum time step must be positive.");
				maximumTimeStep = value;
			}
		}

		/// <summary>
		/// Maximum number of physics steps per update.
		/// </summary>
		public int MaxStepsPerFrame
		{
			get { return maxStepsPerFrame; }
			set
			{
				Require.ArgumentInRange(value > 0, nameof(MaxStepsPerFrame), "Max steps per frame must be positive.");
				maxStepsPerFrame = value;
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
		public void Integrate(double dt)
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

		private void UpdateStep(double dt)
		{
			ComputeForces();

			constraint.ComputeConstraintForces(dt);
			constraint.ApplyConstraintForces();

			constraint.IntegrateKinematicVariables(dt);

			StepCount++;
		}

		private void ComputeForces()
		{
			Parallel.ForEach(System.Particles, particle => particle.Force = Vector2.Zero);

			foreach (var force in System.Forces)
			{
				force.AccumulateForce(System.Particles);
			}
		}

	}
}
