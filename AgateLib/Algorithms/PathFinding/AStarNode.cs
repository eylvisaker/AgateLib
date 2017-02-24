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

namespace AgateLib.Algorithms.PathFinding
{
	/// <summary>
	/// Class which represents a node in an A* calulation.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class AStarNode<T>
	{
		/// <summary>
		/// Location of the node.
		/// </summary>
		public T Location;

		/// <summary>
		/// The parent node.
		/// </summary>
		public AStarNode<T> Parent;
		
		/// <summary>
		/// The cost paid to reach this node.
		/// </summary>
		public int PaidCost;

		/// <summary>
		/// The cost estimated by the heuristic to reach the end.
		/// </summary>
		public int Heuristic;
		
		/// <summary>
		/// The total cost that a path passing through this node would be.
		/// </summary>
		public int TotalCost 
		{ 
			get { return PaidCost + Heuristic; }
		}

		/// <summary>
		/// Convert to a string for debug output.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0} : F={1} G={2} H={3}",
			                     Location, TotalCost, PaidCost, Heuristic);
		}
	}
}