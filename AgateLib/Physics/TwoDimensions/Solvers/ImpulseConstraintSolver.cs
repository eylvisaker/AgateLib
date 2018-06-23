//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Xna.Framework;

namespace AgateLib.Physics.TwoDimensions.Solvers
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

		private const float DefaultSpringConstant = 50;

		private Matrix<float> externalForces;

		private Matrix<float> savedJacobian;
		private float savedLagrangeMultiplier;
		private Matrix<float> constraintForces;

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

		public float SpringConstant { get; set; } = DefaultSpringConstant;

		public float DampeningConstant { get; set; } = (float)Math.Sqrt(DefaultSpringConstant);

		private float dt;

		/// <summary>
		/// Computes the constraint forces from the current state of the system.
		/// </summary>
		public void ComputeConstraintForces(float dt)
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

		private Matrix<float> ComputeJacobian(IPhysicalConstraint constraint, IReadOnlyList<PhysicalParticle> particles)
		{
			Matrix<float> jacobian = Matrix<float>.Build.Dense(1, GeneralizedCoordinatesPerParticle * particles.Count);

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

		private void SolveConstraintEquations(float dt)
		{
			const float tolerance = 1e-6f;
			const int maxIterations = 100;
			const float errorMax = 1e-4f;

			//Debug.WriteLine($"**************************** Starting iterations");

			float totalError = 0;
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

						var newVelocity = Matrix<float>.Build.Dense(
							particleGroup.Count * GeneralizedCoordinatesPerParticle, 1);
						var massInverseMatrix = Matrix<float>.Build.Dense(
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

						float A = (jacobian * massInverseMatrix * jacobian.Transpose())[0, 0];
						float B1 = (-jacobian * newVelocity)[0, 0];

						// Add bias to help force constraint apply.
						//var bias = Matrix<float>.Build.Dense(particles.Count * GeneralizedCoordinatesPerParticle, 1);

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
							totalError += error.LengthSquared();

							newVelocities[particle] = nv;

							//Debug.WriteLine($"    Particle {i}: dV: {error.Length()}");
						}
					}
				}

				if (totalError < errorMax)
					break;
			}

			Debug.WriteLine($"Total error: {totalError} after {nIter + 1} iterations.");
		}

		private void InitializeStep(float dt)
		{
			InitializeVelocityVector(dt);
		}

		private void InitializeVelocityVector(float dt)
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

		public void IntegrateKinematicVariables(float dt)
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