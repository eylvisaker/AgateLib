//
//    Copyright (c) 2006-2020 Erik Ylvisaker
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
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace AgateLib.Randomizer.TypeConverters
{
    /// <summary>
    /// Converts a KerningPairModel object to YAML.
    /// </summary>
    public class SeedYaml : IYamlTypeConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Accepts(Type type)
        {
            return type == typeof(Seed) || type == typeof(Seed?);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var scalar = (YamlDotNet.Core.Events.Scalar)parser.Current;
            var value = scalar.Value;

            if (string.IsNullOrWhiteSpace(value) && type == typeof(Seed?))
            {
                parser.MoveNext();
                return null;
            }

            Seed result = Seed.Parse(value);

            parser.MoveNext();

            return result;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            Seed seed;

            if (type == typeof(Seed?))
            {
                if (value == null)
                {
                    return;
                }

                seed = ((Seed?)value).Value;
            }
            else
            {
                seed = (Seed)value;
            }

            string text = seed.ToString();

            emitter.Emit(new YamlDotNet.Core.Events.Scalar(
                null,
                null,
                text,
                ScalarStyle.Plain,
                true,
                false
            ));
        }
    }
}
