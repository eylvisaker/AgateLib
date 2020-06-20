using FluentAssertions;
using Xunit;
using YamlDotNet.Serialization;

namespace AgateLib.Randomizer
{
    public class SeedSerializationUnitTests
    {
        [Theory]
        [InlineData("---------------------")]
        [InlineData("Nachos are awesome")]
        [InlineData("This seed is almost c")]
        [InlineData("Nachoes rule")]
        [InlineData("Some seed")]
        [InlineData("Some longer seed")]
        [InlineData("A really long seed va")]
        public void SeedFromTextNoTrunc(string text)
        {
            Seed s = Seed.Parse(text);

            s.ToString().Should().Be(text.Replace(' ', '_'));
        }

        [Theory]
        [InlineData("-------------------------", "---------------------2")]
        [InlineData("A really long seed values", "A really long seed va")]
        [InlineData("This value will be trunca", "This value will be tr0")]
        [InlineData("This value will be tr----", "This value will be tr2")]
        public void SeedFromTextWithTrunc(string text, string expected)
        {
            Seed s = Seed.Parse(text);

            s.ToString().Should().Be(expected.Replace(' ', '_'));
        }

        [Theory]
        [InlineData("0xffffffffffffffff,0xffffffffffffffff", 0xffffffffffffffff, 0xffffffffffffffff)]
        [InlineData("0x123456789abcdef0,0x987654321aabbaaf", 0x123456789abcdef0, 0x987654321aabbaaf)]
        public void SeedFromOldEncoding(string yaml, ulong highBits, ulong lowBits)
        {
            var deserializer = new DeserializerBuilder().WithTypeConvertersForBasicStructures().Build();

            Seed result = deserializer.Deserialize<SerialStructure>("s: " + yaml).s.Value;

            result.HighBits.Should().Be(highBits);
            result.LowBits.Should().Be(lowBits);
        }

        [Theory]
        [InlineData("---------------------2", 0xffffffffffffffff, 0xffffffffffffffff)]
        [InlineData("kfwfP72KrW8xTogbtO4CH", 0x123456789abcdef0, 0x987654321aabbaaf)]
        public void SeedFromYaml(string yaml, ulong highBits, ulong lowBits)
        {
            var deserializer = new DeserializerBuilder().WithTypeConvertersForBasicStructures().Build();

            Seed result = deserializer.Deserialize<SerialStructure>("s: " + yaml).s.Value;

            result.HighBits.Should().Be(highBits);
            result.LowBits.Should().Be(lowBits);
        }

        [Theory]
        [InlineData("'---------------------2'", 0xffffffffffffffff, 0xffffffffffffffff)]
        [InlineData("kfwfP72KrW8xTogbtO4CH", 0x123456789abcdef0, 0x987654321aabbaaf)]
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
