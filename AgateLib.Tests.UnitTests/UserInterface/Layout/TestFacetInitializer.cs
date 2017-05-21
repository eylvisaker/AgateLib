using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.Test;
using AgateLib.Resources;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UnitTests.UserInterface.Layout
{
	class TestFacetInitializer
	{
		public int WindowPadding { get; set; } = 256;
		public int WindowMargin { get; set; } = 128;
		public int WindowBorder { get; set; } = 64;
		public int PanelPadding { get; set; } = 32;
		public int PanelMargin { get; set; } = 16;
		public int PanelBorder { get; set; } = 8;
		public int LabelPadding { get; set; } = 4;
		public int LabelMargin { get; set; } = 2;
		public int LabelBorder { get; set; } = 1;

		public Point WindowPosition { get; set; } = new Point(10, 15);

		public TestFacet Facet { get; private set; }
		public AgateResourceManager Resources { get; private set; }
		public IFacetLayoutEngine LayoutEngine { get; private set; }
		public IWidgetAdapter Adapter { get; private set; }

		public void InitializeWindow(int width, int height)
		{

			string yaml = $@"
themes:
    default:
        window:
            box:
                padding: {WindowPadding}
                margin: {WindowMargin}
            border:
                size: {WindowBorder}
        panel:
            box:
                padding: {PanelPadding}
                margin: {PanelMargin}
            border:
                size: {PanelBorder}
        label:
            box:
                padding: {LabelPadding}
                margin: {LabelMargin}
            border:
                size: {LabelBorder}

facets:
    main: 
    -   name: window
        type: window
        position: {WindowPosition.X} {WindowPosition.Y}
        size: {width} {height}
        children:
        -   name: container_1
            type: panel
            children:
            -   name: label_1
                type: label
                text: hello
            -   name: label_2
                type: label
                text: hello, world
            -   name: label_3
                type: label
                text: hello your face
        -   name: container_2
            type: panel
            children:
            -   name: label_4
                type: label
                text: dogs are the best
            -   name: label_5
                type: label
                text: cats suck
";

			var fileProvider = new FakeReadFileProvider();
			fileProvider.Add("resources.yaml", yaml);

			var resourceDataModel = new ResourceDataLoader(fileProvider)
				.Load("resources.yaml");

			Resources = new AgateResourceManager(resourceDataModel);

			Facet = new TestFacet();
			Resources.UserInterface.InitializeFacet(Facet);

			LayoutEngine = Facet.InterfaceRoot.LayoutEngine;
			Adapter = Facet.InterfaceRoot.Renderer.Adapter;

			LayoutEngine.UpdateLayout(Facet.InterfaceRoot);
		}
	}
}
