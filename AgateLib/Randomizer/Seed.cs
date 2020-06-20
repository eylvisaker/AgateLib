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
        private static readonly char[] delimiter = new char[] { ',' };
        private static Dictionary<char, uint> base64encodingForward = new Dictionary<char, uint>();
        private static Dictionary<uint, char> base64encodingBack = new Dictionary<uint, char>();

        static Seed()
        {
            uint val = 0;

            base64encodingForward.Add('_', val);
            base64encodingBack.Add(val, '_');
            val++;

            for (char i = '0'; i <= '9'; i++)
            {
                base64encodingForward.Add(i, val);
                base64encodingBack.Add(val, i);
                val++;
            }

            for (char i = 'A'; i <= 'Z'; i++)
            {
                base64encodingForward.Add(i, val);
                base64encodingBack.Add(val, i);
                val++;
            }

            for (char i = 'a'; i <= 'z'; i++)
            {
                base64encodingForward.Add(i, val);
                base64encodingBack.Add(val, i);
                val++;
            }

            base64encodingForward.Add('-', val);
            base64encodingBack.Add(val, '-');
            val++;

            Require.That(base64encodingForward.Count == 64, $"Should have 64 digit values but only have {base64encodingForward.Count}");
        }

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
            if (obj is Seed s)
            {
                return Equals(s);
            }

            if (obj is ulong ul)
            {
                return Equals(ul);
            }

            if (obj is uint ui)
            {
                return Equals((ulong)ui);
            }

            if (obj is int i)
            {
                return Equals((ulong)i);
            }

            if (obj is long l)
            {
                return Equals((ulong)l);
            }

            return false;
        }

        public bool Equals(Seed other)
        {
            return this.HighBits == other.HighBits
                && this.LowBits == other.LowBits;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            for (int charIndex = 0; charIndex < 22; charIndex++)
            {
                uint bits;

                if (charIndex < 10)
                {
                    int bitIndex = charIndex * 6;
                    bits = (uint)(LowBits >> bitIndex) & 0x3f; // lowest six bits is 0x3f
                }
                else if (charIndex == 10)
                {
                    // 4 bits from the lowbits and 2 bits from the high bits.
                    bits = (uint)(LowBits >> 60) & 0x0f;
                    bits |= (uint)((HighBits & 0x03) << 4);
                }
                else
                {
                    int bitIndex = (charIndex - 11) * 6 + 2;
                    bits = (uint)(HighBits >> bitIndex) & 0x3f;
                }

                char c = base64encodingBack[bits];

                result.Append(c.ToString());
            }

            return result.ToString().TrimEnd('_');
        }

        public static Seed Parse(string value)
        {
            if (value.Contains(','))
            {
                ulong[] values = value
                    .Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => Convert.ToUInt64(s, 16))
                    .ToArray();

                Require.ArgumentInRange(values.Length == 2, nameof(value),
                    $"Seed string must be in format of 0xabcd{delimiter[0]}0xabcd");

                return new Seed(values[0], values[1]);
            }

            return FromText(value);
        }

        public static Seed FromText(string text)
        {
            bool writingHighBits = false;
            ulong lobits = 0;
            ulong bits = 0;
            int bitIndex = 0;
            int textIndex = 0;

            while (textIndex < text.Length)
            {
                char c = text[textIndex];

                if (c == ' ') c = '_';
                if (!base64encodingForward.ContainsKey(c))
                    c = '-';

                ulong bitval = base64encodingForward[c];

                if (textIndex == 21)
                {
                    // only need the last two bits
                    bits |= (bitval & 0x03) << 62;
                    break;
                }
                if (textIndex == 10)
                {
                    bits |= (bitval & 0x0f) << 60;
                    writingHighBits = true;
                    lobits = bits;

                    bitIndex = 2;
                    
                    bits = bitval >> 4;
                }
                else 
                {
                    bitval <<= bitIndex;

                    bits |= bitval;

                    bitIndex += 6;
                }

                textIndex ++;
            }

            if (!writingHighBits)
            {
                lobits = bits;
                bits = 0;
            }

            return new Seed(bits, lobits);
        }
    }
}
