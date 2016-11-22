using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;
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
		/// <summary>
		/// Searches Assets.UserInterfaceAssets
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
				.Build();

			using (var file = new StreamReader(Assets.UserInterfaceAssets.OpenRead(filename)))
			{
				ResourceDataModel result = deserializer.Deserialize<ResourceDataModel>(file);

				ReadExternalFiles(deserializer, result);

				return result;
			}
		}

		private void ReadExternalFiles(Deserializer deserializer, ResourceDataModel config)
		{
			ReadSources<FontModelCollection, List<FontModel>>(deserializer, config, config.FontSources, 
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
				using (var file = new StreamReader(Assets.UserInterfaceAssets.OpenRead(filename)))
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
