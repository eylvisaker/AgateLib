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
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.UserInterface.Styling.Themes.Model.TypeConverters
{
    class ImageSourceYamlConverter : IYamlTypeConverter
    {
        private static readonly char[] delimiter = new[] { ':' };

        public bool Accepts(Type type)
        {
            return type == typeof(ImageSource);
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

            var result = new ImageSource();

            result.File = values[0];

            if (values.Count > 1)
            {
                result.SourceRect = ParseSourceRect(values[1]);
            }

            parser.MoveNext();
            return result;
        }

        private Rectangle? ParseSourceRect(string text)
        {
            Regex matcher = new Regex(@"rect\((\d+) +(\d+) +(\d+) (\d+)\)");

            var match = matcher.Match(text);
            var groups = match.Groups;

            var result = new Rectangle(
                int.Parse(groups[1].Value),
                int.Parse(groups[2].Value),
                int.Parse(groups[3].Value),
                int.Parse(groups[4].Value));

            return result;
        }
        
        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
