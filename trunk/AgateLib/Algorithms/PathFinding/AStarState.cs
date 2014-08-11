using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Algorithms.PathFinding
{
	public class AStarState<T>
	{
		List<T> mEndPoints = new List<T>();
		List<T> mPath = new List<T>();

		public T Start { get; set; }

		public List<T> EndPoints
		{
			get { return mEndPoints; }
		}

		public List<T> Path
		{
			get { return mPath; }
		}

		public List<AStarNode<T>> openNodes = new List<AStarNode<T>>();
		public List<AStarNode<T>> closedNodes = new List<AStarNode<T>>();

		internal void OnCompleted()
		{
			if (Completed != null)
				Completed(this, EventArgs.Empty);
		}
		public event EventHandler Completed;
		
		public bool SearchingPath { get; set; }
		public bool Complete { get; set; }
		public bool FoundPath { get; set; }

		public object Tag { get; set; }

		public bool AbortOperation { get; set; }
	}
}
