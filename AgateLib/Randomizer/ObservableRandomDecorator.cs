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

        public long Seed => random.Seed;

        public int NextIntegerMaxValue => random.NextIntegerMaxValue;

        public double NextDouble()
        {
            double result = random.NextDouble();

            SeedChanged?.Invoke();

            return result;
        }

        public int NextInteger()
        {
            int result = random.NextInteger();

            SeedChanged?.Invoke();

            return result;
        }

        public float NextSingle()
        {
            float result = random.NextSingle();

            SeedChanged?.Invoke();

            return result;
        }
    }
}
