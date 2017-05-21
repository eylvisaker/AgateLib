using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Platform;
using AgateLib.Platform.Test;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Resources
{
	[TestClass]
	public class ThemeResourcePathTests : AgateUnitTest
	{
		class InterfaceContainer : IUserInterfaceFacet
		{
			public string FacetName => "DefaultFacet";

			public FacetScene InterfaceRoot { get; set; }

			public Window window { get; set; }
		}

		FakeReadFileProvider fileProvider = new FakeReadFileProvider();
		AgateResourceManager resources;

		protected override void Dispose(bool disposing)
		{
			resources?.Dispose();
		}

		[TestMethod]
		public void ThemeResourcePathsAllRoot()
		{
			Initialize("", "", "", "");

			Assert.AreEqual(1, fileProvider.ReadCount("ui_back_1.png"));
			Assert.AreEqual(1, fileProvider.ReadCount("ui_back_2.png"));
			Assert.AreEqual(1, fileProvider.ReadCount("ui_border_1.png"));
			Assert.AreEqual(1, fileProvider.ReadCount("ui_border_2.png"));
		}

		[TestMethod]
		public void ThemeResourcePathsSeparateImagePath()
		{
			Initialize("", "", "", "Images");

			Assert.AreEqual(1, fileProvider.ReadCount("Images/ui_back_1.png"));
			Assert.AreEqual(1, fileProvider.ReadCount("Images/ui_back_2.png"));
			Assert.AreEqual(1, fileProvider.ReadCount("Images/ui_border_1.png"));
			Assert.AreEqual(1, fileProvider.ReadCount("Images/ui_border_2.png"));
		}

		[TestMethod]
		public void ThemeResourcePaths()
		{
			Initialize("Assets", "Themes", "Facets", "Images");

			Assert.AreEqual(1, fileProvider.ReadCount("Assets/Themes/Images/ui_back_1.png"));
			Assert.AreEqual(1, fileProvider.ReadCount("Assets/Themes/Images/ui_back_2.png"));
			Assert.AreEqual(1, fileProvider.ReadCount("Assets/Themes/Images/ui_border_1.png"));
			Assert.AreEqual(1, fileProvider.ReadCount("Assets/Themes/Images/ui_border_2.png"));
		}

		private void Initialize(
			string rootFilePath, 
			string themeFilePath, 
			string facetFilePath, 
			string themeImageFilePath)
		{
			string rootFile = $@"
theme-sources:
-   {JoinPath(themeFilePath, "themeFile.yaml")}

facet-sources:
-   {JoinPath(facetFilePath, "facetFile.yaml")}";

			string facetFile = @"
DefaultFacet:
-   type: window
    name: window
    position: 10 10
    size: 300 300 
    children:
    -   name: menu
        type: menu
        menu-items:
        -   text: Hello
        -   text: Waffles
";

			string themeFile = $@"
default:
    window:
        background:
            image: {JoinPath(themeImageFilePath, "ui_back_1.png")}
        border:
            image: {JoinPath(themeImageFilePath, "ui_border_1.png")}

    menuitem:
        box:
            padding: 8
        state:
            selected:
                background:
                    image: {JoinPath(themeImageFilePath, "ui_back_2.png")}
                border:
                    image: {JoinPath(themeImageFilePath, "ui_border_2.png")}
                    size: 6
            hover:
                background:
                    color: 00ffff

    label:
        text-color: navy
";

			fileProvider = new FakeReadFileProvider();
			fileProvider.Add(JoinPath(rootFilePath, "resources.yaml"), rootFile);
			fileProvider.Add(JoinPath(rootFilePath, facetFilePath, "facetFile.yaml"), facetFile);
			fileProvider.Add(JoinPath(rootFilePath, themeFilePath, "themeFile.yaml"), themeFile);

			var imagePath = JoinPath(rootFilePath, themeFilePath, themeImageFilePath);

			fileProvider.Add(JoinPath(imagePath, "ui_back_1.png"), "");
			fileProvider.Add(JoinPath(imagePath, "ui_back_2.png"), "");
			fileProvider.Add(JoinPath(imagePath, "ui_border_1.png"), "");
			fileProvider.Add(JoinPath(imagePath, "ui_border_2.png"), "");

			var dataLoader = new ResourceDataLoader(fileProvider);
			var dataModel = dataLoader.Load(JoinPath(rootFilePath, "resources.yaml"));

			var container = new InterfaceContainer();

			resources = new AgateResourceManager(dataModel);
			resources.InitializeContainer(container);

			container.InterfaceRoot.OnUpdate(ClockTimeSpan.Zero, false);
			container.InterfaceRoot.Draw();
		}

		private string JoinPath(params string[] pathItems)
		{
			return string.Join("/", pathItems.Where(x => !string.IsNullOrWhiteSpace(x)));
		}
		
	}
}
