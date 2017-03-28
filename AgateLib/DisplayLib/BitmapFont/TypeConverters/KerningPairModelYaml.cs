//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.DisplayLib.BitmapFont.TypeConverters
{
	/// <summary>
	/// Converts a KerningPairModel object to YAML.
	/// </summary>
	public class KerningPairModelYaml : IYamlTypeConverter
	{
		private static readonly char[] delimiter = new[] { ' ' };

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Accepts(Type type)
		{
			return type == typeof(KerningPairModel);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parser"></param>
		/// <param name="type"></param>
		/// <returns></returns>
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="emitter"></param>
		/// <param name="value"></param>
		/// <param name="type"></param>
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
