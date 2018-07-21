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

namespace AgateLib.Randomizer
{
    /// <summary>
    /// Defines a random number generator which uses the FastRand algorithm to generate random values.
    /// </summary>
    /// <remarks>
    /// Adapted from https://github.com/Matthew-Davey/mercury-particle-engine/blob/develop/Mercury.ParticleEngine.Core/FastRand.cs
    /// </remarks>
    public class FastRandom : IRandom
    {
        private int _state = 1;

        [Obsolete("Specify a seed explicitly.")]
        public FastRandom() : this(1)
        { }

        /// <summary>
        /// Initializes a FastRandom object. 
        /// </summary>
        /// <param name="seed"></param>
        public FastRandom(int seed)
        {
            Seed = seed;
        }

        /// <summary>
        /// Produce a new random number generator by pulling a seed value of an existing
        /// random number generator.
        /// </summary>
        /// <param name="random"></param>
        public FastRandom(IRandom random)
        {
            var a = random.NextInteger(Int16.MaxValue);
            var b = random.NextInteger(Int16.MaxValue);
            var c = random.NextInteger(1);

            var aa = a << 16;
            var bb = b << 1;

            Seed = aa | bb | c;
        }

        /// <summary>
        /// Gets or sets the seed value that will produce the next random number.
        /// </summary>
        public int Seed
        {
            get => _state;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(Seed), "seed must be greater than zero");

                _state = value;
            }
        }

        long IRandom.Seed => _state;

        /// <summary>
        /// Gets the next double precision value.
        /// </summary>
        /// <returns></returns>
        public double NextDouble() => NextSingle();

        public int NextIntegerMaxValue => Int16.MaxValue;

        /// <summary>
        /// Gets the next random integer value.
        /// </summary>
        /// <returns>A random positive integer.</returns>
        public int NextInteger()
        {
            _state = 214013 * _state + 2531011;
            return (_state >> 16) & 0x7FFF;
        }

        /// <summary>
        /// Gets the next random single value.
        /// </summary>
        /// <returns>A random single value between 0 and 1.</returns>
        public float NextSingle() => NextInteger() / (float)(Int16.MaxValue + 1);

    }
}
