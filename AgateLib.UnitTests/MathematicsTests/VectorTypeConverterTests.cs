using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.TypeConverters;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;
using YamlDotNet.Serialization;

namespace AgateLib.Tests.MathematicsTests
{
	public class VectorTypeConverterTests
	{
		private Deserializer deserializer;
		private Serializer serializer;

		public VectorTypeConverterTests()
		{
			serializer = new SerializerBuilder()
				.WithTypeConverter(new Vector2ConverterYaml())
				.WithTypeConverter(new Vector3ConverterYaml())
				.WithTypeConverter(new Vector4ConverterYaml())
				.Build();

			deserializer = new DeserializerBuilder()
				.WithTypeConverter(new Vector2ConverterYaml())
				.WithTypeConverter(new Vector3ConverterYaml())
				.WithTypeConverter(new Vector4ConverterYaml())
				.Build();
		}

		[Fact]
		public void TypeConverter_Vector2()
		{
			DoRoundTrip(4 * Vector2.UnitX + 3.8f * Vector2.UnitY, Vector2X.Equals);
			DoRoundTrip(-8.84f * Vector2.UnitX + -22.1f * Vector2.UnitY, Vector2X.Equals);
		}

		[Fact]
		public void TypeConverter_Vector3()
		{
			DoRoundTrip(4 * Vector3.UnitX + 3.8f * Vector3.UnitY + -11.22f * Vector3.UnitZ, Vector3X.Equals);
			DoRoundTrip(-8.84f * Vector3.UnitX + -22.1f * Vector3.UnitY + 12.5f * Vector3.UnitZ, Vector3X.Equals);
		}

		private void DoRoundTrip<T>(T item, Func<T, T, float, bool> compare) where T : struct
		{
			T result = deserializer.Deserialize<T>(serializer.Serialize(item));

            compare(result, item, 0.0001f).Should().BeTrue(
				$"Serialization round trip failed for {item}.");
		}
	}
}
