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
using System.Threading.Tasks;
using AgateLib.Physics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Xna.Framework;

namespace AgateLib.Physics.TwoDimensions.Solvers
{
	/// <summary>
	/// Unstable.
	/// </summary>
	/// <remarks>
	/// Algorithm from 
	/// https://www.toptal.com/game/video-game-physics-part-iii-constrained-rigid-body-simulation
	/// </remarks>
	public class ProjectedGaussSeidelConstraintSolver : IConstraintSolver
	{
		/// <summary>
		/// This is three because each particle has three coordinates: X, Y, and rotation angle.
		/// </summary>
		private const int GeneralizedCoordinatesPerParticle = 3;

		private const float DefaultSpringConstant = 25;

		private Matrix<float> jacobian;
		private Matrix<float> lagrangeMultipliers;
		private Matrix<float> massInverseMatrix;
		private Matrix<float> velocity;
		private Matrix<float> externalForces;
		private Matrix<float> totalConstraintForces;
		private Matrix<float> constraintValues;
		private List<Vector<float>> constraintForces;
		private GaussSeidelAlgorithm gaussSeidel = new GaussSeidelAlgorithm();

		public ProjectedGaussSeidelConstraintSolver(KinematicsSystem system)
		{
			System = system;
		}

		/// <summary>
		/// The number of generalized coordinates.
		/// </summary>
		private int GeneralizedCoordinateCount => Particles.Count * GeneralizedCoordinatesPerParticle;

		/// <summary>
		/// The number of columns in the Jacobian matrix.
		/// </summary>
		private int JacobianColumns => GeneralizedCoordinateCount;

		/// <summary>
		/// The number of rows in the Jacobian matrix. 
		/// </summary>
		private int JacobianRows => Constraints.Count;

		private IReadOnlyList<PhysicalParticle> Particles => System.Particles;

		private List<AppliedConstraint> Constraints { get; set; } = new List<AppliedConstraint>();

		public KinematicsSystem System { get; set; }

		public float SpringConstant { get; set; } = DefaultSpringConstant;

		public float DampeningConstant { get; set; } = (float)Math.Sqrt(DefaultSpringConstant);

		public Matrix<float> LagrangeMultipliers => lagrangeMultipliers;

		public Matrix<float> Jacobian => jacobian;

		public Matrix<float> CoefficientMatrix { get; private set; }
		public Matrix<float> EquationConstants { get; private set; }

		public Matrix<float> TotalConstraintForces => totalConstraintForces;

		public Matrix<float> Velocity => velocity;

		public List<Vector<float>> ConstraintForces => constraintForces;

		/// <summary>
		/// Computes the constraint forces from the current state of the system.
		/// </summary>
		public void ComputeConstraintForces(float dt)
		{
			InitializeStep();

			ComputeJacobian();

			SolveConstraintEquations(dt);
		}

		public void ApplyConstraintForces()
		{
			if (Constraints.Count == 0)
			{
				Parallel.ForEach(Particles, particle =>
				{
					particle.ConstraintForce = Vector2.Zero;
					particle.ConstraintTorque = 0;
				});

				return;
			}

#if DEBUG
			var check = totalConstraintForces.Transpose() * velocity;
			Debug.WriteLine($"Constraint force dot product: {check[0, 0]}");
#endif

			for (int i = 0; i < Particles.Count; i++)
			{
				var particle = Particles[i];
				int basis = i * 3;

				var totalConstraintForce = new Vector2(totalConstraintForces[basis + 0, 0],
													   totalConstraintForces[basis + 1, 0]);

				particle.ConstraintForce = totalConstraintForce;
				particle.ConstraintTorque = totalConstraintForces[basis + 2, 0];
			}
		}

