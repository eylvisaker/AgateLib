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
using System.Text;
using AgateLib.Geometry;
using System.Threading.Tasks;

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

		internal List<AStarNode<T>> openNodes = new List<AStarNode<T>>();
		internal List<AStarNode<T>> closedNodes = new List<AStarNode<T>>();

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
