using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgateLib.Quality;

namespace RigidBodyDynamics
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Algorithm from 
	/// https://www.toptal.com/game/video-game-physics-part-iii-constrained-rigid-body-simulation
	/// </remarks>
	internal class KinematicsSolver
	{
		/// <summary>
		/// This is three because each particle has three coordinates: X, Y, and rotation angle.
		/// </summary>
		private const int GeneralizedCoordinatesPerObject = 3;

		private List<Physical> PhysicalObjects = new List<Physical>();
		private List<IPhysicalConstraint> Constraints = new List<IPhysicalConstraint>();
		private SimpleJacobianDifferentiator jacobianDifferentiator = new SimpleJacobianDifferentiator();

		/// <summary>
		/// The number of columns in the jacobianDifferentiator matrix.
		/// </summary>
		private int Columns => PhysicalObjects.Count * GeneralizedCoordinatesPerObject;

		/// <summary>
		/// The number of rows in the jacobianDifferentiator matrix. 
		/// </summary>
		private int Rows => Constraints.Count;

		public void Update(TimeSpan elapsed)
		{
			var dt = elapsed.TotalSeconds;

			jacobianDifferentiator.Advance();

			IntegrateKinematicVariables(dt);
		}

		private void ComputeJacobian()
		{
			var jacobian = this.jacobianDifferentiator.Current;

			Parallel.For(0, Constraints.Count, i =>
			{
				var constraint = Constraints[i];

				for (int j = 0; j < PhysicalObjects.Count; j++)
				{
					if (!constraint.AppliesTo(PhysicalObjects[j]))
						continue;

					ConstraintDerivative derivative = constraint.Derivative(PhysicalObjects[j]);

					jacobian[i, j * 3] = derivative.RespectToX;
					jacobian[i, j * 3 + 1] = derivative.RespectToY;
					jacobian[i, j * 3 + 2] = derivative.RespectToAngle;
				}
			});
		}

		private void IntegrateKinematicVariables(double dt)
		{
			foreach (var item in PhysicalObjects)
			{
				item.Angle += item.AngularVelocity * dt;

				item.Velocity += dt * item.Force / item.Mass;
				item.Position += dt * item.Velocity;
			}
		}

		public void AddObjects(params Physical[] items)
		{
			PhysicalObjects.AddRange(items);
		}

		public void AddConstraints(List<IPhysicalConstraint> constraints)
		{
			Constraints.AddRange(constraints);
		}
	}

	public class ValueHistory<T>
	{
		/// <summary>
		/// Most recent item is placed in history[0], oldest item is in history[history.Length - 1]
		/// </summary>
		private T[] history;
		private int historySize = 2;

		public ValueHistory()
		{
			Size = historySize;
		}

		public int Size
		{
			get { return historySize; }
			set
			{
				Require.ArgumentInRange(value > 0, nameof(Size), $"{nameof(Size)} must be positive.");

				historySize = value;

				var old = history;
				history = new T[Size];

				if (old != null)
				{
					for (int i = 0; i < old.Length && i < history.Length; i++)
					{
						history[i] = old[i];
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public T Current
		{
			get { return history[0]; }
			set { history[0] = value; }
		}

		public T this[int index] => history[index];

		public void Cycle()
		{
			T last = history[history.Length - 1];

			for (int i = history.Length - 2; i >= 0; i++)
			{
				history[i] = history[i - 1];
			}

			history[0] = last;
		}
	}

	public class SimpleJacobianDifferentiator 
	{
		private const int historySize = 2;

		private ValueHistory<Jacobian> history = new ValueHistory<Jacobian>();

		public int Rows { get; set; }

		public int Columns { get; set; }

		public Jacobian Current => history.Current;

		public void Advance()
		{
			FixHistory();

			history.Cycle();

			var current = history.Current;

			Initialize(ref current);

			history.Current = current;
		}

		/// <summary>
		/// If any parameters have changed, redo history.
		/// </summary>
		private void FixHistory()
		{
			if (history == null || history.Size != historySize)
			{
				history = new ValueHistory<Jacobian> { Size = 2 };

				history.Current = new Jacobian(Rows, Columns);
				history.Cycle();

				history.Current = new Jacobian(Rows, Columns);
			}
		}

		/// <summary>
		/// Called at the beginning of Derivative only. Initializes the 
		/// Jacobian matrix to be the right size and zeroed out.
		/// </summary>
		/// <returns></returns>
		public void Initialize(ref Jacobian jacobian)
		{
			if (jacobian == null ||
				jacobian.Rows != Rows ||
				jacobian.Columns != Columns)
			{
				jacobian = new Jacobian(Rows, Columns);
			}
			else
			{
				for (int i = 0; i < jacobian.Rows; i++)
				{
					for (int j = 0; j < jacobian.Columns; j++)
					{
						jacobian[i, j] = 0;
					}
				}
			}
		}
	}
}