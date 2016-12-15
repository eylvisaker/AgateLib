using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry.TypeConverters;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface.DataModel.TypeConverters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.Resources
{
	public class ResourceDataSerializer
	{
		private Serializer serializer;

		public ResourceDataSerializer()
		{
			serializer = new SerializerBuilder()
				.WithNamingConvention(new HyphenatedNamingConvention())
				.WithTypeConverter(new ColorConverterYaml())
				.WithTypeConverter(new LayoutBoxConverterYaml())
				.WithTypeConverter(new PointConverterYaml())
				.WithTypeConverter(new SizeConverterYaml())
				.WithTypeConverter(new RectangleConverterYaml())
				.Build();
		}

		public string Serialize(FontResourceCollection fonts)
		{
			return serializer.Serialize(fonts);
		}
	}
}
