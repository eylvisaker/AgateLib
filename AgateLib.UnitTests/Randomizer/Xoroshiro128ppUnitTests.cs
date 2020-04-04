using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AgateLib.Randomizer
{
    /// <summary>
    /// The values used in this test were obtained by building a small test application in C
    /// using the source code from http://prng.di.unimi.it/xoshiro128plusplus.c
    /// </summary>
    public class Xoroshiro128ppUnitTests
    {
        [Theory]
        [InlineData(0xba5eba11deadbeef, 0xcafebabedeadbabe, 0x409921dd)]
        [InlineData(0, 1, 0x80)]
        [InlineData(0x0000000100000000, 1, 0x101)]
        [InlineData(0xb44dc876c78be3ed, 0x5a7714f1d9de1442, 0xca3c24bd)]
        public void FirstValueFromSeed(ulong seedHi, ulong seedLo, uint expectedValue)
        {
            Xoroshiro128pp random = new Xoroshiro128pp(new Seed(seedHi, seedLo));

            random.NextUInt32().Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(0, 1, 
                    new uint[] { 0x80, 0x40081, 0x20040881, 0x20480900, 0x20500983, 0x48111c02, 0x88c81402, 0x2adb05d7, 0x48de3b98, 0xa3cb5158 })]
        [InlineData(1, 1,
                    new uint[] { 0x80, 0x0, 0x40081, 0x28090a00, 0x50500140, 0xaa170c93, 0x981494a1, 0x56f02857, 0x3326c675, 0x7b13d560 })]
        public void SequenceFromSeed(ulong seedHi, ulong seedLo, params uint[] expectedValues)
        {
            Xoroshiro128pp random = new Xoroshiro128pp(new Seed(seedHi, seedLo));

            for (int i = 0; i < expectedValues.Length; i++)
            {
                random.NextUInt32().Should().Be(expectedValues[i]);
            }
        }

        [Theory]
        [InlineData(0xba5eba11deadbeef, 0xcafebabedeadbabe, 0xb44dc876c78be3ed, 0x5a7714f1d9de1442)]
        public void GenerateSeed(ulong seedHi, ulong seedLo, ulong expectedSeedHi, ulong expectedSeedLo)
        {
            Xoroshiro128pp random = new Xoroshiro128pp(new Seed(seedHi, seedLo));

            Seed seed = random.GenerateSeed();

            seed.HighBits.Should().Be(expectedSeedHi);
            seed.LowBits.Should().Be(expectedSeedLo);
        }
    }
}
