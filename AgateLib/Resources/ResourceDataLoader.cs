//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
using AgateLib.Quality;
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
		private Deserializer deserializer;

		public ResourceDataLoader(IReadFileProvider fileProvider = null)
		{
			this.fileProvider = fileProvider ?? AgateApp.Assets;

			Require.ArgumentNotNull(this.fileProvider, nameof(fileProvider),
				$"Either {nameof(fileProvider)} or {nameof(AgateApp)}.{nameof(AgateApp.Assets)} should not be null.");

			deserializer = new DeserializerBuilder()
				.WithNamingConvention(new HyphenatedNamingConvention())
				.WithTypeConverter(new ColorConverterYaml())
				.WithTypeConverter(new LayoutBoxConverterYaml())
				.WithTypeConverter(new PointConverterYaml())
				.WithTypeConverter(new SizeConverterYaml())
				.WithTypeConverter(new RectangleConverterYaml())
				.WithTypeConverter(new KerningPairModelYaml())
				.Build();
		}

		/// <summary>
		/// Searches Assets.UserInterfaceAssets for the file and any peripheral files.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public ResourceDataModel Load(string filename)
		{
			using (var file = new StreamReader(fileProvider.OpenRead(filename)))
			{
				ResourceDataModel result = deserializer.Deserialize<ResourceDataModel>(file);
				result.FileProvider = fileProvider;
				result.ApplyPath(Path.GetDirectoryName(filename));

				ReadExternalFiles(deserializer, result);

				result.Validate();

				return result;
			}
		}

		private void ReadExternalFiles(Deserializer deserializer, ResourceDataModel config)
		{
			foreach (var fontSource in config.FontSources)
			{
				var fontSourcePath = CombinePath(config.Path, fontSource);
				var path = GetDirectoryName(fontSourcePath);

				ReadSources<FontResourceCollection, FontResource>(
					deserializer,
					config,
					fontSourcePath,
					(key, value) =>
					{
						value.ApplyPath(path);
						config.Fonts.Add(key, value);
					});
			}

			foreach (var themeSource in config.ThemeSources)
			{
				var themeSourcePath = CombinePath(config.Path, themeSource);
				var path = GetDirectoryName(themeSourcePath);

				ReadSources<ThemeModelCollection, ThemeModel>(deserializer, config, themeSourcePath,
					(key, value) =>
					{
						value.ApplyPath(path);
						config.Themes.Add(key, value);
					});
			}

			foreach (var facetSource in config.FacetSources)
			{
				var facetSourcePath = CombinePath(config.Path, facetSource);
				var path = GetDirectoryName(facetSourcePath);

				ReadSources<FacetModelCollection, FacetModel>(deserializer, config, facetSourcePath,
					(key, value) => config.Facets.Add(key, value));
			}
		}

		private string GetDirectoryName(string filePath)
		{
			if (filePath.Contains("/") || filePath.Contains("\\"))
			{
				var lastSlash = filePath.LastIndexOf("/");
				var lastBackslash = filePath.LastIndexOf("\\");
				var lastIndex = Math.Max(lastSlash, lastBackslash);

				return filePath.Substring(0, lastIndex);
			}
			else
			{
				return "";
			}
		}

		private string CombinePath(string rootPath, string localFile)
		{
			var validRoot = !string.IsNullOrWhiteSpace(rootPath);
			var validLocal = !string.IsNullOrWhiteSpace(localFile);

			if (validRoot && validLocal)
			{
				return rootPath + "/" + localFile;
			}
			else if (validRoot)
			{
				return rootPath;
			}
			else if (validLocal)
			{
				return localFile;
			}

			return "";
		}

		private void ReadSources<TCollection, TItem>(Deserializer deserializer, ResourceDataModel config,
			string filename, Action<string, TItem> store)
			where TCollection : IEnumerable<KeyValuePair<string, TItem>>
		{
			using (var file = new StreamReader(fileProvider.OpenRead(filename)))
			{
				try
				{
					var result = deserializer.Deserialize<TCollection>(file);

					foreach (var kvp in result)
					{
						store(kvp.Key, kvp.Value);
					}
				}
				catch (Exception e)
				{
					throw new AgateResourceException($"Exception while reading {filename}.", e);
				}
			}
		}
	}
}
