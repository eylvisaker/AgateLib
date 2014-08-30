using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AgateLib.Geometry;
using AgateLib.Algorithms.PathFinding;

namespace AgateLib.Algorithms.PathFinding
{
	[TestClass]
	public class AStarTest
	{
		class FakeMap : IAStarMap<Point>
		{
			public void ReportProgress(AStarState<Point> task)
			{
			}

			public int CalculateHeuristic(Point location, Point destination)
			{
				return Math.Abs(destination.X - location.X) + Math.Abs(destination.Y - location.Y);
			}
			public int CalculateHeuristic(Point location, List<Point> destination)
			{
				int minval = int.MaxValue;

				foreach (var dest in destination)
				{
					int val = CalculateHeuristic(location, dest);
					if (val < minval) minval = val;
				}

				return minval;
			}

			public IEnumerable<Point> GetAvailableSteps(AStarState<Point> task, Point location)
			{
				for (int j = -1; j <= 1; j++)
				{
					for (int i = -1; i <= 1; i++)
					{
						if (i == j || i == -j) continue;

						Point trial = new Point(location.X + i, location.Y + j);

						if (IsAvailable(trial))
							yield return trial;
					}
				}
			}

			private bool IsAvailable(Point trial)
			{
				if (trial.X < 0) return false;
				if (trial.Y < 0) return false;
				if (trial.X > 15) return false;
				if (trial.Y > 15) return false;

				// area in (3, 3) - (12, 12) is blocked
				if (trial.X < 3) return true;
				if (trial.Y < 3) return true;
				if (trial.X > 12) return true;
				if (trial.Y > 12) return true;

				return false;
			}

			public int GetStepCost(AgateLib.Geometry.Point target, AgateLib.Geometry.Point start)
			{
				return 1;
			}
		}

		[TestMethod]
		public void AStarPath()
		{
			AStarState<Point> task = new AStarState<Point>();
			task.Start = new AgateLib.Geometry.Point(4, 2);
			task.EndPoints.Add(new AgateLib.Geometry.Point(5, 15));

			var astar = new AStar<Point>(new FakeMap());

			astar.FindPathSync(task);

			// two steps to the left to get to (2, 2)
			// 13 steps down to get to (2, 15)
			// 3 steps to the right to get to (5, 15)
			// that's 18 steps, plus the start point makes 19.
			Assert.AreEqual(19, task.Path.Count);
		}
	}
}
