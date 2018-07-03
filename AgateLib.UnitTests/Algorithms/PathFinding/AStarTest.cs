using System;
using System.Collections.Generic;
using AgateLib.Algorithms.PathFinding;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Tests.Algorithms.PathFinding
{
    public class AStarTest
    {
        class FakeMap : IAStarMap<Point>
        {
            public void ReportProgress(AStarState<Point> task)
            {
            }

            public int CalculateHeuristic(Point location, Point destination)
            {
                return Math.Abs(destination.X - location.X) + Math.Abs(destination.Y - location.Y);
            }
            public int CalculateHeuristic(Point location, List<Point> destination)
            {
                int minval = int.MaxValue;

                foreach (var dest in destination)
                {
                    int val = CalculateHeuristic(location, dest);
                    if (val < minval) minval = val;
                }

                return minval;
            }

            public IEnumerable<Point> GetAvailableSteps(AStarState<Point> task, Point location)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        if (i == j || i == -j) continue;

                        Point trial = new Point(location.X + i, location.Y + j);

                        if (IsAvailable(trial))
                            yield return trial;
                    }
                }
            }

            private bool IsAvailable(Point trial)
            {
                if (trial.X < 0) return false;
                if (trial.Y < 0) return false;
                if (trial.X > 15) return false;
                if (trial.Y > 15) return false;

                // area in (3, 3) - (12, 12) is blocked
                if (trial.X < 3) return true;
                if (trial.Y < 3) return true;
                if (trial.X > 12) return true;
                if (trial.Y > 12) return true;

                return false;
            }

            public int GetStepCost(Point target, Point start)
            {
                return 1;
            }
        }

        [Fact]
        public void AStarPathSync()
        {
            AStarState<Point> state = new AStarState<Point>();
            state.Start = new Point(4, 2);
            state.EndPoints.Add(new Point(5, 15));

            var astar = new AStar<Point>(new FakeMap());

            astar.FindPathSync(state);

            // two steps to the left to get to (2, 2)
            // 13 steps down to get to (2, 15)
            // 3 steps to the right to get to (5, 15)
            // that's 18 steps, plus the start point makes 19.
            state.Path.Count.Should().Be(19);
        }

        [Fact]
        public void AStarPath()
        {
            AStarState<Point> state = new AStarState<Point>();
            state.Start = new Point(4, 2);
            state.EndPoints.Add(new Point(5, 15));

            var astar = new AStar<Point>(new FakeMap());

            var task = astar.FindPath(state);

            task.Wait();

            // two steps to the left to get to (2, 2)
            // 13 steps down to get to (2, 15)
            // 3 steps to the right to get to (5, 15)
            // that's 18 steps, plus the start point makes 19.
            state.Path.Count.Should().Be(19);
        }
    }
}
