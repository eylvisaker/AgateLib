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
	/// YAML serializer for Vector3
	/// </summary>
	public class Vector3ConverterYaml : VectorConverterGenericYaml<Vector3>
	{
		protected override Vector3 Deserialize(double[] values)
		{
			Require.True<InvalidDataException>(values.Length == 3,
				"Must have exactly three values to convert to a Vector3 object.");

			return new Vector3(values[0], values[1], values[2]);
		}

		protected override string Serialize(Vector3 value)
		{
			return $"{value.X} {value.Y} {value.Z}";
		}
	}
}
