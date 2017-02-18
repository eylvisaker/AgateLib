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
	/// YAML serializer for Vector2
	/// </summary>
	public class Vector2ConverterYaml : VectorConverterGenericYaml<Vector2>
	{
		protected override Vector2 Deserialize(double[] values)
		{
			Require.True<InvalidDataException>(values.Length == 2,
				"Must have exactly two values to convert to a Vector2 object.");

			return new Vector2(values[0], values[1]);
		}

		protected override string Serialize(Vector2 value)
		{
			return $"{value.X} {value.Y}";
		}
	}
}
