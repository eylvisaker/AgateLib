using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Algorithms.PathFinding
{
    public interface IDjikstraCartesianMap
    {
        bool CanEnter(Point location);

        /// <summary>
        /// Return the available movements from the current location.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        IEnumerable<Point> GetAvailableSteps(Point location);

    }
}
