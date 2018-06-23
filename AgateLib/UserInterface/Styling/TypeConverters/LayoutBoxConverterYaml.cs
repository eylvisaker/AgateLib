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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.UserInterface.Styling.TypeConverters
{
	public class LayoutBoxConverterYaml : IYamlTypeConverter
	{
		public bool Accepts(Type type)
		{
			return type == typeof(LayoutBox) || type == typeof(LayoutBox?);
		}

		public object ReadYaml(IParser parser, Type type)
		{
			var scalar = (YamlDotNet.Core.Events.Scalar)parser.Current;
			var value = scalar.Value;

			if (string.IsNullOrWhiteSpace(value) && type == typeof(LayoutBox?))
			{
				parser.MoveNext();
				return null;
			}

			LayoutBox result = new LayoutBox();
			int[] values = value.Split(' ').Select(x => int.Parse(x)).ToArray();

			result.Left = values[0];
			result.Top = values[1 % values.Length];
			result.Right = values[2 % values.Length];
			result.Bottom = values[(values.Length == 4 ? 3 : 1) % values.Length];

			parser.MoveNext();
			return result;
		}

		public void WriteYaml(IEmitter emitter, object value, Type type)
		{
			var box = (LayoutBox)value;
			string result;

			if (box.Bottom == box.Top)
			{
				if (box.Right == box.Left)
				{
					if (box.Top == box.Left)
					{
						result = box.Left.ToString();
					}
					else
					{
						result = string.Format("{0} {1}", box.Left, box.Top);
					}
				}
				else
				{
					result = string.Format("{0} {1} {2}", box.Left, box.Top, box.Right);
				}
			}
			else
			{
				result = string.Format("{0} {1} {2} {3}", box.Left, box.Top, box.Right, box.Bottom);
			}

			emitter.Emit(new YamlDotNet.Core.Events.Scalar(
				null,
				null,
				result,
				ScalarStyle.Plain,
				true,
				false
			));
		}
	}
}
