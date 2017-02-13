using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.IO;
using AgateLib.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AgateLib.UnitTests.Resources
{
	[TestClass]
	public class ResourceDataLoaderThemeReadingTests : AgateUnitTest
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
            size: 10
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
                    size: 6

    label:
        text-color: navy";

		
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
		public void ReadThemeModel()
		{
			var resourceData = new ResourceDataLoader().Load(filename);

			Assert.AreEqual(1, resourceData.Themes.Count);
			Assert.AreEqual("default", resourceData.Themes.Keys.First());
			var theme = resourceData.Themes["default"];

			var window = theme["window"];
			Assert.AreEqual("ui_back_1.png", window.Background.Image);
		}
	}
}
