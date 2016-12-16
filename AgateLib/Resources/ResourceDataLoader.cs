using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.DisplayLib.BitmapFont.TypeConverters;
using AgateLib.Geometry.TypeConverters;
using AgateLib.IO;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.DataModel.TypeConverters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.Resources
{
	public class ResourceDataLoader
	{
		private readonly IReadFileProvider fileProvider;

		public ResourceDataLoader(IReadFileProvider fileProvider = null)
		{
			this.fileProvider = fileProvider ?? Assets.UserInterfaceAssets;
		}

		/// <summary>
		/// Parses the text directly to a resource data model.
		/// Throws an exception if external files are referenced.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public ResourceDataModel LoadFromText(string text)
		{
			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(new HyphenatedNamingConvention())
				.WithTypeConverter(new ColorConverterYaml())
				.WithTypeConverter(new LayoutBoxConverterYaml())
				.WithTypeConverter(new PointConverterYaml())
				.WithTypeConverter(new SizeConverterYaml())
				.Build();

			using (var file = new StringReader(text)) 
			{
				ResourceDataModel result = deserializer.Deserialize<ResourceDataModel>(text);

				ThrowIfExternalFiles(result);

				return result;
			}
		}

		private void ThrowIfExternalFiles(ResourceDataModel config)
		{
			if (config.FontSources.Any() ||
				config.ThemeSources.Any() ||
				config.FacetSources.Any())
			{
				throw new AgateResourceException("The following properties must be empty: font-sources, theme-sources, facet-sources");
			}
		}

		/// <summary>
		/// Searches Assets.UserInterfaceAssets for the file and any peripheral files.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public ResourceDataModel Load(string filename)
		{
			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(new HyphenatedNamingConvention())
				.WithTypeConverter(new ColorConverterYaml())
				.WithTypeConverter(new LayoutBoxConverterYaml())
				.WithTypeConverter(new PointConverterYaml())
				.WithTypeConverter(new SizeConverterYaml())
				.WithTypeConverter(new RectangleConverterYaml())
				.WithTypeConverter(new KerningPairModelYaml())
				.Build();

			using (var file = new StreamReader(fileProvider.OpenRead(filename)))
			{
				ResourceDataModel result = deserializer.Deserialize<ResourceDataModel>(file);

				ReadExternalFiles(deserializer, result);

				result.Validate();

				return result;
			}
		}

		private void ReadExternalFiles(Deserializer deserializer, ResourceDataModel config)
		{
			ReadSources<FontResourceCollection, FontResource>(deserializer, config, config.FontSources,
				(key, value) => config.Fonts.Add(key, value));

			ReadSources<ThemeModelCollection, ThemeModel>(deserializer, config, config.ThemeSources,
				(key, value) => config.Themes.Add(key, value));

			ReadSources<FacetModelCollection, FacetModel>(deserializer, config, config.FacetSources,
				(key, value) => config.Facets.Add(key, value));
		}

		private void ReadSources<TCollection, TItem>(Deserializer deserializer, ResourceDataModel config, 
			IEnumerable<string> sources, Action<string, TItem> store) 
			where TCollection : IEnumerable<KeyValuePair<string, TItem>>
		{
			foreach (var filename in sources)
			{
				using (var file = new StreamReader(fileProvider.OpenRead(filename)))
				{
					var result = deserializer.Deserialize<TCollection>(file);

					foreach (var kvp in result)
					{
						store(kvp.Key, kvp.Value);
					}
				}
			}
		}
	}
}
