using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AgateLib.Mathematics;
using MathNet.Numerics.LinearAlgebra;

namespace AgateLib.Physics.TwoDimensions
{
	/// <summary>
	/// Impulse based constraint solver.
	/// </summary>
	/// <remarks>
	/// Algorithm from 
	/// https://www.toptal.com/game/video-game-physics-part-iii-constrained-rigid-body-simulation
	/// </remarks>
	public class ImpulseConstraintSolver : IConstraintSolver
	{
		/// <summary>
		/// This is three because each particle has three coordinates: X, Y, and rotation angle.
		/// </summary>
		private const int GeneralizedCoordinatesPerParticle = 3;

		private const double DefaultSpringConstant = 50;

		private Matrix<double> externalForces;

		private Matrix<double> savedJacobian;
		private double savedLagrangeMultiplier;
		private Matrix<double> constraintForces;

		private Dictionary<PhysicalParticle, Vector3> newVelocities = new Dictionary<PhysicalParticle, Vector3>();

		/// <summary>
		/// Constructs a impulse based constraint colver.
		/// </summary>
		/// <param name="system"></param>
		public ImpulseConstraintSolver(KinematicsSystem system)
		{
			System = system;
		}

		/// <summary>
		/// The number of generalized coordinates.
		/// </summary>
		private int GeneralizedCoordinateCount => Particles.Count * GeneralizedCoordinatesPerParticle;

		private IReadOnlyList<PhysicalParticle> Particles => System.Particles;

		private IReadOnlyList<IPhysicalConstraint> Constraints => System.Constraints;

		/// <summary>
		/// The system containing all the particles that can interact.
		/// </summary>
		public KinematicsSystem System { get; set; }

		public double SpringConstant { get; set; } = DefaultSpringConstant;

		public double DampeningConstant { get; set; } = (float)Math.Sqrt(DefaultSpringConstant);

		private double dt;

		/// <summary>
		/// Computes the constraint forces from the current state of the system.
		/// </summary>
		public void ComputeConstraintForces(double dt)
		{
			this.dt = dt;

			InitializeStep(dt);

			SolveConstraintEquations(dt);
		}

		public void ApplyConstraintForces()
		{
			for (int i = 0; i < Particles.Count; i++)
			{
				var particle = Particles[i];

				var newV = new Vector2(newVelocities[particle].X,
									   newVelocities[particle].Y);

				particle.ConstraintForce = (newV - particle.Velocity) / dt;
				particle.ConstraintTorque = (newVelocities[particle].Z - particle.AngularVelocity) / dt;

				particle.Velocity = newV;
				particle.AngularVelocity = newVelocities[particle].Z;
			}
		}

		private Matrix<double> ComputeJacobian(IPhysicalConstraint constraint, IReadOnlyList<PhysicalParticle> particles)
		{
			Matrix<double> jacobian = Matrix<double>.Build.Dense(1, GeneralizedCoordinatesPerParticle * particles.Count);

			for (int j = 0; j < particles.Count; j++)
			{
				if (!constraint.AppliesTo(particles[j]))
					continue;

				ConstraintDerivative derivative = constraint.Derivative(particles[j]);

				int basis = j * GeneralizedCoordinatesPerParticle;

				jacobian[0, basis + 0] = derivative.RespectToX;
				jacobian[0, basis + 1] = derivative.RespectToY;
				jacobian[0, basis + 2] = derivative.RespectToAngle;
			}

			return jacobian;
		}