		private void ComputeJacobian()
		{
			//	Parallel.For(0, Constraints.Count, i =>
			for (int i = 0; i < Constraints.Count; i++)
			{
				var constraint = Constraints[i];

				for (int j = 0; j < Particles.Count; j++)
				{
					if (!constraint.AppliesTo(Particles[j]))
						continue;

					ConstraintDerivative derivative = constraint.Derivative(Particles[j]);

					int basis = j * GeneralizedCoordinatesPerParticle;

					jacobian[i, basis + 0] = derivative.RespectToX;
					jacobian[i, basis + 1] = derivative.RespectToY;
					jacobian[i, basis + 2] = derivative.RespectToAngle;
				}
			}
		}

		private void SolveConstraintEquations(float dt)
		{
			if (Constraints.Count == 0)
				return;

			// Here's the equation:
			//    J * M^(-1) * J^T * lambda = -J(v / dt - J * M^(-1) * F_{ext}
			// Lambda (the Lagrange parameter) is the set of unknowns. 
			// This is just a straightfoward system of linear equations of the form
			//  A * x = B
			// where A = J * M^(-1) * J^T 
			//       x = Lagrange parameters
			//       B = -J * (v/dt + M^(-1) * F_{ext})
			//
			//   A is Nc x Nc where Nc is the number of constraints
			//   x is Nc x 1 where Nc is the number of constraints
			//   B is 


			// Interpretation is:
			//  A - diagonal elements are the .Length()" of the Jacobian for the i-th 
			//      constraint. Dimension is NxN where N is the number of constraints.
			//  B - J*v is derivative of the constraint. Alternately, it is the projection 
			//      of the velocity in the direction of the Jacobian and indicates how much 
			//      the constraint value will change if the particle continues its motion  
			//      without any applied force. Dimension is Mx1 where M is the number of particles.
			Matrix<float> A = jacobian * massInverseMatrix * jacobian.Transpose();
			Matrix<float> B = -jacobian * (velocity / dt + massInverseMatrix * externalForces);

			B -= SpringConstant * constraintValues;// + DampeningConstant * jacobian * velocity;

			CoefficientMatrix = A;
			EquationConstants = B;

			Matrix<float> lowerLimit = Matrix<float>.Build.Dense(Constraints.Count, 1);
			Matrix<float> upperLimit = Matrix<float>.Build.Dense(Constraints.Count, 1);

			for (int i = 0; i < Constraints.Count; i++)
			{
				lowerLimit[i, 0] = Constraints[i].MultiplierMin;
				upperLimit[i, 0] = Constraints[i].MultiplierMax;
			}

			lagrangeMultipliers = gaussSeidel.SolveProjected(A, B, lowerLimit, upperLimit);

			totalConstraintForces = jacobian.Transpose() * lagrangeMultipliers;

			for (int i = 0; i < Constraints.Count; i++)
			{
				constraintForces[i] = jacobian.Transpose().Column(i) * lagrangeMultipliers[i, 0];
			}

			Debug.Assert(lagrangeMultipliers.RowCount == Constraints.Count);
			Debug.Assert(lagrangeMultipliers.ColumnCount == 1);
		}

		private void InitializeStep()
		{
			InitializeConstraintValues();

			if (Constraints.Count == 0)
				return;

			InitializeJacobian();
			InitializeMassMatrix();
			InitializeVelocityVector();
			InitializeForceVector();
		}

		/// <summary>
		/// Called at the beginning of Derivative only. Initializes the 
		/// Jacobian matrix to be the right size and zeroed out.
		/// </summary>
		/// <returns></returns>
		public void InitializeJacobian()
		{
			if (jacobian == null ||
				jacobian.RowCount != JacobianRows ||
				jacobian.ColumnCount != JacobianColumns)
			{
				jacobian = Matrix<float>.Build.Dense(JacobianRows, JacobianColumns);
			}
			else
			{
				for (int i = 0; i < jacobian.RowCount; i++)
				{
					for (int j = 0; j < jacobian.ColumnCount; j++)
					{
						jacobian[i, j] = 0;
					}
				}
			}
		}

