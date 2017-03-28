//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Algorithms.PathFinding
{
	/// <summary>
	/// Class which represents the current state of the A* algorithm search in progress.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class AStarState<T>
	{
		List<T> mEndPoints = new List<T>();
		List<T> mPath = new List<T>();

		internal List<AStarNode<T>> openNodes = new List<AStarNode<T>>();
		internal List<AStarNode<T>> closedNodes = new List<AStarNode<T>>();


		/// <summary>
		/// The starting point.
		/// </summary>
		public T Start { get; set; }

		/// <summary>
		/// The list of acceptable end points.
		/// </summary>
		public List<T> EndPoints
		{
			get { return mEndPoints; }
		}

		/// <summary>
		/// The resulting path.
		/// </summary>
		public List<T> Path
		{
			get { return mPath; }
		}

		/// <summary>
		/// Delegate to invoke when the path finding is complete.
		/// </summary>
		[Obsolete("This should move to an async/await model.")]
		internal void OnCompleted()
		{
			Completed?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Event invoked when the path finding is complete.
		/// </summary>
		[Obsolete("This should move to an async/await model.")]
		public event EventHandler Completed;
		
		/// <summary>
		/// True if currently searching for the path.
		/// </summary>
		public bool SearchingPath { get; set; }

		/// <summary>
		/// True if the algorithm finished.
		/// </summary>
		public bool Complete { get; set; }

		/// <summary>
		/// Trus if a path was found.
		/// </summary>
		public bool FoundPath { get; set; }

		/// <summary>
		/// A user-defined tag object.
		/// </summary>
		[Obsolete("This should be replaced with closures in an async/await model.")]
		public object Tag { get; set; }

		/// <summary>
		/// Set to true to force the path finding to abort.
		/// </summary>
		public bool AbortOperation { get; set; }
	}
}
