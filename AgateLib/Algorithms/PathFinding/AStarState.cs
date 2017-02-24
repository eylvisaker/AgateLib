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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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
