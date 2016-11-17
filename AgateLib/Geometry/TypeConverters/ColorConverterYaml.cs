using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.Geometry.TypeConverters
{
	public class ColorConverterYaml : IYamlTypeConverter
	{
		public bool Accepts(Type type)
		{
			return type == typeof(Color) || type == typeof(Color?);
		}

		public object ReadYaml(IParser parser, Type type)
		{
			var scalar = (YamlDotNet.Core.Events.Scalar)parser.Current;
			var value = scalar.Value;

			if (string.IsNullOrWhiteSpace(value) && type == typeof(Color?))
			{
				parser.MoveNext();
				return null;
			}

			Color result;

			if (Color.IsNamedColor(value))
				result = Color.GetNamedColor(value);
			else
				result = Color.FromArgb(value);
			
			parser.MoveNext();
			return result;
		}

		public void WriteYaml(IEmitter emitter, object value, Type type)
		{
			var color = (Color)value;
			emitter.Emit(new YamlDotNet.Core.Events.Scalar(
				null,
				null,
				color.ToArgbString(),
				ScalarStyle.Plain,
				true,
				false
			));
		}
	}
}
