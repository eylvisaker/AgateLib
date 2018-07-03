using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.TypeConverters;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.Tests.MathematicsTests.Geometry.TypeConverters
{
    public class GeometryConverterTests
    {
        Deserializer deser;
        Serializer ser;

        public class Something
        {
            public Color Color { get; set; }
            public Point Position { get; set; }
            public Size Size { get; set; }

            public Color? NullableColor { get; set; }
            public Point? NullablePosition { get; set; }
            public Size? NullableSize { get; set; }
        }

        public GeometryConverterTests()
        {
            deser = new DeserializerBuilder()
                .WithTypeConverter(new ColorConverterYaml())
                .WithTypeConverter(new PointConverterYaml())
                .WithTypeConverter(new SizeConverterYaml())
                .WithNamingConvention(new HyphenatedNamingConvention())
                .Build();

            ser = new SerializerBuilder()
                .WithTypeConverter(new ColorConverterYaml())
                .WithTypeConverter(new SizeConverterYaml())
                .WithTypeConverter(new PointConverterYaml())
                .WithNamingConvention(new HyphenatedNamingConvention())
                .Build();
        }

        [Fact]
        public void YamlDeserializeColor()
        {
            var x = deser.Deserialize<Something>("color: ffeeddcc");

            x.Color.Should().Be(new Color(0xee, 0xdd, 0xcc, 0xff));
        }

        [Fact]
        public void YamlDeserializeSize()
        {
            var x = deser.Deserialize<Something>("size: 4 5");

            x.Size.Width.Should().Be(4);
            x.Size.Height.Should().Be(5);
        }

        [Fact]
        public void YamlDeserializePoint()
        {
            var x = deser.Deserialize<Something>("position: 4 5");

            x.Position.X.Should().Be(4);
            x.Position.Y.Should().Be(5);
        }

        [Fact]
        public void YamlDeserializeNullableColor()
        {
            var x = deser.Deserialize<Something>("nullable-color: ffeeddcc");

            x.NullableColor.Value.Should().Be(new Color(0xee, 0xdd, 0xcc, 0xff));
        }

        [Fact]
        public void YamlDeserializeNullableSize()
        {
            var x = deser.Deserialize<Something>("nullable-size: 4 5");

            x.NullableSize?.Width.Should().Be(4);
            x.NullableSize?.Height.Should().Be(5);
        }

        [Fact]
        public void YamlDeserializeNullablePoint()
        {
            var x = deser.Deserialize<Something>("nullable-position: 4 5");

            x.NullablePosition?.X.Should().Be(4);
            x.NullablePosition?.Y.Should().Be(5);
        }

        [Fact]
        public void YamlSerializeColor()
        {
            Something x = new Something { Color = new Color(0x22, 0x33, 0x44, 0x11) };

            var text = ser.Serialize(x);

            text.Trim().Should().Be("color: 11223344");
        }

        [Fact]
        public void YamlSerializePoint()
        {
            Something x = new Something { Position = new Point(4, 5) };

            var text = ser.Serialize(x);

            text.Trim().Should().Be("position: 4 5");
        }

        [Fact]
        public void YamlSerializeSize()
        {
            Something x = new Something { Size = new Size(4, 5) };

            var text = ser.Serialize(x);

            text.Trim().Should().Be("size: 4 5");
        }
    }
}
