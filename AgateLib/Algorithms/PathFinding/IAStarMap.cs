using System.Collections.Generic;
using AgateLib.Geometry;

namespace AgateLib.Algorithms.PathFinding
{
	public interface IAStarMap
	{
		void ReportProgress(AStarTask task);

		/// <summary>
		/// Calculate the heuristic for reaching the destination.
		/// If this method returns zero, the A* algorithm assumes
		/// this point is equivalent to the destination and ends
		/// successfully.
		/// If this method returns a negative number, the A* algorithm
		/// assumes that this point is invalid.
		/// </summary>
		/// <param name="location">
		/// A <see cref="Point"/>
		/// </param>
		/// <param name="destination">
		/// A <see cref="Point"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
		int CalculateHeuristic(Point location, List<Point> destination);
		IEnumerable<Point> GetAvailableSteps(AStarTask task, Point location);
		int GetStepCost(Point target, Point start);
	}
}

