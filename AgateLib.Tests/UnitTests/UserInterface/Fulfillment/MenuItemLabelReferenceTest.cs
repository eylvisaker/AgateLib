using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Fulfillment
{
	[TestClass]
	public class MenuItemLabelReferenceTest : FacetUnitTest, IUserInterfaceFacet
	{
		protected override string FacetSource { get; } = @"
facets:
    NonWrappingLayout: 
    -   name: window_1
        type: window
        position: 20 20
        size: 275 300
        children:
        -   name: menu_1
            type: menu
            menu-items:
            -   name: label_1
                text: First Label
            -   text: Second Label that is really long and goes on forever
            -   text: Third Label
            -   text: Fourth Label
            -   text: Fifth Label    
    -   name: window_2
        type: window
        position: 340 20
        size: 275 300
        children:
        -   name: menu_2
            type: menu
            layout:
                direction: row
            menu-items:
            -   text: This is a long message that should wrap around.
            -   text: This is another, even longer message that should also wrap around, even further than the first!
";

		[BindTo("label_1")]
		public Label Label_1 { get; set; }

		[TestMethod]
		public void LabelReferenceAssigned()
		{
			Assert.IsNotNull(Label_1);
		}
	}
}
