//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Rendering;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.UserInterface.DataModel.TypeConverters
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
