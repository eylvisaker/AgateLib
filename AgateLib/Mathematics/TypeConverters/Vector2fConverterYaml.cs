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
	/// YAML serializer for Vector2f
	/// </summary>
	public class Vector2fConverterYaml : VectorConverterGenericYaml<Vector2f>
	{
		protected override Vector2f Deserialize(double[] values)
		{
			Require.True<InvalidDataException>(values.Length == 2,
				"Must have exactly two values to convert to a Vector2f object.");

			return new Vector2f(values[0], values[1]);
		}

		protected override string Serialize(Vector2f value)
		{
			return $"{value.X} {value.Y}";
		}
	}
}
