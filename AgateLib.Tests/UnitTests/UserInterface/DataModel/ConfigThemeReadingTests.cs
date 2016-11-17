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
	public class ConfigThemeReadingTests
	{
		string filename = "test.yaml";
		string yaml = @"
themes:
  default:
    window:
        background:
            image: ui_back_1.png
            color: blue
        border:
            image: ui_border_2.png
            slice: 10
        transition:
            type: slide
            direction: top
            time: 2

    menuitem:
        box:
            padding: 8
        state:
            selected:
                border:
                    image: ui_border_1.png
                    slice: 6

    label:
        text-color: navy";

		
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
		public void ReadThemeModel()
		{
			var configModel = UserInterfaceDataLoader.Config(filename);

			Assert.AreEqual(1, configModel.Themes.Count);
			Assert.AreEqual("default", configModel.Themes.Keys.First());
			var theme = configModel.Themes["default"];

			var window = theme["window"];
			Assert.AreEqual("ui_back_1.png", window.Background.Image);
		}
	}
}
