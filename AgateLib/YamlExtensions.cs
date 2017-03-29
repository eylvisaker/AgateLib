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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.TypeConverters;
using YamlDotNet.Serialization;

namespace AgateLib
{
	/// <summary>
	/// Extensions to make YAML conversions with AgateLib types easier.
	/// </summary>
	public static class YamlExtensions
	{
		/// <summary>
		/// Adds all type converters that exist within the AgateLib.Mathematics namespace.
		/// </summary>
		/// <typeparam name="TBuilder"></typeparam>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static TBuilder WithTypeConvertersForAgateLibMathematics<TBuilder>(this BuilderSkeleton<TBuilder> builder)
			where TBuilder : BuilderSkeleton<TBuilder>
		{
			return builder
				.WithTypeConverter(new ColorConverterYaml())
				.WithTypeConverter(new PointConverterYaml())
				.WithTypeConverter(new RectangleConverterYaml())
				.WithTypeConverter(new SizeConverterYaml())
				.WithTypeConverter(new Vector2ConverterYaml())
				.WithTypeConverter(new Vector2fConverterYaml())
				.WithTypeConverter(new Vector3ConverterYaml())
				.WithTypeConverter(new Vector3fConverterYaml())
				.WithTypeConverter(new Vector4ConverterYaml())
				.WithTypeConverter(new Vector4fConverterYaml());
		}
	}
}
