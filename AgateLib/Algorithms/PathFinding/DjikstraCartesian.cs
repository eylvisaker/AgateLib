using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AgateLib.Algorithms.PathFinding
{
    public class DjikstraCartesian
    {
        private class DjikstraGrid
        {
            private readonly IDjikstraCartesianMap map;
            private DjikstraNode[] nodes;
            private Point topLeft;
            private int gridWidth;

            public DjikstraGrid(int maxDistance, IDjikstraCartesianMap map)
            {
                this.MaxDistance = maxDistance;
                this.map = map;
                gridWidth = maxDistance * 2 + 1;

                nodes = new DjikstraNode[gridWidth * gridWidth];
            }

            public DjikstraNode this[Point pt]
            {
                get
                {
                    Point local = pt - topLeft;

                    int index = local.Y * gridWidth + local.X;

                    return nodes[index];
                }
            }

            public IEnumerable<DjikstraNode> Nodes => nodes;

            public int MaxDistance { get; }

            public void Initialize(Point centerPoint)
            {
                topLeft = centerPoint - new Point(MaxDistance, MaxDistance);

                for (int i = 0; i < nodes.Length; i++)
                {
                    if (nodes[i] == null)
                        nodes[i] = new DjikstraNode();

                    nodes[i].Location = topLeft + new Point(i % gridWidth, i / gridWidth);
                    nodes[i].Visited = false;
                    nodes[i].Parent = null;
                    nodes[i].Distance = float.MaxValue;
                }
            }

            public DjikstraNode NextUnvisitedNode()
            {
                float lowest = float.MaxValue;
                int lowestIndex = -1;

                for (int i = 0; i < nodes.Length; i++)
                {
                    if (nodes[i].Visited)
                        continue;

                    if (nodes[i].Distance < lowest)
                    {
                        lowest = nodes[i].Distance;
                        lowestIndex = i;
                    }
                }

                if (lowestIndex < 0)
                    return null;

                return nodes[lowestIndex];
            }
            
            public IEnumerable<Point> NeighborsOf(Point location)
            {
                foreach (var pt in map.GetAvailableSteps(location))
                {
                    if (pt.X < topLeft.X)
                        continue;
                    if (pt.Y < topLeft.Y)
                        continue;
                    if (pt.X >= topLeft.X + gridWidth)
                        continue;
                    if (pt.Y >= topLeft.Y + gridWidth)
                        continue;

                    if (this[pt].Enterable)
                    {
                        yield return pt;
                    }
                }
            }
        }

        private readonly IDjikstraCartesianMap map;
        private DjikstraNode[,] nodes;
        private DjikstraGrid grid;

        public DjikstraCartesian(IDjikstraCartesianMap map)
        {
            this.map = map;
        }

        public Point Start { get; set; }

        public int MaxDistance { get; set; }

        public IEnumerable<DjikstraNode> Nodes => grid.Nodes;

        /// <summary>
        /// Finds all points that can be reached.
        /// </summary>
        public void FindReachable()
        {
            if (grid == null || grid.MaxDistance < MaxDistance)
            {
                grid = new DjikstraGrid(MaxDistance, map);
            }

            grid.Initialize(Start);
            grid[Start].Distance = 0;

            MarkEnterableNodes();

            DjikstraNode node;

            while ((node = grid.NextUnvisitedNode()) != null)
            {
                var location = node.Location;
                node.Visited = true;

                foreach (Point neighbor in grid.NeighborsOf(location))
                {
                    var nn = grid[neighbor];

                    Point delta = neighbor - location;
                    float stepSize = delta.ToVector2().Length();

                    float altDist = node.Distance + stepSize;

                    if (altDist < nn.Distance)
                    {
                        nn.Distance = altDist;
                        nn.Parent = node;
                    }
                }
            }
        }

        private void MarkEnterableNodes()
        {
            foreach (DjikstraNode node in grid.Nodes)
            {
                node.Enterable = map.CanEnter(node.Location);
            }
        }

        private void Swap<P>(ref P a, ref P b)
        {
            P c = b;
            b = a;
            a = c;
        }
    }

    public class DjikstraNode
    {
        public float Distance { get; internal set; }
        public DjikstraNode Parent { get; internal set; }
        public bool Visited { get; internal set; }
        public Point Location { get; internal set; }
        public bool Enterable { get; internal set; }

        public override string ToString() => $"({Location.X},{Location.Y}) - V:{Visited} - E:{Enterable} - D:{Distance}";
    }
}
