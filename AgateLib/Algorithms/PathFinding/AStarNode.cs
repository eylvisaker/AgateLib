using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Algorithms.PathFinding
{
    /// <summary>
    /// Class which represents a node in an A* calulation.
    /// </summary>
    /// <typeparam name="T">The type which represents a node.</typeparam>
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
        /// Weight of the Heuristic in the calculation.
        /// </summary>
        public double HeuristicWeight = 1;

        /// <summary>
        /// The total cost that a path passing through this node would be.
        /// </summary>
        public int TotalCost
            => PaidCost + (int)(Heuristic * HeuristicWeight);

        /// <summary>
        /// Convert to a string for debug output.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string heuristicWeight = HeuristicWeight != 1 ? $" * {HeuristicWeight}" : "";

            return $"G={PaidCost} + H={Heuristic}{heuristicWeight} = {TotalCost} @ {Location}";
        }
    }

}
