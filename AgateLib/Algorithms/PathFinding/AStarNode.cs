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
using AgateLib.Geometry;

namespace AgateLib.Algorithms.PathFinding
{
	/// <summary>
	/// Class which represents a node in an A* calulation.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class AStarNode<T>
	{
		public T Location;
		public AStarNode<T> Parent;
		
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