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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgateLib.Algorithms.PathFinding
{
    /// <summary>
    /// Class which implements the A* path finding algorithm.
    /// </summary>
    /// <typeparam name="T">A type which represents a point on a map.</typeparam>
    public class AStar<T>
    {
        /// <summary>
        /// Class which represents a node in an A* calulation.
        /// </summary>
        /// <typeparam name="Tnode"></typeparam>
        public class AStarNode<Tnode>
        {
            /// <summary>
            /// Location of the node.
            /// </summary>
            public Tnode Location;

            /// <summary>
            /// The parent node.
            /// </summary>
            public AStarNode<Tnode> Parent;

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

        private const int maxSteps = 200;
        private IAStarMap<T> map;
        private Func<T, T, bool> equals;
        private List<T> path = new List<T>();

        private List<AStarNode<T>> openNodes = new List<AStarNode<T>>();
        private List<AStarNode<T>> closedNodes = new List<AStarNode<T>>();

        /// <summary>
        /// Constructs an A* algorithm object.
        /// </summary>
        /// <param name="map"></param>
        public AStar(IAStarMap<T> map)
            : this(map, (x, y) => x.Equals(y))
        { }

        /// <summary>
        /// Constructs an A* algorithm object.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="comparer"></param>
        public AStar(IAStarMap<T> map, IEqualityComparer<T> comparer)
            : this(map, comparer.Equals)
        { }

        /// <summary>
        /// Constructs an A* algorithm object.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="comparison"></param>
        public AStar(IAStarMap<T> map, Func<T, T, bool> comparison)
        {
            this.map = map;
            equals = comparison;
        }

        /// <summary>
        /// The starting point.
        /// </summary>
        public T Start { get; set; }

        /// <summary>
        /// The list of acceptable end points.
        /// </summary>
        public List<T> EndPoints { get; } = new List<T>();

        /// <summary>
        /// The resulting path.
        /// </summary>
        public List<T> Path => path;
        
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
        /// Set to true to force the path finding algorithm to gracefully abort.
        /// </summary>
        public bool AbortOperation { get; set; }

        /// <summary>
        /// Finds a path on the current thread and returns.
        /// task.CompletedCallBack is ignored.
        /// </summary>
        /// <param name="state"></param>
        public void FindPath()
        {
            if (EndPoints.Count == 0)
                throw new InvalidOperationException($"There must be at least one end point.");

            Complete = false;

            try
            {
                SearchingPath = true;

                FindPathExec();
            }
            finally
            {
                SearchingPath = false;
            }

            Complete = true;
        }
        
        private void FindPathExec()
        {
            openNodes.Clear();
            closedNodes.Clear();
            path.Clear();
            AbortOperation = false;

            var node = new AStarNode<T>
            {
                Location = Start,
                Parent = null,
                Heuristic = map.CalculateHeuristic(Start, EndPoints),
                PaidCost = 0
            };

            openNodes.Add(node);

            bool found = false;
            int steps = 0;

            do
            {
                SortOpenNodes(openNodes);

                //sMap.ReportProgress(openNodes, closedNodes, end);

                node = openNodes[0];
                openNodes.RemoveAt(0);
                closedNodes.Add(node);

                if (EndPoints.Contains(node.Location))
                {
                    found = true;
                    break;
                }

                steps++;
                if (steps > maxSteps)
                    break;

                foreach (T test in map.GetAvailableSteps(node.Location))
                {
                    if (AbortOperation)
                        return;

                    if (LocationIn(closedNodes, test))
                        continue;

                    int deltaCost = map.GetStepCost(test, node.Location);

                    int index = FindPointInOpenNodes(openNodes, test);

                    if (index >= 0)
                    {
                        if (node.PaidCost + deltaCost < openNodes[index].PaidCost)
                        {
                            var target = openNodes[index];

                            target.Parent = node;
                            target.PaidCost = node.PaidCost + deltaCost;

                            openNodes[index] = target;
                        }
                    }
                    else
                    {
                        var newtarget = new AStarNode<T>
                        {
                            Location = test,
                            Parent = node,
                            PaidCost = node.PaidCost + deltaCost,
                            Heuristic = map.CalculateHeuristic(test, EndPoints)
                        };

                        if (newtarget.Heuristic < 0)
                        {
                            continue;
                        }

                        openNodes.Add(newtarget);

                        if (newtarget.Heuristic == 0)
                        {
                            node = newtarget;
                            found = true;
                        }
                    }

                    if (found)
                        break;
                }

            } while (openNodes.Count > 0 && found == false);

            if (!found)
            {
                FoundPath = false;
                return;
            }

            var resultPath = new List<T>();

            resultPath.Add(node.Location);

            while (node.Parent != null && node.Parent != node)
            {
                node = node.Parent;
                resultPath.Add(node.Location);
            }

            path.AddRange(Enumerable.Reverse(resultPath));
            FoundPath = true;
        }

        private int FindPointInOpenNodes(List<AStarNode<T>> openNodes, T location)
        {
            for (int i = 0; i < openNodes.Count; i++)
            {
                if (equals(openNodes[i].Location, location))
                    return i;
            }

            return -1;
        }

        private bool LocationIn(List<AStarNode<T>> nodeList, T location)
        {
            return nodeList.Any(x => equals(x.Location, location));
        }

        private void SortOpenNodes(List<AStarNode<T>> openNodes)
        {
            openNodes.Sort((x, y) =>
            {
                int result = x.TotalCost.CompareTo(y.TotalCost);
                if (result == 0)
                    result = -x.PaidCost.CompareTo(y.PaidCost);
                return result;
            });
        }

    }
}

