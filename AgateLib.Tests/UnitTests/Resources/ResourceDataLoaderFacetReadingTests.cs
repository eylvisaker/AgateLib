using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;
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
	public class ResourceDataLoaderFacetReadingTests : AgateUnitTest
	{
		string filename = "test.yaml";
		string yaml = @"
facets:
    TestGui: 
    -   name: window_1
        type: window
        position: 10 15
        size: 250 400
    -   name: window_2
        type: window
        position: 270 10
        size: 275 300
        children:
        -   name: menu_1
            type: menu
            visible: false
";

		
		[TestInitialize]
		public void Initialize()
		{
			var fileProvider = new Mock<IReadFileProvider>();
			fileProvider
				.Setup(x => x.OpenReadAsync(filename))
				.Returns(() => Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(yaml))));

			AgateApp.Assets = fileProvider.Object;
		}

		[TestMethod]
		public void ReadFacetModel()
		{
			var configModel = new ResourceDataLoader().Load(filename);

			Assert.AreEqual(1, configModel.Facets.Count);
			Assert.AreEqual("TestGui", configModel.Facets.Keys.First());
			var gui = configModel.Facets["TestGui"];

			var window1 = gui.First(x => x.Name == "window_1");
			var window2 = gui.First(x => x.Name == "window_2");

			Assert.AreEqual("window", window1.Type);
			Assert.AreEqual(10, window1.Position?.X);
			Assert.AreEqual(15, window1.Position?.Y);
			Assert.AreEqual(250, window1.Size?.Width);
			Assert.AreEqual(400, window1.Size?.Height);
			Assert.AreEqual(270, window2.Position?.X);
			Assert.AreEqual(10, window2.Position?.Y);

			var menu = window2.Children.First(x => x.Name == "menu_1");

			Assert.AreEqual("menu", menu.Type);
			Assert.AreEqual(false, menu.Visible);
		}
	}
}
