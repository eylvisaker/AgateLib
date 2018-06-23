using System.IO;
using AgateLib;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;

namespace ManualTests.AgateLib.ReplaceMe
{
	[Singleton]
	public class TextRepository : ITextRepository
	{
		public TextRepository(string rootDirectory = "Text/en-US")
		{
			//var culture = GetSupportedCulture();

			//var dir = "Text/" + culture;

			//var fp = AgateApp.Assets.Subdirectory(dir);

			//fileProvider = fp;

			RootDirectory = rootDirectory;
		}

		public string RootDirectory { get; set; }

		public string this[string key]
		{
			get
			{
				try
				{
					using (var reader = new StreamReader(TitleContainer.OpenStream(key + ".txt")))
					{
						return reader.ReadToEnd();
					}
				}
				catch (FileNotFoundException notFound)
				{
					return "Missing text asset: " + key;
				}
			}
		}
	}
}
