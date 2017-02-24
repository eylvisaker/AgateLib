using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.IO;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YamlDotNet.Serialization;

namespace AgateLib.UnitTests.Resources
{
	[TestClass]
	public class ResourceDataLoaderReferenceReadingTests : AgateUnitTest
	{
		ResourceManagerInitializer initializer;
		ResourceDataModel dataModel;
		AgateResourceManager resources;

		[TestInitialize]
		public void Initialize()
		{
			initializer = new ResourceManagerInitializer();

			dataModel = initializer.DataModel;
			resources = initializer.Manager;
		}

		protected override void Dispose(bool disposing)
		{
			initializer.Dispose();
		}

		[TestMethod]
		public void ReadFontModelFromSeparateFile()
		{
			Assert.AreEqual(1, dataModel.Fonts.Count);
			Assert.AreEqual("MedievalSharp", dataModel.Fonts.Keys.First());

			var font = dataModel.Fonts["MedievalSharp"].First();

			Assert.AreEqual("Fonts/MedievalSharp18.png", font.Image);
			Assert.AreEqual(32, font.Metrics.Keys.First());
			Assert.AreEqual(1, font.Metrics.Values.First().RightOverhang);
		}

		[TestMethod]
		public void ReadThemeModelFromSeparateFile()
		{
			Assert.AreEqual(1, dataModel.Themes.Count);
			Assert.AreEqual("default", dataModel.Themes.Keys.First());

			var theme = dataModel.Themes["default"];
			var window = theme["window"];

			Assert.AreEqual("ui_back_1.png", window.Background.Image);
			Assert.AreEqual(Color.Blue, window.Background.Color);
		}

		[TestMethod]
		public void ReadFacetModelFromSeparateFile()
		{
			Assert.AreEqual(1, dataModel.Facets.Count);
			Assert.AreEqual("default_facet", dataModel.Facets.Keys.First());

			var facet = dataModel.Facets["default_facet"];
			var window = facet.First(x => x.Name == "window_A");
			var menu = window.Children.First(x => x.Name == "menu_1");

			Assert.AreEqual("window", window.Type);
			Assert.AreEqual("menu", menu.Type);
		}
	}
}
