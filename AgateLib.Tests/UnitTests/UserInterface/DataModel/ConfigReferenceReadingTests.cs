using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Geometry;
using AgateLib.IO;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YamlDotNet.Serialization;

namespace AgateLib.UnitTests.UserInterface.DataModel
{
	[TestClass]
	public class ConfigReferenceReadingTests
	{
		string configFilename = "test.yaml";
		string fontsFilename = "fonts.yaml";
		string themeFilename = "themes.yaml";

		string configyaml = @"
font-sources: 
- fonts.yaml

theme-sources:
- themes.yaml";

		string fontsyaml = @"
MedievalSharp:
- name: MedievalSharp18
  image: Fonts/MedievalSharp18.png
  metrics:
    32:
      x: 0
      y: 2
      width: 8
      height: 30
      right-overhang: 1";
		string themesyaml = @"
default:
    window:
        background:
            image: ui_back_1.png
            color: blue";

		[TestInitialize]
		public void Initialize()
		{
			var fileProvider = new Mock<IReadFileProvider>();

			fileProvider
				.Setup(x => x.OpenReadAsync(configFilename))
				.Returns(() => Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(configyaml))));

			fileProvider
				.Setup(x => x.OpenReadAsync(fontsFilename))
				.Returns(() => Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(fontsyaml))));

			fileProvider
				.Setup(x => x.OpenReadAsync(themeFilename))
				.Returns(() => Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(themesyaml))));

			Assets.UserInterfaceAssets = fileProvider.Object;
		}

		[TestMethod]
		public void ReadFontModelFromSeparateFile()
		{
			var configModel = UserInterfaceDataLoader.Config(configFilename);

			Assert.AreEqual(1, configModel.Fonts.Count);
			Assert.AreEqual("MedievalSharp", configModel.Fonts.Keys.First());

			var font = configModel.Fonts["MedievalSharp"].First();

			Assert.AreEqual("Fonts/MedievalSharp18.png", font.Image);
			Assert.AreEqual(32, font.Metrics.Keys.First());
			Assert.AreEqual(1, font.Metrics.Values.First().RightOverhang);

		}

		[TestMethod]
		public void ReadThemeModelFromSeparateFile()
		{
			var configModel = UserInterfaceDataLoader.Config(configFilename);

			Assert.AreEqual(1, configModel.Themes.Count);
			Assert.AreEqual("default", configModel.Themes.Keys.First());

			var theme = configModel.Themes["default"];
			var window = theme["window"];

			Assert.AreEqual("ui_back_1.png", window.Background.Image);
			Assert.AreEqual(Color.Blue, window.Background.Color);
		}
	}
}
