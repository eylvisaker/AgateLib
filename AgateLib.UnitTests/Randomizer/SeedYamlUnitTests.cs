using FluentAssertions;
using Xunit;
using YamlDotNet.Serialization;

namespace AgateLib.Randomizer
{
    public class SeedYamlUnitTests
    {
        [Theory]
        [InlineData("0xffffffffffffffff,0xffffffffffffffff", 0xffffffffffffffff, 0xffffffffffffffff)]
        [InlineData("0x123456789abcdef0,0x987654321aabbaaf", 0x123456789abcdef0, 0x987654321aabbaaf)]
        public void SeedFromYaml(string yaml, ulong highBits, ulong lowBits)
        {
            var deserializer = new DeserializerBuilder().WithTypeConvertersForBasicStructures().Build();

            Seed result = deserializer.Deserialize<SerialStructure>("s: " + yaml).s.Value;

            result.HighBits.Should().Be(highBits);
            result.LowBits.Should().Be(lowBits);
        }

        [Theory]
        [InlineData("0xffffffffffffffff,0xffffffffffffffff", 0xffffffffffffffff, 0xffffffffffffffff)]
        [InlineData("0x123456789abcdef0,0x987654321aabbaaf", 0x123456789abcdef0, 0x987654321aabbaaf)]
        public void SeedToYaml(string yaml, ulong highBits, ulong lowBits)
        {
            var serializer = new SerializerBuilder().WithTypeConvertersForBasicStructures().Build();

            string result = serializer.Serialize(new SerialStructure { s = new Seed(highBits, lowBits) }).Trim();

            result.Should().Be("s: " + yaml);
        }

        private class SerialStructure
        {
            public Seed? s { get; set; }
        }
    }
}
