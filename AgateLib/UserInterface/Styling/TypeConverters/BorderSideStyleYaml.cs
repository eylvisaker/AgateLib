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
using AgateLib.Display;
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.UserInterface.Styling.TypeConverters
{
    public class BorderSideStyleYaml : IYamlTypeConverter
    {
        private static readonly char[] delimiter = new[] { ' ' };

        public bool Accepts(Type type)
        {
            return type == typeof(BorderSideStyle);
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
                .ToList();

            Color? color = TryParseEachAndRemove<Color?>(values, 
                x => {
                    bool success = ColorX.TryParse(x, out Color c);
                    return (success, c);
                });
            int? size = TryParseEachAndRemove(values, x
                => {
                    bool success = int.TryParse(x, out int number);
                    return (success, number);
                });

            var result = new BorderSideStyle
            {
                Width = size ?? 0,
                Color = color ?? Color.Transparent,
            };

            parser.MoveNext();
            return result;
        }

        private T TryParseEachAndRemove<T>(List<string> values, Func<string, (bool, T)> parser)
        {
            foreach(var value in values)
            {
                (bool success, T parsedValue) = parser(value);

                if (success)
                {
                    values.Remove(value);

                    return parsedValue;
                }
            }

            return default(T);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            BorderSideStyle borderSide = (BorderSideStyle)value;

            emitter.Emit(new YamlDotNet.Core.Events.Scalar(
                null,
                null,
                $"{borderSide.Width} {borderSide.Color}",
                ScalarStyle.Plain,
                true,
                false
            ));
        }
    }
}
