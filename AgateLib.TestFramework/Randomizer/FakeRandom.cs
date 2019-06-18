using AgateLib.Randomizer;

namespace AgateLib.Randomizer
{
    public class FakeRandom : IRandom
    {
        public long Seed => 1;

        public int NextIntegerMaxValue => short.MaxValue;

        public double NextDouble() => 0.5;
        public int NextInteger() => NextIntegerMaxValue / 2;
        public float NextSingle() => 0.5f;
    }
}
