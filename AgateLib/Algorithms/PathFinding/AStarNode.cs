//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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