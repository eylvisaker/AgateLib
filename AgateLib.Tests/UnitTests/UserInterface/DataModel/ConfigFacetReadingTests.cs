using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.IO;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YamlDotNet.Serialization;

namespace AgateLib.UnitTests.UserInterface.DataModel
{
	[TestClass]
	public class ConfigFacetReadingTests
	{
		string filename = "test.yaml";
		string yaml = @"
facets:
    TestGui: 
        window_1:
            type: window
            x: 10
            y: 10
            width: 250
            height: 400
        window_2:
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
			fileProvider
				.Setup(x => x.OpenReadAsync(filename))
				.Returns(() => Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(yaml))));

			Assets.UserInterfaceAssets = fileProvider.Object;
		}

		[TestMethod]
		public void ReadFacetModel()
		{
			var configModel = UserInterfaceDataLoader.Config(filename);

			Assert.AreEqual(1, configModel.Facets.Count);
			Assert.AreEqual("TestGui", configModel.Facets.Keys.First());
			var gui = configModel.Themes["TestGui"];

			var window1 = gui["window_1"];
			var window2 = gui["window_2"];

		}
	}
}
