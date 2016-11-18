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
		string facetFilename = "facets.yaml";

		string configyaml = @"
font-sources: 
- fonts.yaml

theme-sources:
- themes.yaml

facet-sources:
- facets.yaml";

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
		string facetsyaml = @"
default_facet: 
    window_A:
        type: window
        x: 270
        y: 10
        width: 275
        height: 300
        children:
            menu_1:
                type: menu
                dock: fill
";
		[TestInitialize]
		public void Initialize()
		{
			var fileProvider = new Mock<IReadFileProvider>();

			SetupFile(fileProvider, configFilename, configyaml);
			SetupFile(fileProvider, fontsFilename, fontsyaml);
			SetupFile(fileProvider, themeFilename, themesyaml);
			SetupFile(fileProvider, facetFilename, facetsyaml);

			Assets.UserInterfaceAssets = fileProvider.Object;
		}

		private void SetupFile(Mock<IReadFileProvider> fileProvider, string filename, string contents)
		{
			fileProvider
				.Setup(x => x.OpenReadAsync(filename))
				.Returns(() => Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(contents))));
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


		[TestMethod]
		public void ReadFacetModelFromSeparateFile()
		{
			var configModel = UserInterfaceDataLoader.Config(configFilename);

			Assert.AreEqual(1, configModel.Facets.Count);
			Assert.AreEqual("default_facet", configModel.Facets.Keys.First());

			var facet = configModel.Facets["default_facet"];
			var window = facet["window_A"];
			var menu = window.Children["menu_1"];

			Assert.AreEqual("window", window.Type);
			Assert.AreEqual("menu", menu.Type);
		}
	}
}
