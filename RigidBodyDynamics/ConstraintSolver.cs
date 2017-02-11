using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AgateLib.DisplayLib.Particles;
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

		private SimpleJacobianDifferentiator jacobianDifferentiator = new SimpleJacobianDifferentiator();
		private Matrix<float> lagrangeParameter;
		private Matrix<float> massInverseMatrix;
		private Matrix<float> velocity;
		private Matrix<float> forces;

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
		/// <param name="dt">The amount of time in seconds that has passed.</param>
		public void Update(float dt)
		{
			InitializeStep();

			ComputeJacobian();

			ComputeConstraintForces(dt);
		}

		private void InitializeStep()
		{
			jacobianDifferentiator.Rows = JacobianRows;
			jacobianDifferentiator.Columns = JacobianColumns;

			jacobianDifferentiator.Advance();

			InitializeVector(ref lagrangeParameter, false);

			InitializeMassMatrix();
			InitializeVelocityVector();
			InitializeForceVector();
		}

		private void InitializeVelocityVector()
		{
			InitializeVector(ref velocity, true);

			for (int i = 0; i < Particles.Count; i++)
			{
				var particle = Particles[i];

				VectorSumValueForParticle(velocity, i, particle.Position.X, particle.Position.Y, particle.Angle);
			}
		}

		private void InitializeForceVector()
		{
			InitializeVector(ref forces, true);

			for (int i = 0; i < Particles.Count; i++)
			{
				var particle = Particles[i];

				VectorSumValueForParticle(velocity, i, particle.Force.X, particle.Force.Y, particle.Torque);
			}
		}

		private void VectorSumValueForParticle(Matrix<float> matrix, int particleIndex, float X, float Y, float Angle)
		{
			int basis = particleIndex * 3;

			matrix[particleIndex + 0, 0] = X;
			matrix[particleIndex + 1, 0] = Y;
			matrix[particleIndex + 2, 0] = Angle;
		}

		private void ComputeConstraintForces(float dt)
		{
			var jacobian = jacobianDifferentiator.Current;
			var derivative = jacobianDifferentiator.ComputeDerivative(dt);
			
			// Here's the equation:
			//  J * M^(-1) * J^T * lambda = -dJ/dT * \dot{q} - J * M^(-1) * F_{ext}
			// Lambda (the Lagrange parameter) is the set of unknowns. 
			// This is just a straightfoward system of linear equations of the form
			//  A * x = B
			// Solve for x by doing:
			//      x = A^-1 * B

			Matrix<float> A = jacobian * massInverseMatrix * jacobian.Transpose();
			Matrix<float> B = derivative * velocity - jacobian * massInverseMatrix * forces;

			lagrangeParameter = A.Inverse() * B;

			Debug.Assert(lagrangeParameter.RowCount == Constraints.Count);
			Debug.Assert(lagrangeParameter.ColumnCount == 1);
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
				int basis = i * 3;

				// first two generalized coordinates are X and Y, third is angle.
				massInverseMatrix[basis + 0, basis + 0] = (float)Particles[i].Mass;
				massInverseMatrix[basis + 1, basis + 1] = (float)Particles[i].Mass;
				massInverseMatrix[basis + 2, basis + 2] = (float)Particles[i].IntertialMoment;
			}
		}


		private void ComputeJacobian()
		{
			var jacobian = this.jacobianDifferentiator.Current;

			Parallel.For(0, Constraints.Count, i =>
			{
				var constraint = Constraints[i];

				for (int j = 0; j < Particles.Count; j++)
				{
					if (!constraint.AppliesTo(Particles[j]))
						continue;

					ConstraintDerivative derivative = constraint.Derivative(Particles[j]);

					jacobian[i, j * 3] = derivative.RespectToX;
					jacobian[i, j * 3 + 1] = derivative.RespectToY;
					jacobian[i, j * 3 + 2] = derivative.RespectToAngle;
				}
			});
		}
	}
}