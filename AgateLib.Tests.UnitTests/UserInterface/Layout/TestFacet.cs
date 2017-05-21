using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UnitTests.UserInterface.Layout
{

	class TestFacet : IUserInterfaceFacet
	{
		public string FacetName { get { return "main"; } }

		public Window window { get; set; }
		public Panel container_1 { get; set; }
		public Panel container_2 { get; set; }
		public Label label_1 { get; set; }
		public Label label_2 { get; set; }
		public Label label_3 { get; set; }
		public Label label_4 { get; set; }
		public Label label_5 { get; set; }

		public FacetScene InterfaceRoot { get; set; }
	}

}
