namespace AgateLib.Randomizer
{
    public class FakeRandom : IRandom
    {
        public Seed Seed => new Seed(1);

        public uint NextUInt32MaxValue => (uint)short.MaxValue;

        public double NextDouble() => 0.5;
        public uint NextUInt32() => NextUInt32MaxValue / 2;
        public float NextSingle() => 0.5f;

        /// <summary>
        /// Generates a seed that can be used for a new random number generator. 
        /// You may store this to generate the same random sequence at a later time.
        /// </summary>
        /// <param name="random"></param>
        public Seed GenerateSeed() => new Seed(1);

        public IRandom Spawn() => new FakeRandom();
    }
}
