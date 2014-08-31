//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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


		public async void QueueFindPath(AStarState<T> task)
		{
			System.Diagnostics.Debug.Assert(task.Tag != null);

			task.SearchingPath = true;
			mActiveTasks++;

			await FindPathThreadPoolCallback(task);
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

				FindPath(task).RunSynchronously();
			}
			finally
			{
				task.SearchingPath = false;
			}
		}

		async Task FindPathThreadPoolCallback(AStarState<T> task)
		{
			try
			{
				await FindPath(task);
				task.Complete = true;

				task.OnCompleted();
			}
			finally
			{
				task.SearchingPath = false;
				mActiveTasks--;
			}
		}

		public async Task FindPath(AStarState<T> state)
		{
			var openNodes = state.openNodes;
			var closedNodes = state.closedNodes;

			openNodes.Clear();
			closedNodes.Clear();
			state.Path.Clear();
			state.AbortOperation = false;

			var node = new AStarNode<T>
			{
				Location = state.Start,
				Parent = null,
				Heuristic = mMap.CalculateHeuristic(state.Start, state.EndPoints),
				PaidCost = 0
			};

			state.openNodes.Add(node);

			bool found = false;
			int steps = 0;

			do
			{
				SortOpenNodes(openNodes);

				//sMap.ReportProgress(openNodes, closedNodes, end);

				node = openNodes[0];
				openNodes.RemoveAt(0);
				closedNodes.Add(node);

				if (state.EndPoints.Contains(node.Location))
				{
					found = true;
					break;
				}

				steps++;
				if (steps > maxSteps)
					break;

				foreach (T test in mMap.GetAvailableSteps(state, node.Location))
				{
					if (mAbort)
						return;
					if (state.AbortOperation)
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
												Heuristic = mMap.CalculateHeuristic(test, state.EndPoints)
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
				state.FoundPath = false;
				return;
			}

			state.Path.Add(node.Location);

			while (node.Parent != null && node.Parent != node)
			{
				if (mAbort)
					return;

				node = node.Parent;
				state.Path.Add(node.Location);
			}

			state.FoundPath = true;
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

