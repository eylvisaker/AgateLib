using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using MathNet.Numerics.LinearAlgebra;

namespace AgateLib.Physics
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

		private const double DefaultSpringConstant = 25;

		private Matrix<double> jacobian;
		private Matrix<double> lagrangeMultipliers;
		private Matrix<double> massInverseMatrix;
		private Matrix<double> velocity;
		private Matrix<double> externalForces;
		private Matrix<double> totalConstraintForces;
		private Matrix<double> constraintValues;
		private List<Vector<double>> constraintForces;

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

		public double SpringConstant { get; set; } = DefaultSpringConstant;

		public double DampeningConstant { get; set; } = Math.Sqrt(DefaultSpringConstant);

		public Matrix<double> LagrangeMultipliers => lagrangeMultipliers;

		public Matrix<double> Jacobian => jacobian;

		public Matrix<double> CoefficientMatrix { get; private set; }
		public Matrix<double> EquationConstants { get; private set; }

		public Matrix<double> TotalConstraintForces => totalConstraintForces;

		public Matrix<double> Velocity => velocity;

		public List<Vector<double>> ConstraintForces => constraintForces;

		/// <summary>
		/// Computes the constraint forces from the current state of the system.
		/// </summary>
		public void ComputeConstraintForces(double dt)
		{
			InitializeStep();

			ComputeJacobian();

			SolveConstraintEquations(dt);
		}

		public void ApplyConstraintForces()
		{
			if (totalConstraintForces == null)
				return;

			var check = totalConstraintForces.Transpose() * velocity;
			Debug.WriteLine($"Constraint force dot product: {check[0, 0]}");

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
			Parallel.For(0, Constraints.Count, i =>
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
			});
		}

		private void SolveConstraintEquations(double dt)
		{
			// Here's the equation:
			//    J * M^(-1) * J^T * lambda = -J(v / dt - J * M^(-1) * F_{ext}
			// Lambda (the Lagrange parameter) is the set of unknowns. 
			// This is just a straightfoward system of linear equations of the form
			//  A * x = B
			// where A = J * M^(-1) * J^T 
			//       x = lambda
			//       B = -J * (v/dt + M^(-1) * F_{ext})

			Matrix<double> A = jacobian * massInverseMatrix * jacobian.Transpose();
			Matrix<double> B = -jacobian * (velocity / dt + massInverseMatrix * externalForces);

			B -= SpringConstant * constraintValues / dt;// + DampeningConstant * jacobian * velocity;

			CoefficientMatrix = A;
			EquationConstants = B;

			lagrangeMultipliers = ProjectedGaussSeidelSolution(A, B);
			
			totalConstraintForces = jacobian.Transpose() * lagrangeMultipliers;

			for (int i = 0; i < Constraints.Count; i++)
			{
				constraintForces[i] = jacobian.Transpose().Column(i) * lagrangeMultipliers[i, 0];
			}

			Debug.Assert(lagrangeMultipliers.RowCount == Constraints.Count);
			Debug.Assert(lagrangeMultipliers.ColumnCount == 1);
		}

		/// <summary>
		/// </summary>
		/// <param name="A"></param>
		/// <param name="B"></param>
		/// <returns></returns>
		/// <remarks>
		/// Method is described here:
		/// https://en.wikipedia.org/wiki/Gauss%E2%80%93Seidel_method
		/// </remarks>
		public Matrix<double> ProjectedGaussSeidelSolution(Matrix<double> A, Matrix<double> B)
		{
			const double tolerance = 1e-6;
			Matrix<double> result = Matrix<double>.Build.Dense(A.RowCount, 1);
			int[] pivot = Enumerable.Range(0, result.RowCount).ToArray();
			bool singular = false;
			int singularIndex = result.RowCount;

			// perform pivoting if any diagonal elements are zero.
			for (int i = 0; i < A.RowCount; i++)
			{
				if (Math.Abs(A[i, i]) < tolerance)
				{
					int swapIndex = i;
					double maxValue = tolerance;

					for (int j = i + 1; j < A.RowCount; j++)
					{
						if (Math.Abs(A[j, i]) > maxValue)
						{
							maxValue = Math.Abs(A[j, i]);
							swapIndex = j;
						}
					}

					if (swapIndex == i)
					{
						// we have a singular matrix, so just move this row to the bottom.
						singularIndex--;
						swapIndex = singularIndex;
						singular = true;
					}

					pivot[i] = swapIndex;
					pivot[swapIndex] = i;
				}
			}

			double error;
			int iter = 0;
			const int maxIter = 50;

			do
			{
				error = 0;
				iter++;

				for (int i = 0; i < singularIndex; i++)
				{
					var element = B[pivot[i], 0];

					for (int j = 0; j < i; j++)
					{
						element -= A[pivot[i], j] * result[pivot[j], 0];
					}

					for (int j = i + 1; j < result.RowCount; j++)
					{
						element -= A[pivot[i], j] * result[pivot[j], 0];
					}

					element /= A[pivot[i], pivot[i]];

					var oldElement = result[pivot[i], 0];
					error += Math.Abs(element - oldElement);

					result[pivot[i], 0] = element;
				}

			} while (error > tolerance && iter < maxIter);

			return result;
		}

		private void InitializeStep()
		{
			InitializeConstraintValues();
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
				jacobian = Matrix<double>.Build.Dense(JacobianRows, JacobianColumns);
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
						Particles = group,
					});
				}
			}
			

			constraintValues = Matrix<double>.Build.Dense(Constraints.Count, 1);
			constraintForces = new List<Vector<double>>();

			for (int i = 0; i < Constraints.Count; i++)
			{
				constraintValues[i, 0] = Constraints[i].Value;
				constraintForces.Add(Vector<double>.Build.Dense(GeneralizedCoordinateCount));
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


		private void InitializeVector(ref Matrix<double> vector, bool clear)
		{
			if (vector == null || vector.RowCount != GeneralizedCoordinateCount)
				vector = Matrix<double>.Build.Dense(GeneralizedCoordinateCount, 1);

			if (clear)
				vector.CoerceZero(float.MaxValue);
		}

		private void InitializeMassMatrix()
		{
			if (massInverseMatrix == null || massInverseMatrix.ColumnCount != JacobianColumns)
			{
				massInverseMatrix = Matrix<double>.Build.Diagonal(JacobianColumns, JacobianColumns);
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

		private void CopyValuesForParticle(Matrix<double> matrix, int particleIndex, double x, double y, double angle)
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


		public void IntegrateKinematicVariables(double dt)
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