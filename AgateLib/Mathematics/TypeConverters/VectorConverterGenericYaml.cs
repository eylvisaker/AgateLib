//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using Microsoft.Xna.Framework;
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
			var isNullable = type == typeof(Vector2?) || type == typeof(Vector3?) || type == typeof(Vector4?);

			if (string.IsNullOrWhiteSpace(value))
			{
				if (isNullable)
				{
					parser.MoveNext();
					return null;
				}

				throw new InvalidDataException("Null value not allowed.");
			}

			var values = value
				.Split(delimiter, StringSplitOptions.RemoveEmptyEntries)
				.Select(float.Parse)
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
		protected abstract T Deserialize(float[] values);
	}
}
