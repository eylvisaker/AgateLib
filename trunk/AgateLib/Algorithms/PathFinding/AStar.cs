using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AgateLib.Geometry;
using System.Threading.Tasks;

namespace AgateLib.Algorithms.PathFinding
{
	public class AStar<T>
	{
		const int maxSteps = 200;
		IAStarMap<T> mMap;
		bool mAbort;
		int mActiveTasks;

		Func<T, T, bool> mEquals;

		public AStar(IAStarMap<T> map)
			: this(map, (x, y) => x.Equals(y))
		{ }
		public AStar(IAStarMap<T> map, IEqualityComparer<T> comparer)
			: this(map, (x, y) => comparer.Equals(x, y))
		{ }
		public AStar(IAStarMap<T> map, Func<T, T, bool> comparison)
		{
			mMap = map;
			mEquals = comparison;
		}

		[Obsolete("Set map in constructor.", true)]
		public void SetMap(IAStarMap<T> map)
		{
			mAbort = true;

			while (mActiveTasks > 0)
			{
				//Thread.Sleep(0);
				Core.KeepAlive();
			}

			mMap = map;

			mAbort = false;
		}


		public void QueueFindPath(AStarState<T> task)
		{
			System.Diagnostics.Debug.Assert(task.Tag != null);

			task.SearchingPath = true;
			mActiveTasks++;

			ThreadPool.QueueUserWorkItem(FindPathThreadPoolCallback, task);
		}
		/// <summary>
		/// Finds a path on the current thread and returns.
		/// task.CompletedCallBack is ignored.
		/// </summary>
		/// <param name="task"></param>
		public void FindPathSync(AStarState<T> task)
		{
			try
			{
				task.SearchingPath = true;

				FindPath(task);
			}
			finally
			{
				task.SearchingPath = false;
			}
		}

		void FindPathThreadPoolCallback(object _task)
		{
			AStarState<T> task = null;

			try
			{
				task = (AStarState<T>)_task;

				FindPath(task);
				task.Complete = true;

				task.OnCompleted();
			}
			finally
			{
				task.SearchingPath = false;
				mActiveTasks--;
			}
		}

		public void FindPath(AStarState<T> task)
		{
			var openNodes = task.openNodes;
			var closedNodes = task.closedNodes;

			openNodes.Clear();
			closedNodes.Clear();
			task.Path.Clear();
			task.AbortOperation = false;

			var node = new AStarNode<T>
			{
				Location = task.Start,
				Parent = null,
				Heuristic = mMap.CalculateHeuristic(task.Start, task.EndPoints),
				PaidCost = 0
			};

			task.openNodes.Add(node);

			bool found = false;
			int steps = 0;

			do
			{
				SortOpenNodes(openNodes);

				//sMap.ReportProgress(openNodes, closedNodes, end);

				node = openNodes[0];
				openNodes.RemoveAt(0);
				closedNodes.Add(node);

				if (task.EndPoints.Contains(node.Location))
				{
					found = true;
					break;
				}

				steps++;
				if (steps > maxSteps)
					break;

				foreach (T test in mMap.GetAvailableSteps(task, node.Location))
				{
					if (mAbort)
						return;
					if (task.AbortOperation)
						return;

					if (LocationIn(closedNodes, test))
						continue;

					int deltaCost = mMap.GetStepCost(test, node.Location);

					int index = FindPointInOpenNodes(openNodes, test);

					if (index >= 0)
					{
						if (node.PaidCost + deltaCost < openNodes[index].PaidCost)
						{
							var target = openNodes[index];

							target.Parent = node;
							target.PaidCost = node.PaidCost + deltaCost;

							openNodes[index] = target;
						}
					}
					else
					{
						var newtarget = new AStarNode<T>
											{
												Location = test,
												Parent = node,
												PaidCost = node.PaidCost + deltaCost,
												Heuristic = mMap.CalculateHeuristic(test, task.EndPoints)
											};

						if (newtarget.Heuristic < 0)
						{
							continue;
						}

						openNodes.Add(newtarget);

						if (newtarget.Heuristic == 0)
						{
							node = newtarget;
							found = true;
						}
					}

					if (found)
						break;
				}

			} while (openNodes.Count > 0 && found == false);

			if (!found)
			{
				task.FoundPath = false;
				return;
			}

			task.Path.Add(node.Location);

			while (node.Parent != null && node.Parent != node)
			{
				if (mAbort)
					return;

				node = node.Parent;
				task.Path.Add(node.Location);
			}

			task.FoundPath = true;
		}

		int FindPointInOpenNodes(List<AStarNode<T>> openNodes, T location)
		{
			for (int i = 0; i < openNodes.Count; i++)
			{
				if (mEquals(openNodes[i].Location, location))
					return i;
			}

			return -1;
		}
		[Obsolete("Use LocationIn", true)]
		bool PointInOpenNodes(List<AStarNode<T>> openNodes, T location)
		{
			return LocationIn(openNodes, location);
		}
		[Obsolete("Use LocationIn", true)]
		bool PointInClosedNodes(List<AStarNode<T>> closedNodes, T location)
		{
			return LocationIn(closedNodes, location);
		}
		bool LocationIn(List<AStarNode<T>> nodeList, T location)
		{
			return nodeList.Any(x => mEquals(x.Location, location));
		}

		void SortOpenNodes(List<AStarNode<T>> openNodes)
		{
			openNodes.Sort((x, y) =>
			{
				int retval = x.TotalCost.CompareTo(y.TotalCost);
				if (retval == 0)
					retval = -x.PaidCost.CompareTo(y.PaidCost);
				return retval;
			});
		}

	}
}

