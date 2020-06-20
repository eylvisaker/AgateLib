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
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AgateLib.Randomizer
{
    public interface IRandom
    {
        /// <summary>
        /// Gets the seed value.
        /// </summary>
        Seed Seed { get; }

        /// <summary>
        /// Gets the maximum value that can be returned by NextUInt32().
        /// </summary>
        uint NextUInt32MaxValue { get; }

        /// <summary>
        /// Gets the next random integer in the sequence, between 0 and UInt32.MaxValue
        /// </summary>
        /// <returns></returns>
        uint NextUInt32();

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

        /// <summary>
        /// Generates a seed value that can be used for another sequence of random numbers.
        /// </summary>
        /// <returns></returns>
        Seed GenerateSeed();

        /// <summary>
        /// Generates a new random number generator. Calling 
        /// <code>random.Spawn()</code>
        /// is equivalent to 
        /// <code>new RandomType(random.GenerateSeed)</code>
        /// </summary>
        /// <returns></returns>
        IRandom Spawn();
    }

    public static class RandomX
    {
        /// <summary>
        /// Gets a seed value from the system clock.
        /// </summary>
        /// <returns></returns>
        public static Seed GetSeedFromSystemClock() => new Seed((ulong)DateTime.Now.Ticks, (ulong)DateTime.Now.Ticks ^ 0xdeadba5eba11beef);

        /// <summary>
        /// Gets the next random integer in the sequence, between zero and Int32.MaxValue.
        /// </summary>
        /// <returns></returns>
        public static int NextInteger(this IRandom random) => random.NextInteger(0, int.MaxValue);

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
        /// Gets a random unit vector.
        /// </summary>
        /// <param name="random"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static Vector2 NextUnitVector(this IRandom random) => Vector2X.FromPolar(1, random.NextAngle());

        /// <summary>
        /// Gets a random point.
        /// </summary>
        /// <param name="random"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static Point NextPoint(this IRandom random, Rectangle bounds)
            => new Point(random.NextInteger(bounds.Left, bounds.Right),
                         random.NextInteger(bounds.Top, bounds.Bottom));

        /// <summary>
        /// Picks a random point within the specified rectangle.
        /// </summary>
        /// <param name="random">The IRandom object which provides random numbers</param>
        /// <param name="rect">The bounding rectangle.</param>
        /// <returns></returns>
        public static Vector2 NextVector2(this IRandom random, Rectangle rect)
        {
            return new Vector2(
                random.NextSingle(rect.Left, rect.Right),
                random.NextSingle(rect.Top, rect.Bottom)
                );
        }

        [Obsolete("Use NextVector2 instead.", true)]
        public static Vector2 NextVector2InRect(this IRandom random, Rectangle rect) => NextVector2(random, rect);

        /// <summary>
        /// Picks a random item from a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T PickOne<T>(this IRandom random, IReadOnlyCollection<T> items)
        {
            Require.ArgumentInRange(items.Count > 0, nameof(items), "No items in collection");

            if (items is IReadOnlyList<T> list)
            {
                return list[random.NextInteger(list.Count)];
            }

            return items.Skip(random.NextInteger(items.Count)).First();
        }

        /// <summary>
        /// Picks a random item from a list. Items weighted more heavily are more likely to be picked.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="random">The IRandom object to use as a source of random numbers.</param>
        /// <param name="items">The list of items to pick from.</param>
        /// <param name="weightFunc">A function that when called for each object in the list returns its weight.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T PickOneWeighted<T>(this IRandom random, IReadOnlyCollection<T> items, Func<T, int> weightFunc)
        {
            Require.ArgumentInRange(items.Count > 0, nameof(items), "No items in collection");

            int max = 0;
            int index = 0;

            foreach (var item in items)
            {
                int weight = weightFunc(item);
                max += weight;

                Require.That(weight >= 0, $"Weight of item {index} is negative. All weights must be non-negative.");

                index++;
            }

            Require.That(max > 0, "Sum of weights must be positive. All weights were zero");

            int initialVal = random.NextInteger(max);
            int val = initialVal;

            foreach (var item in items)
            {
                val -= weightFunc(item);

                if (val < 0)
                {
                    return item;
                }
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
        public static IRandom Create(Seed seed)
        {
            if (RandomFactory == null)
            {
                return new Xoroshiro128pp(seed);
            }

            return RandomFactory(seed);
        }

        /// <summary>
        /// Creates a new IRandom object with a specified seed.
        /// </summary>
        /// <returns></returns>
        public static IRandom Create(string seedString)
        {
            var seed = Seed.Parse(seedString);

            if (RandomFactory == null)
            {
                return new Xoroshiro128pp(seed);
            }

            return RandomFactory(seed);
        }

        /// <summary>
        /// Set to specify a custom random factory method.
        /// </summary>
        public static Func<Seed, IRandom> RandomFactory { get; set; }
    }
}

