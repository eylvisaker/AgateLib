using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics;
using AgateLib.Mathematics.TypeConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YamlDotNet.Serialization;

namespace AgateLib.UnitTests.MathematicsTests
{
	[TestClass]
	public class VectorTypeConverterTests
	{
		private Deserializer deserializer;
		private Serializer serializer;

		[TestInitialize]
		public void Initialize()
		{
			serializer = new SerializerBuilder()
				.WithTypeConverter(new Vector2ConverterYaml())
				.WithTypeConverter(new Vector3ConverterYaml())
				.WithTypeConverter(new Vector4ConverterYaml())
				.WithTypeConverter(new Vector2fConverterYaml())
				.WithTypeConverter(new Vector3fConverterYaml())
				.WithTypeConverter(new Vector4fConverterYaml())
				.Build();

			deserializer = new DeserializerBuilder()
				.WithTypeConverter(new Vector2ConverterYaml())
				.WithTypeConverter(new Vector3ConverterYaml())
				.WithTypeConverter(new Vector4ConverterYaml())
				.WithTypeConverter(new Vector2fConverterYaml())
				.WithTypeConverter(new Vector3fConverterYaml())
				.WithTypeConverter(new Vector4fConverterYaml())
				.Build();
		}

		[TestMethod]
		public void TypeConverter_Vector2()
		{
			DoRoundTrip(4 * Vector2.UnitX + 3.8 * Vector2.UnitY, Vector2.Equals);
			DoRoundTrip(-8.84 * Vector2.UnitX + -22.1 * Vector2.UnitY, Vector2.Equals);

			DoRoundTrip(4 * Vector2f.UnitX + 3.8 * Vector2f.UnitY, Vector2f.Equals);
			DoRoundTrip(-8.84 * Vector2f.UnitX + -22.1 * Vector2f.UnitY, Vector2f.Equals);
		}

		[TestMethod]
		public void TypeConverter_Vector3()
		{
			DoRoundTrip(4 * Vector3.UnitX + 3.8 * Vector3.UnitY + -11.22 * Vector3.UnitZ, Vector3.Equals);
			DoRoundTrip(-8.84 * Vector3.UnitX + -22.1 * Vector3.UnitY + 12.5 * Vector3.UnitZ, Vector3.Equals);

			DoRoundTrip(4 * Vector3f.UnitX + 3.8 * Vector3f.UnitY + -11.22 * Vector3f.UnitZ, Vector3f.Equals);
			DoRoundTrip(-8.84 * Vector3f.UnitX + -22.1 * Vector3f.UnitY + 12.5 * Vector3f.UnitZ, Vector3f.Equals);
		}

		private void DoRoundTrip<T>(T item, Func<T, T, double, bool> compare) where T : struct
		{
			T result = deserializer.Deserialize<T>(serializer.Serialize(item));

			Assert.IsTrue(compare(result, item, 0.0001), 
				$"Serialization round trip failed for {item}.");
		}
	}
}
