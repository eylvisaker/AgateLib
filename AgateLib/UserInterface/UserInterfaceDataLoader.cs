using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.IO;
using AgateLib.UserInterface.DataModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AgateLib.UserInterface
{
	public static class UserInterfaceDataLoader
	{
		/// <summary>
		/// Searches Assets.UserInterfaceAssets
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static UserInterfaceConfig Config(string filename)
		{
			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(new HyphenatedNamingConvention())
				.Build();

			using (var file = new StreamReader(Assets.UserInterfaceAssets.OpenRead(filename)))
			{
				UserInterfaceConfig result = deserializer.Deserialize<UserInterfaceConfig>(file);

				ReadExternalFiles(deserializer, result);

				return result;
			}
		}

		private static void ReadExternalFiles(Deserializer deserializer, UserInterfaceConfig config)
		{
			foreach(var filename in config.FontSources)
			{
				using (var file = new StreamReader(Assets.UserInterfaceAssets.OpenRead(filename)))
				{
					var result = deserializer.Deserialize<FontModelCollection>(file);

					foreach (var font_kvp in result)
					{
						config.Fonts.Add(font_kvp.Key, font_kvp.Value);
					}
				}
			}
		}
	}
}
