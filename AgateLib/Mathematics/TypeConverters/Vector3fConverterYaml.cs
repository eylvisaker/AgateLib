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
	/// YAML serializer for Vector3f
	/// </summary>
	public class Vector3fConverterYaml : VectorConverterGenericYaml<Vector3f>
	{
		protected override Vector3f Deserialize(double[] values)
		{
			Require.True<InvalidDataException>(values.Length == 3,
				"Must have exactly three values to convert to a Vector3f object.");

			return new Vector3f(values[0], values[1], values[2]);
		}

		protected override string Serialize(Vector3f value)
		{
			return $"{value.X} {value.Y} {value.Z}";
		}
	}
}
