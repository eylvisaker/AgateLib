using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Algorithms.PathFinding
{
	public class AStarTask
	{
		List<Point> mEndPoints = new List<Point>();
		List<Point> mPath = new List<Point>();

		public Point Start { get; set; }

		public List<Point> EndPoints
		{
			get { return mEndPoints; }
		}

		public List<Point> Path
		{
			get { return mPath; }
		}

		public List<AStarNode> openNodes = new List<AStarNode>();
		public List<AStarNode> closedNodes = new List<AStarNode>();

		public delegate void AStarTaskCompletedHandler(AStarTask task);

		public AStarTaskCompletedHandler CompletedCallBack { get; set; }

		public bool SearchingPath { get; set; }
		public bool Complete { get; set; }
		public bool FoundPath { get; set; }

		public object Tag { get; set; }

		public bool AbortOperation { get; set; }
	}
}
