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

namespace AgateLib.Mathematics.TypeConverters
{
	/// <summary>
	/// YAML serializer for Vector4f
	/// </summary>
	public class Vector4fConverterYaml : VectorConverterGenericYaml<Vector4f>
	{
		protected override Vector4f Deserialize(double[] values)
		{
			Require.True<InvalidDataException>(values.Length == 4,
				"Must have exactly four values to convert to a Vector4f object.");

			return new Vector4f(values[0], values[1], values[2], values[3]);
		}

		protected override string Serialize(Vector4f value)
		{
			return $"{value.X} {value.Y} {value.Z} {value.W}";
		}
	}
}
