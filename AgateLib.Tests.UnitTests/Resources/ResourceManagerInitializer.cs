using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.IO;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AgateLib.UnitTests.Resources
{
	public class ResourceManagerInitializer : IDisposable
	{
		const string resourceFilename = "test.yaml";
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
      source-rect: 0 2 8 30
      right-overhang: 1";

		string themesyaml = @"
default:
    window:
        background:
            image: ui_back_1.png
            color: blue
            repeat: none
            clip: content
            position: 4 3";

		string facetsyaml = @"
default_facet: 
-   name: window_A
    type: window
    position: 270 10
    size: 275 300
    children:
    -   name: menu_1
        type: menu
";


		public class TestFacet : IUserInterfaceFacet
		{
			public string FacetName { get; set; } = "default_facet";

			public FacetScene InterfaceRoot { get; set; }

			[BindTo("window_A")]
			public Window WindowA { get; set; }

			[BindTo("menu_1")]
			public Menu MenuB { get; set; }
		}

		public ResourceManagerInitializer()
		{
			var fileProvider = new Mock<IReadFileProvider>();

			SetupFile(fileProvider, resourceFilename, configyaml);
			SetupFile(fileProvider, fontsFilename, fontsyaml);
			SetupFile(fileProvider, themeFilename, themesyaml);
			SetupFile(fileProvider, facetFilename, facetsyaml);

			AgateApp.Assets = fileProvider.Object;

			DataModel = new ResourceDataLoader().Load(resourceFilename);
			Manager = new AgateResourceManager(DataModel);
		}

		public void Dispose()
		{
			Manager.Dispose();
		}

		public ResourceDataModel DataModel { get; set; }
		public AgateResourceManager Manager { get; set; }
		
		private void SetupFile(Mock<IReadFileProvider> fileProvider, string filename, string contents)
		{
			fileProvider
				.Setup(x => x.OpenReadAsync(filename))
				.Returns(() => Task.FromResult((Stream)new MemoryStream(Encoding.UTF8.GetBytes(contents))));
		}

	}
}
