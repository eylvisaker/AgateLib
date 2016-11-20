using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.IO;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AgateLib.UnitTests.Resources
{
	public class ResourceManagerTestHarness : AgateUnitTest
	{
		protected const string resourceFilename = "test.yaml";
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

		protected ResourceDataModel DataModel { get; set; }
		protected ResourceManager Manager { get; set; }

		[TestInitialize]
		public virtual void Initialize()
		{
			var fileProvider = new Mock<IReadFileProvider>();

			SetupFile(fileProvider, resourceFilename, configyaml);
			SetupFile(fileProvider, fontsFilename, fontsyaml);
			SetupFile(fileProvider, themeFilename, themesyaml);
			SetupFile(fileProvider, facetFilename, facetsyaml);

			Assets.UserInterfaceAssets = fileProvider.Object;

			DataModel = new ResourceDataLoader().Load(resourceFilename);
			Manager = new ResourceManager(DataModel);
		}

		private void SetupFile(Mock<IReadFileProvider> fileProvider, string filename, string contents)
		{
			fileProvider
				.Setup(x => x.OpenReadAsync(filename))
				.Returns(() => Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(contents))));
		}

	}
}