		private void InitializeConstraintValues()
		{
			Constraints.Clear();

			foreach (var constraint in System.Constraints)
			{
				foreach (var group in constraint.ApplyTo(System))
				{
					Constraints.Add(new AppliedConstraint
					{
						Constraint = constraint,
						Particles = group.ToList(),
					});
				}
			}

			if (Constraints.Count == 0)
				return;

			constraintValues = Matrix<float>.Build.Dense(Constraints.Count, 1);
			constraintForces = new List<Vector<float>>();

			for (int i = 0; i < Constraints.Count; i++)
			{
				constraintValues[i, 0] = Constraints[i].Value;
				constraintForces.Add(Vector<float>.Build.Dense(GeneralizedCoordinateCount));
			}
		}

		private void InitializeVelocityVector()
		{
			InitializeVector(ref velocity, true);

			for (int i = 0; i < Particles.Count; i++)
			{
				var particle = Particles[i];

				CopyValuesForParticle(velocity, i, particle.Velocity.X, particle.Velocity.Y, particle.AngularVelocity);
			}
		}

		private void InitializeForceVector()
		{
			InitializeVector(ref externalForces, true);

			for (int i = 0; i < Particles.Count; i++)
			{
				var particle = Particles[i];

				CopyValuesForParticle(externalForces, i, particle.Force.X, particle.Force.Y, particle.Torque);
			}
		}


		private void InitializeVector(ref Matrix<float> vector, bool clear)
		{
			if (vector == null || vector.RowCount != GeneralizedCoordinateCount)
				vector = Matrix<float>.Build.Dense(GeneralizedCoordinateCount, 1);

			if (clear)
				vector.CoerceZero(float.MaxValue);
		}

		private void InitializeMassMatrix()
		{
			if (massInverseMatrix == null || massInverseMatrix.ColumnCount != JacobianColumns)
			{
				massInverseMatrix = Matrix<float>.Build.Diagonal(JacobianColumns, JacobianColumns);
			}

			for (int i = 0; i < Particles.Count; i++)
			{
				int basis = i * GeneralizedCoordinatesPerParticle;

				// first two generalized coordinates are X and Y, third is angle.
				massInverseMatrix[basis + 0, basis + 0] = 1 / Particles[i].Mass;
				massInverseMatrix[basis + 1, basis + 1] = 1 / Particles[i].Mass;
				massInverseMatrix[basis + 2, basis + 2] = 1 / Particles[i].InertialMoment;
			}
		}

		private void CopyValuesForParticle(Matrix<float> matrix, int particleIndex, float x, float y, float angle)
		{
			int basis = particleIndex * GeneralizedCoordinatesPerParticle;

			matrix[basis + 0, 0] = x;
			matrix[basis + 1, 0] = y;
			matrix[basis + 2, 0] = angle;
		}

		public void DebugInfo(StringBuilder b, int debugPage, PhysicalParticle particle)
		{
			if (debugPage == 1)
			{
				b.AppendLine($"Constraint Forces:\n{TotalConstraintForces?.Transpose().ToMatrixString()}");
				b.AppendLine($"Lagrange Multipliers:\n{LagrangeMultipliers?.Transpose()?.ToMatrixString()}");
				b.AppendLine($"Jacobian:\n{Jacobian?.ToMatrixString()}");
				b.AppendLine($"Velocity:\n{Velocity?.Transpose()?.ToMatrixString()}");
				b.AppendLine($"Coefficient Matrix: (det {CoefficientMatrix?.Determinant()})\n{CoefficientMatrix?.ToMatrixString()}");
				b.AppendLine($"Equation Constants:\n{EquationConstants?.ToMatrixString()}");
			}
		}


		public void IntegrateKinematicVariables(float dt)
		{
			foreach (var item in System.Particles)
			{
				item.AngularVelocity += dt * (item.Torque + item.ConstraintTorque) / item.InertialMoment;
				item.Angle += dt * item.AngularVelocity;

				item.Velocity += dt * (item.Force + item.ConstraintForce) / item.Mass;
				item.Position += dt * item.Velocity;

				item.UpdatePolygonTransformation();
			}
		}
	}
}