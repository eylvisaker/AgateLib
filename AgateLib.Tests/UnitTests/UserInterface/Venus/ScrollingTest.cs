using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Venus
{
	[TestClass]
	public class ScrollingTest : AgateUnitTest, IUserInterfaceFacet
	{
		string facetSource = @"
facets:
    MenuScroll: 
        Window:
            type: window
            position: 20 20
            size: 275 50
            children:
                Header:
                    type: label
                    text: Menu Header
                Menu:
                    type: menu
                    overflow: scroll
                    children:
                        1:
                            type: menuitem
                            children:
                                label_1: 
                                    type: label
                                    text: First menu item
                        2:
                            type: menuitem
                            children:
                                1: 
                                    type: label
                                    text: Second menu item that is really long and goes on forever
                        3:
                            type: menuitem
                            children:
                                1:
                                    type: label
                                    text: Third menu item. This one is pretty long and has multiple sentences. (Well, one of them is a sentence fragment.)
                        4:  
                            type: menuitem
                            children:
                                1:
                                    type: label
                                    text: Fourth menu item.
                        5:  
                            type: menuitem
                            children:
                                1:
                                    type: label
                                    text: Fifth menu item.
                        6:  
                            type: menuitem
                            children:
                                1:
                                    type: label
                                    text: Fifth menu item.
        DebugWindow:
            type: window
            position: 340 20
            size: 300 200
            children:
                DebugLabel: 
                    type: label
                    text: Debug Message
";

		public string FacetName { get { return "MenuScroll"; } }

		public Gui InterfaceRoot { get; set; }

		public Menu Menu { get; set; }

		[TestInitialize]
		public void Initialize()
		{
			var resourceManager = new AgateResourceManager(
				new ResourceDataLoader().LoadFromText(facetSource));

			resourceManager.InitializeContainer(this);

			InterfaceRoot.LayoutEngine.UpdateLayout(InterfaceRoot);
		}

		[TestMethod]
		public void OverflowItemSizeRestricted()
		{
			Assert.AreEqual(new Rectangle(0, 10, 275, 40),
				Menu.WidgetRect);
		}

		[TestMethod]
		public void ScrollToLastItem()
		{
			Menu.ScrollToWidget(Menu.MenuItems.Last());

			Assert.AreEqual(new Point(-1, -1), Menu.ScrollOffset);
		}
	}
}
