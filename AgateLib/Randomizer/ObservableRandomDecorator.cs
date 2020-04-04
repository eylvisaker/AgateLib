using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Randomizer
{
    /// <summary>
    /// Decorator pattern for observing when the random seed changes.
    /// </summary>
    public class ObservableRandomDecorator : IRandom
    {
        private readonly IRandom random;

        public ObservableRandomDecorator(IRandom random)
        {
            this.random = random;
        }

        public event Action SeedChanged;

        public Seed Seed => random.Seed;

        public uint NextUInt32MaxValue => random.NextUInt32MaxValue;

        public double NextDouble()
        {
            double result = random.NextDouble();

            SeedChanged?.Invoke();

            return result;
        }

        public uint NextUInt32()
        {
            uint result = random.NextUInt32();

            SeedChanged?.Invoke();

            return result;
        }

        public float NextSingle()
        {
            float result = random.NextSingle();

            SeedChanged?.Invoke();

            return result;
        }

        public Seed GenerateSeed()
        {
            Seed result = random.GenerateSeed();

            SeedChanged?.Invoke();

            return result;
        }
    }
}
