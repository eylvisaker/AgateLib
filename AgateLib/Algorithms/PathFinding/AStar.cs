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

using AgateLib.Quality;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Algorithms.PathFinding
{
    /// <summary>
    /// Class which implements the A* path finding algorithm.
    /// </summary>
    /// <typeparam name="T">A type which represents a point on a map.</typeparam>
    public class AStar<T>
    {
        public const int DefaultMaxSteps = 200;

        private IAStarMap<T> map;
        private Func<T, T, bool> equals;
        private List<T> path = new List<T>();

        private List<AStarNode<T>> openNodes = new List<AStarNode<T>>();
        private List<AStarNode<T>> closedNodes = new List<AStarNode<T>>();
        private List<T> _endPoints = new List<T>();

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

        public event AStarProgressHandler<T> AStarProgress;

        /// <summary>
        /// The starting point.
        /// </summary>
        public T Start { get; set; }

        /// <summary>
        /// The list of acceptable end points.
        /// </summary>
        public List<T> EndPoints
        {
            get => _endPoints;
            set
            {
                Require.ArgumentNotNull(value, nameof(EndPoints));
                _endPoints = value;
            }
        }

        /// <summary>
        /// The resulting path.
        /// </summary>
        public List<T> Path => path;

        /// <summary>
        /// Gets or sets the default number of steps the A* algorithm will take.
        /// Defaults to the value of DefaultMaxSteps.
        /// </summary>
        public int MaxSteps { get; set; } = DefaultMaxSteps;

        /// <summary>
        /// Gets or sets the default weight for the heuristic in the calculation.
        /// Higher values will bias the algorithm to search nodes closer to the end point.
        /// This can result in faster searches in getting around small obstacles but 
        /// slower searches to find ways around large obstacles.
        /// Defaults to 0. Negative values may have bizarre effects.
        /// </summary>
        public double HeuristicWeight { get; set; }

        /// <summary>
        /// True if currently searching for the path.
        /// </summary>
        public bool SearchingPath { get; private set; }

        /// <summary>
        /// True if the algorithm finished.
        /// </summary>
        public bool Complete { get; private set; }

        /// <summary>
        /// Trus if a path was found.
        /// </summary>
        public bool FoundPath { get; private set; }

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
                HeuristicWeight = (1 + HeuristicWeight),
                PaidCost = 0
            };

            node.Heuristic = map.CalcHeuristic(node, EndPoints);

            openNodes.Add(node);

            bool found = false;
            int steps = 0;

            do
            {
                SortOpenNodes(openNodes);

                AStarProgress?.Invoke(openNodes, closedNodes);

                node = openNodes[0];
                openNodes.RemoveAt(0);
                closedNodes.Add(node);

                if (EndPoints.Contains(node.Location))
                {
                    found = true;
                    break;
                }

                steps++;
                if (steps > MaxSteps)
                    break;

                foreach (T test in map.AvailableStepsAt(node))
                {
                    if (AbortOperation)
                        return;

                    if (LocationIn(closedNodes, test))
                        continue;

                    int deltaCost = map.StepCost(node, test);

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
                            HeuristicWeight = 1 + HeuristicWeight,
                        };

                        newtarget.Heuristic = map.CalcHeuristic(newtarget, EndPoints);

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

    public delegate void AStarProgressHandler<T>(List<AStarNode<T>> openNodes, List<AStarNode<T>> closedNodes);
}

