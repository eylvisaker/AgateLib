using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Venus
{
	[TestClass]
	public class LayoutEngineTests : AgateUnitTest
	{
		class Facet : IUserInterfaceFacet
		{
			public Window window;
			public Panel container_1;
			public Panel container_2;
			public Label label_1, label_2, label_3, label_4, label_5;

			public string FacetName { get { return "main"; } }

			public Gui InterfaceRoot { get; set; }
		}

		Facet facet;
		AgateResourceManager resources;
		IGuiLayoutEngine layoutEngine;

		[TestMethod]
		public void LayoutNaturalSizes()
		{
			InitializeWindow(int.MaxValue, int.MaxValue);

			Assert.AreEqual(-10, facet.window.Width);
		}

		private void InitializeWindow(int width, int height)
		{

			string yaml = $@"
themes:
    default:
        window:
            box:
                padding: 256
                margin: 128
            border:
                slice: 64
        panel:
            box:
                padding: 32
                margin: 16
            border:
                slice: 8
        label:
            box:
                padding: 4
                margin: 2
            border:
                slice: 1

facets:
    main: 
        window:
            type: window
            position: 10 15
            size: {width} {height}
            children:
                container_1:
                    type: panel
                    children:
                        label_1:
                            type: label
                            text: hello
                        label_2:
                            type: label
                            text: hello, world
                        label_3:
                            type: label
                            text: hello your face
                container_2:
                    type: panel
                    children:
                        label_4:
                            type: label
                            text: dogs are the best
                        label_5:
                            type: label
                            text: cats suck
";

			var resourceDataModel = new ResourceDataLoader().LoadFromText(yaml);
			resources = new AgateResourceManager(resourceDataModel);

			layoutEngine = resources.UserInterface.LayoutEngine;

			facet = new Facet();
			resources.UserInterface.InitializeFacet(facet);

			layoutEngine.UpdateLayout(facet.InterfaceRoot);
		}
	}
}
