
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
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
    public class ImpulseConstraintSolverV1 : IConstraintSolver
    {
        /// <summary>
        /// This is three because each particle has three coordinates: X, Y, and rotation angle.
        /// </summary>
        private const int GeneralizedCoordinatesPerParticle = 3;

        private const float DefaultSpringConstant = 50f;

        private Matrix<double> jacobian;
        private Matrix<double> massInverseMatrix;
        private Matrix<double> externalForces;
        private Matrix<double> totalConstraintImpulse;
        private Matrix<double> constraintValues;
        private List<Vector<double>> constraintForces;

        private Dictionary<PhysicalParticle, Vector3> newVelocities = new Dictionary<PhysicalParticle, Vector3>();

        public ImpulseConstraintSolverV1(KinematicsSystem system)
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

        private IReadOnlyList<IPhysicalConstraint> Constraints => System.Constraints;

        public KinematicsSystem System { get; set; }

        public float SpringConstant { get; set; } = DefaultSpringConstant;

        public float DampeningConstant { get; set; } = (float)Math.Sqrt(DefaultSpringConstant);

        /// <summary>
        /// Computes the constraint forces from the current state of the system.
        /// </summary>
        public void ComputeConstraintForces(float dt)
        {
            InitializeStep(dt);

            SolveConstraintEquations(dt);
        }

        public void ApplyConstraintForces()
        {
            if (totalConstraintImpulse == null)
                return;

            for (int i = 0; i < Particles.Count; i++)
            {
                var particle = Particles[i];

                var newV = new Vector2(newVelocities[particle].X,
                                       newVelocities[particle].Y);

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

        private void SolveConstraintEquations(float dt)
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
                //  J * M^(-1) * J^T * lambda = -J * v - Bias
                // Lambda (the Lagrange parameter) is the set of unknowns. 
                // This is just a straightfoward system of linear equations of the form
                //  A * x = B
                // where A = J * M^(-1) * J^T 
                //       x = lambda
                //       B = -J * v - Bias
                var velocityStep = newVelocities.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                for (int j = 0; j < Constraints.Count; j++)
                {
                    var constraint = Constraints[j];

                    List<PhysicalParticle> particles = Particles.Where(p => constraint.AppliesTo(p)).ToList();

                    var jacobian = ComputeJacobian(constraint, particles);

                    var newVelocity = Matrix<double>.Build.Dense(particles.Count * GeneralizedCoordinatesPerParticle, 1);
                    var massInverseMatrix = Matrix<double>.Build.Dense(
                        particles.Count * GeneralizedCoordinatesPerParticle,
                        particles.Count * GeneralizedCoordinatesPerParticle);

                    for (int i = 0; i < particles.Count; i++)
                    {
                        int basis = i * GeneralizedCoordinatesPerParticle;

                        newVelocity[basis + 0, 0] = velocityStep[particles[i]].X;
                        newVelocity[basis + 1, 0] = velocityStep[particles[i]].Y;
                        newVelocity[basis + 2, 0] = velocityStep[particles[i]].Z;

                        massInverseMatrix[basis + 0, basis + 0] = 1 / particles[i].Mass;
                        massInverseMatrix[basis + 1, basis + 1] = 1 / particles[i].Mass;
                        massInverseMatrix[basis + 2, basis + 2] = 1 / particles[i].InertialMoment;
                    }

                    double A = (jacobian * massInverseMatrix * jacobian.Transpose())[0, 0];
                    double B1 = (-jacobian * newVelocity)[0, 0];

                    // Add bias to help force constraint apply.
                    //var bias = Matrix<double>.Build.Dense(particles.Count * GeneralizedCoordinatesPerParticle, 1);

                    var bias = SpringConstant * constraint.Value(particles) + (DampeningConstant * B1) * dt;
                    var B = B1 - bias;
                    //B -= constraintValues[j, 0] + DampeningConstant * jacobian.Row(j).ToColumnMatrix();

                    //NormalizeLinearEquations(A, tolerance);

                    //if (MatrixIsZero(A))
                    //{
                    //	lagrangeMultipliers = Matrix<double>.Build.Dense(Constraints.Count, 1);
                    //	return;
                    //}

                    if (A < tolerance)
                        continue;

                    var lagrangeMultipliers = B / A;

                    var impulse = jacobian.Transpose() * lagrangeMultipliers;

                    //Debug.WriteLine($"  Constraint {j}");
                    for (int i = 0; i < particles.Count; i++)
                    {
                        int basis = i * GeneralizedCoordinatesPerParticle;
                        var particle = particles[i];

                        Vector3 nv = newVelocities[particle];

                        nv.X += (float)impulse[basis + 0, 0] / particle.Mass;
                        nv.Y += (float)impulse[basis + 1, 0] / particle.Mass;
                        nv.Z += (float)impulse[basis + 2, 0] / particle.InertialMoment;

                        Vector3 error = nv - newVelocities[particle];
                        totalError += error.LengthSquared();

                        newVelocities[particle] = nv;

                        //Debug.WriteLine($"    Particle {i}: dV: {error.Magnitude}");
                    }
                }

                if (totalError < errorMax)
                    break;
            }

            Debug.WriteLine($"Total error: {totalError} after {nIter + 1} iterations.");
        }

        private static void NormalizeLinearEquations(Matrix<double> A, float tolerance)
        {
            for (int i = 0; i < A.RowCount; i++)
            {
                if (A.Row(i).SumMagnitudes() > tolerance)
                    continue;
                if (A.Column(i).SumMagnitudes() > tolerance)
                    continue;

                A[i, i] = 1;
            }
        }

        private void InitializeStep(float dt)
        {
            InitializeJacobian();
            InitializeConstraintValues();
            InitializeMassMatrix();
            InitializeVelocityVector(dt);
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
            constraintValues = Matrix<double>.Build.Dense(Constraints.Count, 1);
            constraintForces = new List<Vector<double>>();

            for (int i = 0; i < Constraints.Count; i++)
            {
                constraintValues[i, 0] = Constraints[i].Value(System.Particles);
                constraintForces.Add(Vector<double>.Build.Dense(GeneralizedCoordinateCount));
            }

            totalConstraintImpulse = Matrix<double>.Build.Dense(GeneralizedCoordinateCount, 1);
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

        private bool MatrixIsZero(Matrix<double> matrix)
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

        public void IntegrateKinematicVariables(float dt)
        {
            foreach (var item in System.Particles)
            {
                //item.AngularVelocity += dt * (item.Torque + item.ConstraintTorque) / item.InertialMoment;
                item.Rotation += dt * item.AngularVelocity;

                //item.Velocity += dt * (item.Force + item.ConstraintForce) / item.Mass;
                item.Position += dt * item.Velocity;
            }
        }

        public void DebugInfo(StringBuilder b, int debugPage, PhysicalParticle particle)
        {

        }
    }
}