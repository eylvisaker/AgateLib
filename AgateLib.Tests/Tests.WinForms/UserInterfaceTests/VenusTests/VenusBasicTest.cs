using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib.ApplicationModels;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Venus;
using AgateLib.UserInterface.Widgets;
using Panel = AgateLib.UserInterface.Widgets.Panel;

namespace AgateLib.Testing.UserInterfaceTests.VenusTests
{
	class VenusBasicTest : ISerialModelTest
	{
		public string Name => "Venus Basic Test";
		public string Category => "User Interface";

		public void ModifyModelParameters(SerialModelParameters parameters)
		{
			
		}

		public void EntryPoint()
		{
			var adapter = new VenusWidgetAdapter();
			var renderer = new AgateUserInterfaceRenderer(adapter);
			VenusLayoutEngine layout = new VenusLayoutEngine(adapter);

			Gui gui = new Gui(renderer, layout);

			var ui = new InterfaceContainer();
			layout.InitializeWidgets("test", ui);

			
		}

		class InterfaceContainer : IUserInterfaceFacet
		{
			public Window testWindow;
			public Panel testPanel;
			public TextBox testTextBox;

			public string FacetName { get; set; }

			public Gui InterfaceRoot { get; set; }
		}
	}
}
