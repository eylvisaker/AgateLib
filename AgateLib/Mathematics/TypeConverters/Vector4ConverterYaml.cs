using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.Mathematics.TypeConverters
{
	/// <summary>
	/// YAML serializer for Vector4
	/// </summary>
	public class Vector4ConverterYaml : VectorConverterGenericYaml<Vector4>
	{
		protected override Vector4 Deserialize(double[] values)
		{
			Require.True<InvalidDataException>(values.Length == 4,
				"Must have exactly four values to convert to a Vector4 object.");

			return new Vector4(values[0], values[1], values[2], values[3]);
		}

		protected override string Serialize(Vector4 value)
		{
			return $"{value.X} {value.Y} {value.Z} {value.W}";
		}
	}
}
