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
	/// YAML serializer for Vector4f
	/// </summary>
	public class Vector4fConverterYaml : VectorConverterGenericYaml<Vector4f>
	{
		protected override Vector4f Deserialize(double[] values)
		{
			Require.True<InvalidDataException>(values.Length == 4,
				"Must have exactly four values to convert to a Vector4f object.");

			return new Vector4f(values[0], values[1], values[2], values[3]);
		}

		protected override string Serialize(Vector4f value)
		{
			return $"{value.X} {value.Y} {value.Z} {value.W}";
		}
	}
}
