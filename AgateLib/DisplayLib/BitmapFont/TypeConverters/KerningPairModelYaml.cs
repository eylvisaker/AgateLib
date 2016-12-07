using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.DisplayLib.BitmapFont.TypeConverters
{
	public class KerningPairModelYaml : IYamlTypeConverter
	{
		private static readonly char[] delimiter = new[] { ' ' };

		public bool Accepts(Type type)
		{
			return type == typeof(KerningPairModel);
		}

		public object ReadYaml(IParser parser, Type type)
		{
			var scalar = (YamlDotNet.Core.Events.Scalar)parser.Current;
			var value = scalar.Value;

			if (string.IsNullOrWhiteSpace(value))
			{
				parser.MoveNext();
				return null;
			}

			var values = value
				.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
				.Select(s => int.Parse(s))
				.ToArray();

			Condition.Requires<InvalidDataException>(values.Length == 3,
				"Must have exactly three values to convert to a KerningPairModel object.");

			var result = new KerningPairModel { First = values[0], Second = values[1], Amount = values[2] };

			parser.MoveNext();
			return result;
		}

		public void WriteYaml(IEmitter emitter, object value, Type type)
		{
			KerningPairModel kp = (KerningPairModel)value;

			emitter.Emit(new YamlDotNet.Core.Events.Scalar(
				null,
				null,
				$"{kp.First} {kp.Second} {kp.Amount}",
				ScalarStyle.Plain,
				true,
				false
			));
		}
	}
}
