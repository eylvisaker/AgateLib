using AgateLib.Geometry;

namespace AgateLib.Algorithms.PathFinding
{
	public class AStarNode 
	{
		public Point Location;
		public AStarNode Parent;
		
		public int PaidCost;
		public int Heuristic;
		
		public int TotalCost 
		{ 
			get { return PaidCost + Heuristic; }
		}

		public override string ToString()
		{
			return string.Format("{0} : F={1} G={2} H={3}",
			                     Location, TotalCost, PaidCost, Heuristic);
		}
	}
}