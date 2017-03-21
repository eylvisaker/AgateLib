using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Mathematics.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.UnitTests.MathematicsTests.Geometry.TypeConverters
{
	[TestClass]
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

		[TestInitialize]
		public void Initialize()
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

		[TestMethod]
		public void YamlDeserializeColor()
		{
			var x = deser.Deserialize<Something>("color: ffeeddcc");

			Assert.AreEqual(Color.FromArgb(0xff, 0xee, 0xdd, 0xcc), x.Color);
		}

		[TestMethod]
		public void YamlDeserializeSize()
		{
			var x = deser.Deserialize<Something>("size: 4 5");

			Assert.AreEqual(4, x.Size.Width);
			Assert.AreEqual(5, x.Size.Height);
		}

		[TestMethod]
		public void YamlDeserializePoint()
		{
			var x = deser.Deserialize<Something>("position: 4 5");

			Assert.AreEqual(4, x.Position.X);
			Assert.AreEqual(5, x.Position.Y);
		}

		[TestMethod]
		public void YamlDeserializeNullableColor()
		{
			var x = deser.Deserialize<Something>("nullable-color: ffeeddcc");

			Assert.AreEqual(Color.FromArgb(0xff, 0xee, 0xdd, 0xcc), x.NullableColor.Value);
		}

		[TestMethod]
		public void YamlDeserializeNullableSize()
		{
			var x = deser.Deserialize<Something>("nullable-size: 4 5");

			Assert.AreEqual(4, x.NullableSize?.Width);
			Assert.AreEqual(5, x.NullableSize?.Height);
		}

		[TestMethod]
		public void YamlDeserializeNullablePoint()
		{
			var x = deser.Deserialize<Something>("nullable-position: 4 5");

			Assert.AreEqual(4, x.NullablePosition?.X);
			Assert.AreEqual(5, x.NullablePosition?.Y);
		}

		[TestMethod]
		public void YamlSerializeColor()
		{
			Something x = new Something { Color = Color.FromArgb(0x11, 0x22, 0x33, 0x44) };

			var text = ser.Serialize(x);

			Assert.AreEqual("color: 11223344", text.Trim());
		}

		[TestMethod]
		public void YamlSerializePoint()
		{
			Something x = new Something { Position = new Point(4, 5) };

			var text = ser.Serialize(x);

			Assert.AreEqual("position: 4 5", text.Trim());
		}

		[TestMethod]
		public void YamlSerializeSize()
		{
			Something x = new Something { Size = new Size(4, 5) };

			var text = ser.Serialize(x);

			Assert.AreEqual("size: 4 5", text.Trim());
		}
	}
}
