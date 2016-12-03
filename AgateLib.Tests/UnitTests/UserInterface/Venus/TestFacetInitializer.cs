﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UnitTests.UserInterface.Venus
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

		public TestFacet Facet { get; private set; }
		public AgateResourceManager Resources { get; private set; }
		public IGuiLayoutEngine LayoutEngine { get; private set; }
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
			Resources = new AgateResourceManager(resourceDataModel);

			LayoutEngine = Resources.UserInterface.LayoutEngine;
			Adapter = Resources.UserInterface.Adapter;

			Facet = new TestFacet();
			Resources.UserInterface.InitializeFacet(Facet);

			LayoutEngine.UpdateLayout(Facet.InterfaceRoot);
		}
	}
}
