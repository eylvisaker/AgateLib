using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Geometry;
using AgateLib.IO;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YamlDotNet.Serialization;

namespace AgateLib.UnitTests.Resources
{
	[TestClass]
	public class ResourceDataLoaderReferenceReadingTests : ResourceManagerTestHarness
	{
		[TestMethod]
		public void ReadFontModelFromSeparateFile()
		{
			Assert.AreEqual(1, DataModel.Fonts.Count);
			Assert.AreEqual("MedievalSharp", DataModel.Fonts.Keys.First());

			var font = DataModel.Fonts["MedievalSharp"].First();

			Assert.AreEqual("Fonts/MedievalSharp18.png", font.Image);
			Assert.AreEqual(32, font.Metrics.Keys.First());
			Assert.AreEqual(1, font.Metrics.Values.First().RightOverhang);
		}

		[TestMethod]
		public void ReadThemeModelFromSeparateFile()
		{
			Assert.AreEqual(1, DataModel.Themes.Count);
			Assert.AreEqual("default", DataModel.Themes.Keys.First());

			var theme = DataModel.Themes["default"];
			var window = theme["window"];

			Assert.AreEqual("ui_back_1.png", window.Background.Image);
			Assert.AreEqual(Color.Blue, window.Background.Color);
		}

		[TestMethod]
		public void ReadFacetModelFromSeparateFile()
		{
			Assert.AreEqual(1, DataModel.Facets.Count);
			Assert.AreEqual("default_facet", DataModel.Facets.Keys.First());

			var facet = DataModel.Facets["default_facet"];
			var window = facet["window_A"];
			var menu = window.Children["menu_1"];

			Assert.AreEqual("window", window.Type);
			Assert.AreEqual("menu", menu.Type);
		}
	}
}
