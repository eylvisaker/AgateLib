using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Platform.Test;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Resources
{
	[TestClass]
	public class FontResourcePathTests : AgateUnitTest
	{
		class FontContainer
		{
			public IFont AgateFont { get; set; }
		}

		[TestMethod]
		public void FontResourcePathsAllRoot()
		{
			string rootFile = @"
font-sources:
-   fontfile.yaml";

			string fontfile = @"
AgateFont:
-   name: AgateFont-8
    image: image.png
    size: 8";

			var fileProvider = new FakeReadFileProvider();
			fileProvider.Add("resources.yaml", rootFile);
			fileProvider.Add("fontfile.yaml", fontfile);
			fileProvider.Add("image.png", "");

			var dataLoader = new ResourceDataLoader(fileProvider);
			var dataModel = dataLoader.Load("resources.yaml");

			var container = new FontContainer();

			var resources = new AgateResourceManager(dataModel);
			resources.InitializeContainer(container);

			Assert.AreEqual(1, fileProvider.ReadCount("image.png"));
		}

		[TestMethod]
		public void FontResourcePathsFontSubFolder()
		{
			string rootFile = @"
font-sources:
-   fontfile.yaml";

			string fontfile = @"
AgateFont:
-   name: AgateFont-8
    image: Fonts/image.png
    size: 8";

			var fileProvider = new FakeReadFileProvider();
			fileProvider.Add("resources.yaml", rootFile);
			fileProvider.Add("fontfile.yaml", fontfile);
			fileProvider.Add("Fonts/image.png", "");

			var dataLoader = new ResourceDataLoader(fileProvider);
			var dataModel = dataLoader.Load("resources.yaml");

			var container = new FontContainer();

			var resources = new AgateResourceManager(dataModel);
			resources.InitializeContainer(container);

			Assert.AreEqual(1, fileProvider.ReadCount("Fonts/image.png"));
		}

		[TestMethod]
		public void FontResourcePathsSourceFileInSubFolder()
		{
			string rootFile = @"
font-sources:
-   UserInterface/fontfile.yaml";

			string fontfile = @"
AgateFont:
-   name: AgateFont-8
    image: Fonts/image.png
    size: 8";

			var fileProvider = new FakeReadFileProvider();
			fileProvider.Add("resources.yaml", rootFile);
			fileProvider.Add("UserInterface/fontfile.yaml", fontfile);
			fileProvider.Add("UserInterface/Fonts/image.png", "");

			var dataLoader = new ResourceDataLoader(fileProvider);
			var dataModel = dataLoader.Load("resources.yaml");

			var container = new FontContainer();

			var resources = new AgateResourceManager(dataModel);
			resources.InitializeContainer(container);

			Assert.AreEqual(1, fileProvider.ReadCount("UserInterface/Fonts/image.png"));
		}

		[TestMethod]
		public void FontResourcePathsNonRootFile()
		{
			string rootFile = @"
font-sources:
-   UserInterface/fontfile.yaml";

			string fontfile = @"
AgateFont:
-   name: AgateFont-8
    image: Fonts/image.png
    size: 8";

			var fileProvider = new FakeReadFileProvider();
			fileProvider.Add("Assets/resources.yaml", rootFile);
			fileProvider.Add("Assets/UserInterface/fontfile.yaml", fontfile);
			fileProvider.Add("Assets/UserInterface/Fonts/image.png", "");

			var dataLoader = new ResourceDataLoader(fileProvider);
			var dataModel = dataLoader.Load("Assets/resources.yaml");

			var container = new FontContainer();

			var resources = new AgateResourceManager(dataModel);
			resources.InitializeContainer(container);

			Assert.AreEqual(1, fileProvider.ReadCount("Assets/UserInterface/Fonts/image.png"));
		}

		[TestMethod]
		public void FontResourceDirectIncludeWithPath()
		{
			string rootFile = @"
fonts:
    AgateFont:
    - name: AgateFont-8
      image: Fonts/image.png
      size: 8
      metrics:
        32:
          source-rect: 1 2 3 13
          kerning-pairs: {}";

			var fileProvider = new FakeReadFileProvider();
			fileProvider.Add("Assets/UserInterface/resources.yaml", rootFile);
			fileProvider.Add("Assets/UserInterface/Fonts/image.png", "");

			var dataLoader = new ResourceDataLoader(fileProvider);
			var dataModel = dataLoader.Load("Assets/UserInterface/resources.yaml");

			var container = new FontContainer();

			var resources = new AgateResourceManager(dataModel);
			resources.InitializeContainer(container);
		}

	}
}
