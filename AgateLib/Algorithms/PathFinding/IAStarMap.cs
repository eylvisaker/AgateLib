using System.Collections.Generic;
using AgateLib.Geometry;

namespace AgateLib.Algorithms.PathFinding
{
	public interface IAStarMap<T>
	{
		void ReportProgress(AStarState<T> task);

		/// <summary>
		/// Calculate the heuristic for reaching the destination.
		/// If this method returns zero, the A* algorithm assumes
		/// this point is equivalent to the destination and ends
		/// successfully.
		/// If this method returns a negative number, the A* algorithm
		/// assumes that this point is invalid.
		/// </summary>
		/// <param name="location"></param>
		/// <param name="destination"></param>
		/// <returns></returns>
		int CalculateHeuristic(T location, List<T> destination);
		IEnumerable<T> GetAvailableSteps(AStarState<T> task, T location);
		int GetStepCost(T target, T start);
	}
}

