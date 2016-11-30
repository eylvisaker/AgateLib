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
	public class SizeConverterYaml : IYamlTypeConverter
	{
		private static readonly char[] delimiter = new[] { ' ' };

		public bool Accepts(Type type)
		{
			return type == typeof(Size) || type == typeof(Size?);
		}

		public object ReadYaml(IParser parser, Type type)
		{
			var scalar = (YamlDotNet.Core.Events.Scalar)parser.Current;
			var value = scalar.Value;

			if (string.IsNullOrWhiteSpace(value) && type == typeof(Size?))
			{
				parser.MoveNext();
				return null;
			}

			var values = value
				.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
				.Select(s => int.Parse(s))
				.ToArray();

			Condition.Requires<InvalidDataException>(values.Length == 2,
				"Must have exactly two values to convert to a Size object.");

			var result = new Size(values[0], values[1]);

			parser.MoveNext();
			return result;
		}

		public void WriteYaml(IEmitter emitter, object value, Type type)
		{
			Size Size;

			if (type == typeof(Size?))
			{
				if (value == null)
					return;

				Size = ((Size?)value).Value;
			}
			else
				Size = (Size)value;

			emitter.Emit(new YamlDotNet.Core.Events.Scalar(
				null,
				null,
				$"{Size.Width} {Size.Height}",
				ScalarStyle.Plain,
				true,
				false
			));
		}
	}
}
