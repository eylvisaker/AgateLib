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
using System.Collections.Generic;
using AgateLib.Geometry;
using System.Diagnostics.Contracts;

namespace AgateLib.Algorithms.PathFinding
{
	[ContractClass(typeof(AStarMapContract<>))]
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

