using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib.ApplicationModels;
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
			VenusRenderer renderer = new VenusRenderer();
			VenusLayoutEngine layout = new VenusLayoutEngine();

			Gui gui = new Gui(renderer, layout);

			var ui = new InterfaceContainer();
			layout.InitializeWidgets("test", ui);

			
		}

		class InterfaceContainer : IUserInterfaceContainer
		{
			public Window testWindow;
			public Panel testPanel;
			public TextBox testTextBox;
		}
	}
}
