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
using AgateLib.Randomizer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib
{
    public interface IRandom
    {
        /// <summary>
        /// Gets the seed value.
        /// </summary>
        long Seed { get; }

        /// <summary>
        /// Gets the maximum value that can be returned by NextInteger().
        /// </summary>
        int NextIntegerMaxValue { get; }

        /// <summary>
        /// Gets the next random integer in the sequence, between zero and NextIntegerMaxValue.
        /// </summary>
        /// <returns></returns>
        int NextInteger();

        /// <summary>
        /// Gets the next random single precision floating point in the sequence, between zero and 1.
        /// </summary>
        /// <returns></returns>
        float NextSingle();

        /// <summary>
        /// Gets the next double precision floating point in the sequence, between zero and 1.
        /// </summary>
        /// <returns></returns>
        double NextDouble();
    }

    public static class RandomX
    {
        /// <summary>
        /// Gets a seed value from the system clock.
        /// </summary>
        /// <returns></returns>
        public static int GetSeedFromSystemClock() => Math.Abs((int)DateTime.Now.Ticks);

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
        /// Gets the next random single value which is greater than zero and less than or equal to
        /// the specified maxmimum value.
        /// </summary>
        /// <param name="max">The maximum random single value to return.</param>
        /// <returns>A random single value between zero and the specified maximum value.</returns>
        public static double NextDouble(this IRandom random, double max) => max * random.NextDouble();

        /// <summary>
        /// Gets the next random single value between the specified minimum and maximum values.
        /// </summary>
        /// <param name="min">The inclusive minimum value.</param>
        /// <param name="max">The inclusive maximum value.</param>
        /// <returns>A random single value between the specified minimum and maximum values.</returns>
        public static double NextDouble(this IRandom random, double min, double max) => ((max - min) * random.NextDouble()) + min;

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

        /// <summary>
        /// Picks a random item from a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T PickOne<T>(this IRandom random, IReadOnlyList<T> items) => items[random.NextInteger(items.Count)];

        /// <summary>
        /// Picks a random point within the specified rectangle.
        /// </summary>
        /// <param name="random">The IRandom object which provides random numbers</param>
        /// <param name="rect">The bounding rectangle.</param>
        /// <returns></returns>
        public static Vector2 NextVector2InRect(this IRandom random, Rectangle rect)
        {
            return new Vector2(
                random.NextSingle(rect.Left, rect.Right),
                random.NextSingle(rect.Top, rect.Bottom)
                );
        }

        /// <summary>
        /// Picks a random item from a list. Items weighted more heavily are more likely to be picked.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random"></param>
        /// <param name="items"></param>
        /// <param name="weightFunc">A function that when called for each object in the list returns its weight.</param>
        /// <returns></returns>
        public static T PickOneWeighted<T>(this IRandom random, IReadOnlyList<T> items, Func<T, int> weightFunc)
        {
            int max = items.Sum(x => weightFunc(x));
            int initialVal = random.NextInteger(max);
            int val = initialVal;

            for (int i = 0; i < items.Count; i++)
            {
                val -= weightFunc(items[i]);

                if (val < 0)
                    return items[i];
            }

            return default(T);
        }

        /// <summary>
        /// Creates a new IRandom object with a seed set by the system clock.
        /// </summary>
        /// <returns></returns>
        public static IRandom Create()
        {
            return Create(GetSeedFromSystemClock());
        }

        /// <summary>
        /// Creates a new IRandom object with a specified seed.
        /// </summary>
        /// <returns></returns>
        public static IRandom Create(long seed)
        {
            if (RandomFactory == null)
                return new FastRandom((int)seed);

            return RandomFactory(seed);
        }

        /// <summary>
        /// Set to specify a custom random factory method.
        /// </summary>
        public static Func<long, IRandom> RandomFactory { get; set; }
    }
}

