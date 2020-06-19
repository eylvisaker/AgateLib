using AgateLib.Quality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Randomizer
{
    /// <summary>
    /// Provides a 128 bit number that can be used to track the state of random number generator.
    /// </summary>
    public struct Seed
    {
        static readonly char[] delimiter = new char[] { ',' };

        public Seed(ulong lowBits)
        {
            HighBits = 0;
            LowBits = lowBits;
        }

        public Seed(long lowBits)
        {
            if (lowBits < 0)
            {
                HighBits = unchecked((ulong)-1);
                LowBits = (ulong)lowBits;
            }
            else
            {
                HighBits = 0;
                LowBits = (ulong)lowBits;
            }
        }

        public Seed(ulong highBits, ulong lowBits)
        {
            HighBits = highBits;
            LowBits = lowBits;
        }

        public ulong HighBits { get; set; }

        public ulong LowBits { get; set; }

        [Obsolete("Better randomness can be achieved by using 64 or 128 bit integers.")]
        public static implicit operator Seed(int value)
        {
            return new Seed(0, unchecked((uint)value));
        }

        [Obsolete("Better to use the seed constructor and supply 128 bits.")]
        public static implicit operator Seed(long value)
        {
            return new Seed(0, unchecked((ulong)value));
        }

        public static bool operator ==(Seed a, ulong b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Seed a, ulong b)
        {
            return !a.Equals(b);
        }

        public bool Equals(ulong value)
        {
            return HighBits == 0 && LowBits == value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Seed s) return Equals(s);
            if (obj is ulong ul) return Equals(ul);
            if (obj is uint ui) return Equals((ulong)ui);
            if (obj is int i) return Equals((ulong)i);
            if (obj is long l) return Equals((ulong)l);

            return false;
        }

        public bool Equals(Seed other) 
        {
            return this.HighBits == other.HighBits 
                && this.LowBits == other.LowBits;
        }

        public override string ToString()
            => $"0x{HighBits:x}{delimiter[0]}0x{LowBits:x}";

        public static Seed Parse(string value)
        {
            ulong[] values = value
                .Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => Convert.ToUInt64(s, 16))
                .ToArray();

            Require.ArgumentInRange(values.Length == 2, nameof(value),
                $"Seed string must be in format of 0xabcd{delimiter[0]}0xabcd");

            return new Seed(values[0], values[1]);
        }
    }
}
