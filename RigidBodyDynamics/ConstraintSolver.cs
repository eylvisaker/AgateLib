using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AgateLib.DisplayLib.Particles;
using AgateLib.Geometry;
using MathNet.Numerics.LinearAlgebra;

namespace RigidBodyDynamics
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Algorithm from 
	/// https://www.toptal.com/game/video-game-physics-part-iii-constrained-rigid-body-simulation
	/// </remarks>
	public class ConstraintSolver
	{
		/// <summary>
		/// This is three because each particle has three coordinates: X, Y, and rotation angle.
		/// </summary>
		private const int GeneralizedCoordinatesPerObject = 3;

		private Matrix<float> jacobian;
		private Matrix<float> lagrangeParameters;
		private Matrix<float> massInverseMatrix;
		private Matrix<float> velocity;
		private Matrix<float> externalForces;
		private Matrix<float> constraintForces;

		public ConstraintSolver(KinematicsSystem system)
		{
			System = system;
		}

		/// <summary>
		/// The number of generalized coordinates.
		/// </summary>
		private int GeneralizedCoordinates => Particles.Count * GeneralizedCoordinatesPerObject;

		/// <summary>
		/// The number of columns in the Jacobian matrix.
		/// </summary>
		private int JacobianColumns => GeneralizedCoordinates;

		/// <summary>
		/// The number of rows in the Jacobian matrix. 
		/// </summary>
		private int JacobianRows => Constraints.Count;

		private List<PhysicalParticle> Particles => System.Particles;

		private List<IPhysicalConstraint> Constraints => System.Constraints;

		public KinematicsSystem System { get; set; }

		/// <summary>
		/// Updates the dynamics.
		/// </summary>
		public void Update()
		{
			InitializeStep();

			ComputeJacobian();

			ComputeConstraintForces();
		}

		public void ApplyConstraintForces()
		{
			if (lagrangeParameters == null)
				return;

			constraintForces = jacobian.Transpose() * lagrangeParameters;

			for (int i = 0; i < Particles.Count; i++)
			{
				var part = Particles[i];
				int basis = i * 3;

				var constraint = new Vector2(constraintForces[basis + 0, 0],
											 constraintForces[basis + 1, 0]);

				var newForce = part.Force + constraint;
				var newTorque = part.Torque + constraintForces[basis + 2, 0];

				part.Force = newForce;
				part.Torque += newTorque;
			}
		}

		private void ComputeJacobian()
		{
			Parallel.For(0, Constraints.Count, i =>
			{
				var constraint = Constraints[i];

				for (int j = 0; j < Particles.Count; j++)
				{
					if (!constraint.AppliesTo(Particles[j]))
						continue;

					ConstraintDerivative derivative = constraint.Derivative(Particles[j]);

					int basis = j * GeneralizedCoordinatesPerObject;

					jacobian[i, basis + 0] = derivative.RespectToX;
					jacobian[i, basis + 1] = derivative.RespectToY;
					jacobian[i, basis + 2] = derivative.RespectToAngle;
				}
			});
		}


		private Matrix<float> ComputeJacobianDerivative()
		{
			var jacobDerivative = Matrix<float>.Build.Dense(JacobianRows, JacobianColumns);

			Parallel.For(0, Constraints.Count, i =>
			{
				var constraint = Constraints[i];

				for (int j = 0; j < Particles.Count; j++)
				{
					if (!constraint.AppliesTo(Particles[j]))
						continue;

					ConstraintDerivative derivative = constraint.MixedPartialDerivative(Particles[j]);

					int basis = j * GeneralizedCoordinatesPerObject;

					jacobDerivative[i, basis + 0] = derivative.RespectToX;
					jacobDerivative[i, basis + 1] = derivative.RespectToY;
					jacobDerivative[i, basis + 2] = derivative.RespectToAngle;
				}
			});

			return jacobDerivative;
		}

		private void ComputeConstraintForces()
		{
			var derivative = ComputeJacobianDerivative();

			// Here's the equation:
			//  J * M^(-1) * J^T * lambda = -dJ/dt * v - J * M^(-1) * F_{ext}
			// Lambda (the Lagrange parameter) is the set of unknowns. 
			// This is just a straightfoward system of linear equations of the form
			//  A * x = B
			// where A = J * M^(-1) * J^T 
			//       x = lambda
			//       B = -dJ/dt * v - J * M^(-1) * F_{ext}

			Matrix<float> A = jacobian * massInverseMatrix * jacobian.Transpose();
			Matrix<float> B = -derivative * velocity - jacobian * massInverseMatrix * externalForces;

			if (MatrixIsZero(A))
			{
				lagrangeParameters = Matrix<float>.Build.Dense(Constraints.Count, 1);
				return;
			}

			lagrangeParameters = A.Solve(B);

			//Matrix<float> aInv = A.PseudoInverse();

			//lagrangeParameters = aInv * B;

			Debug.Assert(lagrangeParameters.RowCount == Constraints.Count);
			Debug.Assert(lagrangeParameters.ColumnCount == 1);
		}

		private void InitializeStep()
		{
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

		private void InitializeVelocityVector()
		{
			InitializeVector(ref velocity, true);

			for (int i = 0; i < Particles.Count; i++)
			{
				var particle = Particles[i];

				VectorSumValueForParticle(velocity, i, particle.Velocity.X, particle.Velocity.Y, particle.AngularVelocity);
			}
		}

		private void InitializeForceVector()
		{
			InitializeVector(ref externalForces, true);

			for (int i = 0; i < Particles.Count; i++)
			{
				var particle = Particles[i];

				VectorSumValueForParticle(externalForces, i, particle.Force.X, particle.Force.Y, particle.Torque);
			}
		}


		private void InitializeVector(ref Matrix<float> vector, bool clear)
		{
			if (vector == null || vector.RowCount != GeneralizedCoordinates)
				vector = Matrix<float>.Build.Dense(GeneralizedCoordinates, 1);

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
				int basis = i * GeneralizedCoordinatesPerObject;

				// first two generalized coordinates are X and Y, third is angle.
				massInverseMatrix[basis + 0, basis + 0] = 1 / (float)Particles[i].Mass;
				massInverseMatrix[basis + 1, basis + 1] = 1 / (float)Particles[i].Mass;
				massInverseMatrix[basis + 2, basis + 2] = 1 / (float)Particles[i].InertialMoment;
			}

		}

		private void VectorSumValueForParticle(Matrix<float> matrix, int particleIndex, float X, float Y, float Angle)
		{
			int basis = particleIndex * GeneralizedCoordinatesPerObject;

			matrix[basis + 0, 0] = X;
			matrix[basis + 1, 0] = Y;
			matrix[basis + 2, 0] = Angle;
		}

		private bool MatrixIsZero(Matrix<float> matrix)
		{
			for (int i = 0; i < matrix.RowCount; i++)
			{
				for (int j = 0; j < matrix.ColumnCount; j++)
				{
					if (Math.Abs(matrix[i, j]) > 1e-6f)
					{
						var determinant = matrix.Determinant();
						var result = Math.Abs(determinant) <= 1e-8f;

						return result;
					}
				}
			}

			return true;
		}

	}
}