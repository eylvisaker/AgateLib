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
using System.Linq;
using System.Reflection;
using AgateLib.Display;
using Microsoft.Xna.Framework;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.Mathematics.TypeConverters
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

            if (!ColorX.TryParseNamedColor(value, out result))
                result = ColorX.FromArgb(value);

            parser.MoveNext();
            return result;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var color = (Color)value;
            emitter.Emit(new YamlDotNet.Core.Events.Scalar(
                null,
                null,
                $"{color.A:x}{color.R:x}{color.G:x}{color.B:x}",
                ScalarStyle.Plain,
                true,
                false
            ));
        }
    }
}
