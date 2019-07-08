using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgateLib.Algorithms.PathFinding;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace AgateLib.Algorithms.PathFinding
{
    public class DjikstraTest
    {
        /// <summary>
        /// A map which only allows movement along Cartesian axes.
        /// </summary>
        class FakeDjikstraMap : IDjikstraCartesianMap
        {
            public IEnumerable<Point> GetAvailableSteps(Point location)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        if (i == j || i == -j) continue;

                        Point trial = new Point(location.X + i, location.Y + j);

                        if (CanEnter(trial))
                            yield return trial;
                    }
                }
            }

            public bool CanEnter(Point location)
            {
                if (location.X < 0) return false;
                if (location.Y < 0) return false;
                if (location.X > 15) return false;
                if (location.Y > 15) return false;

                // rectangle from (3, 3) - (12, 12) is blocked
                if (location.X < 3) return true;
                if (location.Y < 3) return true;
                if (location.X > 12) return true;
                if (location.Y > 12) return true;

                return false;
            }

            public int GetStepCost(Point target, Point start)
            {
                return 1;
            }
        }

        [Fact]
        public void DjikstraPathCompletes()
        {
            var djikstra = new DjikstraCartesian(new FakeDjikstraMap());

            djikstra.Start = new Point(4, 2);
            djikstra.MaxDistance = 4;

            djikstra.FindReachable();

            var results = djikstra.ReachableLocations.ToList();

            var validResults = new[]
            {
                new Point(0, 2), new Point(2, 0), new Point(8, 2), new Point(5, 2), new Point(2, 4)
            };
            var invalidResults = new[]
            {
                new Point(0, 1), new Point(1, 0), new Point(9, 2), new Point(3, 3), new Point(4, 7)
            };
            
            foreach(var pt in validResults)
            {
                results.Should().Contain(pt);
            }

            foreach(var pt in invalidResults)
            {
                results.Should().NotContain(pt);
            }
        }
    }
}
