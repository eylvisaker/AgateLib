using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AgateLib.Geometry;
using System.Threading.Tasks;

namespace AgateLib.Algorithms.PathFinding
{
	public static class AStar
	{
		const int maxSteps = 200;
		static IAStarMap sMap;
		static bool mAbort;
		static int mActiveTasks;

		[Obsolete("Do something about this. It should be replaced by the async implementation.")]
		public static void SetMap(IAStarMap map)
		{
			mAbort = true;

			while (mActiveTasks > 0)
			{
				//Thread.Sleep(0);
				Core.KeepAlive();
			}

			sMap = map;

			mAbort = false;
		}

		
		public static void QueueFindPath(AStarTask task)
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
		public static void FindPathSync(AStarTask task)
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

		static void FindPathThreadPoolCallback(object _task)
		{
			AStarTask task = null;

			try
			{
				task = (AStarTask)_task;

				FindPath(task);
				task.Complete = true;

				if (task.CompletedCallBack != null)
				{
					task.CompletedCallBack(task);
				}
			}
			finally
			{
				task.SearchingPath = false;
				mActiveTasks--;
			}
		}

		static void FindPath(AStarTask task)
		{
			List<AStarNode> openNodes = task.openNodes;
			List<AStarNode> closedNodes = task.closedNodes;

			openNodes.Clear();
			closedNodes.Clear();
			task.Path.Clear();
			task.AbortOperation = false;

			var node = new AStarNode
			{
				Location = task.Start, 
				Parent = null, 
				Heuristic = sMap.CalculateHeuristic(task.Start, task.EndPoints), 
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
				
				steps ++;
				if (steps > maxSteps)
					break;
				
				foreach(Point test in sMap.GetAvailableSteps(task, node.Location))
				{
					if (mAbort)
						return;
					if (task.AbortOperation)
						return;

					if (PointInClosedNodes(closedNodes, test))
						continue;
					
					int deltaCost = sMap.GetStepCost(test, node.Location);
					
					int index = FindPointInOpenNodes(openNodes, test);
						
					if (index >= 0)
					{
						if (node.PaidCost + deltaCost < openNodes[index].PaidCost)
						{
							AStarNode target = openNodes[index];
							
							target.Parent = node;
							target.PaidCost = node.PaidCost + deltaCost;

							openNodes[index] = target;
						}
					}
					else
					{
						var newtarget = new AStarNode
						                	{
						                		Location = test,
						                		Parent = node,
						                		PaidCost = node.PaidCost + deltaCost,
						                		Heuristic = sMap.CalculateHeuristic(test, task.EndPoints)
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

		static int FindPointInOpenNodes(List<AStarNode> openNodes, Point location)
		{
			for(int i = 0; i < openNodes.Count; i++)
			{
				if (openNodes[i].Location == location)
					return i;
			}
			
			return -1;
		}
		static bool PointInOpenNodes(List<AStarNode> openNodes, Point location)
		{
			return openNodes.Any(x => x.Location == location);	
		}
		static bool PointInClosedNodes(List<AStarNode> closedNodes, Point location)
		{
			return closedNodes.Any(x => x.Location == location);
		}
		static void SortOpenNodes(List<AStarNode> openNodes)
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

