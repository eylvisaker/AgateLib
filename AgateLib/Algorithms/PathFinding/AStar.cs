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

		Func<T, T, bool> mEquals;

		public AStar(IAStarMap<T> map)
			: this(map, (x, y) => x.Equals(y))
		{ }
		public AStar(IAStarMap<T> map, IEqualityComparer<T> comparer)
			: this(map, comparer.Equals)
		{ }
		public AStar(IAStarMap<T> map, Func<T, T, bool> comparison)
		{
			mMap = map;
			mEquals = comparison;
		}

		/// <summary>
		/// Finds a path on the current thread and returns.
		/// task.CompletedCallBack is ignored.
		/// </summary>
		/// <param name="task"></param>
		public void FindPathSync(AStarState<T> state)
		{
			try
			{
				state.SearchingPath = true;

				FindPathExec(state);
			}
			finally
			{
				state.SearchingPath = false;
			}
		}

		public async Task FindPath(AStarState<T> state)
		{
			try
			{
				state.SearchingPath = true;
				await Task.Run(() => FindPathExec(state)).ConfigureAwait(false);

			}
			finally
			{
				state.SearchingPath = false;
			}

			state.Complete = true;
		}

		void FindPathExec(AStarState<T> state)
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

			var path = new List<T>();

			path.Add(node.Location);

			while (node.Parent != null && node.Parent != node)
			{
				node = node.Parent;
				path.Add(node.Location);
			}

			state.Path.AddRange(path);
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

		bool LocationIn(List<AStarNode<T>> nodeList, T location)
		{
			return nodeList.Any(x => mEquals(x.Location, location));
		}

		void SortOpenNodes(List<AStarNode<T>> openNodes)
		{
			openNodes.Sort((x, y) =>
			{
				int result = x.TotalCost.CompareTo(y.TotalCost);
				if (result == 0)
					result = -x.PaidCost.CompareTo(y.PaidCost);
				return result;
			});
		}

	}
}

