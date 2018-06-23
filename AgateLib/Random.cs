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

using AgateLib.Mathematics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib
{
    public interface IRandom
    {
        /// <summary>
        /// Gets the next random integer in the sequence.
        /// </summary>
        /// <returns></returns>
        int NextInteger();

        /// <summary>
        /// Gets the next random single precision floating point in the sequence.
        /// </summary>
        /// <returns></returns>
        float NextSingle();
    }

    public static class RandomExtensions
    {
        /// <summary>
        /// Gets the next random integer value which is greater than zero and less than or equal to
        /// the specified maxmimum value.
        /// </summary>
        /// <param name="max">The maximum random integer value to return.</param>
        /// <returns>A random integer value between zero and the specified maximum value.</returns>
        public static int NextInteger(this IRandom random, int max) => (int)(max * random.NextSingle());

        /// <summary>
        /// Gets the next random integer between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The inclusive minimum value.</param>
        /// <param name="max">The inclusive maximum value.</param>
        public static int NextInteger(this IRandom random, int min, int max) => (int)((max - min) * random.NextSingle()) + min;

        /// <summary>
        /// Gets the next random single value which is greater than zero and less than or equal to
        /// the specified maxmimum value.
        /// </summary>
        /// <param name="max">The maximum random single value to return.</param>
        /// <returns>A random single value between zero and the specified maximum value.</returns>
        public static float NextSingle(this IRandom random, float max) => max * random.NextSingle();

        /// <summary>
        /// Gets the next random single value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The inclusive minimum value.</param>
        /// <param name="max">The inclusive maximum value.</param>
        /// <returns>A random single value between the specified minimum and maximum values.</returns>
        public static float NextSingle(this IRandom random, float min, float max) => ((max - min) * random.NextSingle()) + min;

        /// <summary>
        /// Gets the next random angle value, in the range of -PI to PI
        /// </summary>
        /// <returns>A random angle value.</returns>
        public static float NextAngle(this IRandom random) => random.NextSingle(MathX.PI * -1f, MathX.PI);

        /// <summary>
        /// Gets the next random unit vector.
        /// </summary>
        /// <param name="random"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static Vector2 NextUnitVector(this IRandom random) => Vector2X.FromPolar(1, random.NextAngle());
    }
}