		private void SolveConstraintEquations(double dt)
		{
			const double tolerance = 1e-6;
			const int maxIterations = 100;
			const double errorMax = 1e-4;

			//Debug.WriteLine($"**************************** Starting iterations");

			double totalError = 0;
			int nIter;

			for (nIter = 0; nIter < maxIterations; nIter++)
			{
				//Debug.WriteLine($"\n**** Iteration {nIter}");
				totalError = 0;


				// Here's the equation:
				//    J * M^(-1) * J^T * lambda = -J * v - Bias
				// Lambda (the Lagrange parameter) is the set of unknowns. 
				// This is just a straightfoward system of linear equations of the form
				//    A * x = B
				// where A = J * M^(-1) * J^T 
				//       x = lambda
				//       B = -J * v - Bias
				var velocityStep = newVelocities.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

				foreach (var constraint in Constraints)
				{
					foreach (var particleGroup in constraint.ApplyTo(System))
					{
						var jacobian = ComputeJacobian(constraint, particleGroup);
						var constraintValue = constraint.Value(particleGroup);

						savedJacobian = jacobian;

						var newVelocity = Matrix<double>.Build.Dense(
							particleGroup.Count * GeneralizedCoordinatesPerParticle, 1);
						var massInverseMatrix = Matrix<double>.Build.Dense(
							particleGroup.Count * GeneralizedCoordinatesPerParticle,
							particleGroup.Count * GeneralizedCoordinatesPerParticle);

						for (int i = 0; i < particleGroup.Count; i++)
						{
							int basis = i * GeneralizedCoordinatesPerParticle;

							newVelocity[basis + 0, 0] = velocityStep[particleGroup[i]].X;
							newVelocity[basis + 1, 0] = velocityStep[particleGroup[i]].Y;
							newVelocity[basis + 2, 0] = velocityStep[particleGroup[i]].Z;

							massInverseMatrix[basis + 0, basis + 0] = 1 / particleGroup[i].Mass;
							massInverseMatrix[basis + 1, basis + 1] = 1 / particleGroup[i].Mass;
							massInverseMatrix[basis + 2, basis + 2] = 1 / particleGroup[i].InertialMoment;
						}

						double A = (jacobian * massInverseMatrix * jacobian.Transpose())[0, 0];
						double B1 = (-jacobian * newVelocity)[0, 0];

						// Add bias to help force constraint apply.
						//var bias = Matrix<double>.Build.Dense(particles.Count * GeneralizedCoordinatesPerParticle, 1);

						var bias = SpringConstant * constraintValue + (DampeningConstant * B1) * dt;
						var B = B1 - bias;

						if (A < tolerance)
							continue;

						var lagrangeMultiplier = B / A;
						savedLagrangeMultiplier = lagrangeMultiplier;

						var impulse = jacobian.Transpose() * lagrangeMultiplier;

						if (lagrangeMultiplier > 0 && constraint.ConstraintType == ConstraintType.Inequality)
							continue;

						//Debug.WriteLine($"  Constraint {j}");
						for (int i = 0; i < particleGroup.Count; i++)
						{
							int basis = i * GeneralizedCoordinatesPerParticle;
							var particle = particleGroup[i];

							Vector3 nv = newVelocities[particle];

							nv.X += impulse[basis + 0, 0] / particle.Mass;
							nv.Y += impulse[basis + 1, 0] / particle.Mass;
							nv.Z += impulse[basis + 2, 0] / particle.InertialMoment;

							Vector3 error = nv - newVelocities[particle];
							totalError += error.MagnitudeSquared;

							newVelocities[particle] = nv;

							//Debug.WriteLine($"    Particle {i}: dV: {error.Magnitude}");
						}
					}
				}

				if (totalError < errorMax)
					break;
			}

			Debug.WriteLine($"Total error: {totalError} after {nIter + 1} iterations.");
		}

		private void InitializeStep(double dt)
		{
			InitializeVelocityVector(dt);
		}

		private void InitializeVelocityVector(double dt)
		{
			for (int i = 0; i < Particles.Count; i++)
			{
				var particle = Particles[i];
				var velocity = particle.Velocity;
				var angularVelocity = particle.AngularVelocity;

				velocity += dt * particle.Force / particle.Mass;
				angularVelocity += dt * particle.Torque / particle.InertialMoment;

				newVelocities[particle] = new Vector3(velocity.X, velocity.Y, angularVelocity);
			}
		}

		public void DebugInfo(StringBuilder b, int debugPage, PhysicalParticle particle)
		{
			if (debugPage == 1)
			{
				//b.AppendLine($"Constraint Forces:\n{TotalConstraintForces?.Transpose().ToMatrixString()}");
				b.AppendLine($"Lagrange Multipliers:\n{savedLagrangeMultiplier}");
				b.AppendLine($"Jacobian:\n{savedJacobian?.ToMatrixString()}");
				//b.AppendLine($"Velocity:\n{Velocity?.Transpose()?.ToMatrixString()}");
				//b.AppendLine($"Coefficient Matrix: (det {CoefficientMatrix?.Determinant()})\n{CoefficientMatrix?.ToMatrixString()}");
				//b.AppendLine($"Equation Constants:\n{EquationConstants?.ToMatrixString()}");
			}
		}

		public void IntegrateKinematicVariables(double dt)
		{
			foreach (var item in System.Particles)
			{
				//item.AngularVelocity += dt * (item.Torque + item.ConstraintTorque) / item.InertialMoment;
				item.Angle += dt * item.AngularVelocity;

				//item.Velocity += dt * (item.Force + item.ConstraintForce) / item.Mass;
				item.Position += dt * item.Velocity;

				item.UpdatePolygonTransformation();
			}
		}
	}
}