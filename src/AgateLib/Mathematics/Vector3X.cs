﻿//
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

using Microsoft.Xna.Framework;
using System;

namespace AgateLib.Mathematics
{
    public static class Vector3X
    {
        /// <summary>
        /// Using standard cylindrical coordinates.
        /// </summary>
        /// <remarks>See https://en.wikipedia.org/wiki/Cylindrical_coordinate_system for more information.</remarks>
        /// <param name="radialDist">The radial distance in the x-y plane</param>
        /// <param name="z">The distance in the z direction</param>
        /// <param name="azimuthalAngle">The azimuthal angle in radians.</param>
        /// <returns></returns>
        public static Vector3 FromCylindrical(float radialDist, float z, float azimuthalAngle)
        {
            return new Vector3(
                radialDist * MathF.Cos(azimuthalAngle),
                radialDist * MathF.Sin(azimuthalAngle),
                z);
        }

        /// <summary>
        /// Using standard spherical coordinates.
        /// </summary>
        /// <remarks>See https://en.wikipedia.org/wiki/Spherical_coordinate_system#Cartesian_coordinates for more information.</remarks>
        /// <param name="radialDist">The radial distance</param>
        /// <param name="polarAngle">The polar angle in radians.</param>
        /// <param name="azimuthalAngle">The azimuthal angle in radians.</param>
        /// <returns></returns>
        public static Vector3 FromSpherical(float radialDist, float polarAngle, float azimuthalAngle)
        {
            return new Vector3(
                radialDist * MathF.Sin(polarAngle) * MathF.Cos(azimuthalAngle),
                radialDist * MathF.Sin(polarAngle) * MathF.Sin(azimuthalAngle),
                radialDist * MathF.Cos(polarAngle));
        }

        /// <summary>
        /// Performs equality comparison to within a tolerance value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool Equals(Vector3 a, Vector3 b, float tolerance)
        {
            return Math.Abs(a.X - b.X) < tolerance &&
                   Math.Abs(a.Y - b.Y) < tolerance &&
                   Math.Abs(a.Z - b.Z) < tolerance;
        }
    }
}
