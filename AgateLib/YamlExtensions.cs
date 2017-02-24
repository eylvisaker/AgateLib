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
