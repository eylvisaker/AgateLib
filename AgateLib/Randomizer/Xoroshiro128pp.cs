using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Randomizer
{
    /// <summary>
    /// Implementation of XoRoShiRo128++ from http://prng.di.unimi.it/
    /// </summary>
    public class Xoroshiro128pp : IRandom
    {
        uint[] s = new uint[4];

        public Xoroshiro128pp(Seed seed)
        {
            s[0] = (uint)(seed.HighBits >> 32);
            s[1] = (uint)(seed.HighBits & 0xffffffff);
            s[2] = (uint)(seed.LowBits >> 32);
            s[3] = (uint)(seed.LowBits & 0xffffffff);
        }

        public Seed Seed => ToSeed(s[0], s[1], s[2], s[3]);

        public uint NextUInt32MaxValue => uint.MaxValue;

        public Seed GenerateSeed() => jump();

        public IRandom Spawn() => new Xoroshiro128pp(GenerateSeed());

        public double NextDouble() => next() / (double)NextUInt32MaxValue;

        public int NextInteger() => this.NextInteger(int.MaxValue);

        public float NextSingle() => next() / (float)NextUInt32MaxValue;

        public uint NextUInt32() => next();

        // Written in 2019 by David Blackman and Sebastiano Vigna (vigna@acm.org)
        // To the extent possible under law, the author has dedicated all copyright
        // and related and neighboring rights to this software to the public domain
        // worldwide. This software is distributed without any warranty.
        // 
        // See <http://creativecommons.org/publicdomain/zero/1.0/>. 
        // 
        // 
        // This is xoshiro128++ 1.0, one of our 32-bit all-purpose, rock-solid
        // generators. It has excellent speed, a state size (128 bits) that is
        // large enough for mild parallelism, and it passes all tests we are aware
        // of.
        // 
        // For generating just single-precision (i.e., 32-bit) floating-point
        // numbers, xoshiro128+ is even faster.
        // 
        // The state must be seeded so that it is not everywhere zero. */


        uint rotl(uint x, int k)
        {
            return (x << k) | (x >> (32 - k));
        }

        uint next()
        {
            uint result = rotl(s[0] + s[3], 7) + s[0];

            uint t = s[1] << 9;

            s[2] ^= s[0];
            s[3] ^= s[1];
            s[1] ^= s[2];
            s[0] ^= s[3];

            s[2] ^= t;

            s[3] = rotl(s[3], 11);

            return result;
        }

        static readonly uint[] JUMP = new uint[] { 0x8764000b, 0xf542d2d3, 0x6fa035c3, 0x77f2db5b };
        static readonly uint[] LONG_JUMP = new uint[] { 0xb523952e, 0x0b6f099f, 0xccf5a0ef, 0x1c580662 };

        /* This is the jump function for the generator. It is equivalent
           to 2^64 calls to next(); it can be used to generate 2^64
           non-overlapping subsequences for parallel computations. */

        Seed jump()
        {
            uint s0 = 0;
            uint s1 = 0;
            uint s2 = 0;
            uint s3 = 0;

            for (int i = 0; i < JUMP.Length; i++)
            {
                for (int b = 0; b < 32; b++)
                {
                    if ((JUMP[i] & (1 << b)) != 0)
                    {
                        s0 ^= s[0];
                        s1 ^= s[1];
                        s2 ^= s[2];
                        s3 ^= s[3];
                    }
                    next();
                }
            }

            return ToSeed(s0, s1, s2, s3);
        }


        /* This is the long-jump function for the generator. It is equivalent to
           2^96 calls to next(); it can be used to generate 2^32 starting points,
           from each of which jump() will generate 2^32 non-overlapping
           subsequences for parallel distributed computations. */

        void long_jump()
        {
            uint s0 = 0;
            uint s1 = 0;
            uint s2 = 0;
            uint s3 = 0;
            for (int i = 0; i < LONG_JUMP.Length; i++)
            {
                for (int b = 0; b < 32; b++)
                {
                    if ((LONG_JUMP[i] & (1 << b)) != 0)
                    {
                        s0 ^= s[0];
                        s1 ^= s[1];
                        s2 ^= s[2];
                        s3 ^= s[3];
                    }
                    next();
                }
            }

            s[0] = s0;
            s[1] = s1;
            s[2] = s2;
            s[3] = s3;
        }

        private static Seed ToSeed(uint s0, uint s1, uint s2, uint s3)
            => new Seed(((ulong)s0) << 32 | s1, ((ulong)s2) << 32 | s3);
    }
}
