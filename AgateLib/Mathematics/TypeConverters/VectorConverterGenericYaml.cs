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
	public abstract class VectorConverterGenericYaml<T> : IYamlTypeConverter where T : struct
	{
		private static readonly char[] delimiter = new[] { ' ' };

		/// <summary>
		/// Implement to indicate which values are parsed.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Accepts(Type type)
		{
			return type == typeof(T) || type == typeof(T?);
		}

		/// <summary>
		/// Deserializes Vector3 values.
		/// </summary>
		/// <param name="parser"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public object ReadYaml(IParser parser, Type type)
		{
			var scalar = (YamlDotNet.Core.Events.Scalar)parser.Current;
			var value = scalar.Value;

			if (string.IsNullOrWhiteSpace(value) && type == typeof(Vector3?))
			{
				parser.MoveNext();
				return null;
			}

			var values = value
				.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
				.Select(double.Parse)
				.ToArray();

			var result = Deserialize(values);

			parser.MoveNext();

			return result;
		}

		/// <summary>
		/// Serializes Vector3 values.
		/// </summary>
		/// <param name="emitter"></param>
		/// <param name="value"></param>
		/// <param name="type"></param>
		public void WriteYaml(IEmitter emitter, object value, Type type)
		{
			T point;

			if (type == typeof(T?))
			{
				if (value == null)
					return;

				point = ((T?)value).Value;
			}
			else
				point = (T)value;

			emitter.Emit(new YamlDotNet.Core.Events.Scalar(
				null,
				null,
				Serialize(point),
				ScalarStyle.Plain,
				true,
				false
			));
		}

		/// <summary>
		/// Serializes the value to text.
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		protected abstract string Serialize(T vector);

		/// <summary>
		/// Deserializes the value from the individual values read from the yaml.
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		protected abstract T Deserialize(double[] values);
	}
}
