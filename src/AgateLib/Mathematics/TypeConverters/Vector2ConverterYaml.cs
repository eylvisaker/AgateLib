﻿//
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

using AgateLib.Quality;
using Microsoft.Xna.Framework;
using System.IO;

namespace AgateLib.Mathematics.TypeConverters
{
    /// <summary>
    /// YAML serializer for Vector2
    /// </summary>
    public class Vector2ConverterYaml : VectorConverterGenericYaml<Microsoft.Xna.Framework.Vector2>
    {
        protected override Vector2 Deserialize(float[] values)
        {
            Require.That<InvalidDataException>(values.Length == 2,
                "Must have exactly two values to convert to a Vector2 object.");

            return new Vector2(values[0], values[1]);
        }

        protected override string Serialize(Vector2 value)
        {
            return System.FormattableString.Invariant($"{value.X} {value.Y}");
        }
    }
}
