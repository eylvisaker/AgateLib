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
using System.Collections.Generic;

namespace AgateLib.Algorithms.PathFinding
{
	/// <summary>
	/// This interface is used to provide the required communication between
	/// an area and the A* algorithm searching that area.
	/// </summary>
	/// <typeparam name="T"></typeparam>
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

		/// <summary>
		/// Return the available movements from the current location.
		/// </summary>
		/// <param name="task"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		IEnumerable<T> GetAvailableSteps(AStarState<T> task, T location);

		/// <summary>
		/// Gets the cost value for moving from start to target.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		int GetStepCost(T target, T start);
	}
}

