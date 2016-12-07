using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.Geometry.TypeConverters
{
	public class RectangleConverterYaml : IYamlTypeConverter
	{
		private static readonly char[] delimiter = new[] { ' ' };

		public bool Accepts(Type type)
		{
			return type == typeof(Rectangle) || type == typeof(Rectangle?);
		}

		public object ReadYaml(IParser parser, Type type)
		{
			var scalar = (YamlDotNet.Core.Events.Scalar)parser.Current;
			var value = scalar.Value;

			if (string.IsNullOrWhiteSpace(value) && type == typeof(Rectangle?))
			{
				parser.MoveNext();
				return null;
			}

			var values = value
				.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
				.Select(s => int.Parse(s))
				.ToArray();

			Condition.Requires<InvalidDataException>(values.Length == 4,
				"Must have exactly two values to convert to a Rectangle object.");

			var result = new Rectangle(values[0], values[1], values[2], values[3]);

			parser.MoveNext();
			return result;
		}

		public void WriteYaml(IEmitter emitter, object value, Type type)
		{
			Rectangle rect;

			if (type == typeof(Rectangle?))
			{
				if (value == null)
					return;

				rect = ((Rectangle?)value).Value;
			}
			else
				rect = (Rectangle)value;

			emitter.Emit(new YamlDotNet.Core.Events.Scalar(
				null,
				null,
				$"{rect.X} {rect.Y} {rect.Width} {rect.Height}",
				ScalarStyle.Plain,
				true,
				false
			));
		}
	}
}
