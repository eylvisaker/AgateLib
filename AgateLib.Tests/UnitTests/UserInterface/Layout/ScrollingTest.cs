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

namespace AgateLib.UnitTests.UserInterface.Layout
{
	[TestClass]
	public class ScrollingTest : FacetUnitTest, IUserInterfaceFacet
	{
		protected override string FacetSource { get; } = @"
facets:
    MenuScroll: 
    -   name: Window
        type: window
        position: 20 20
        size: 275 50
        children:
        -   name: Header
            type: label
            text: Menu Header
        -   name: Menu
            type: menu
            overflow: scroll
            menu-items:
            -   text: First menu item
            -   text: Second menu item that is really long and goes on forever
            -   text: Third menu item. This one is pretty long and has multiple sentences. (Well, one of them is a sentence fragment.)
            -   text: Fourth menu item.
            -   text: Fifth menu item.
            -   text: Sixth menu item.
";

		public Menu Menu { get; set; }

		[TestMethod]
		public void MenuItemSizes()
		{
			Assert.AreEqual(new Rectangle(0, 0, 275, 10), Menu.MenuItems.Skip(0).First().WidgetRect);
			Assert.AreEqual(new Rectangle(0, 10, 275, 20), Menu.MenuItems.Skip(1).First().WidgetRect);
			Assert.AreEqual(new Rectangle(0, 30, 275, 30), Menu.MenuItems.Skip(2).First().WidgetRect);
			Assert.AreEqual(new Rectangle(0, 60, 275, 10), Menu.MenuItems.Skip(3).First().WidgetRect);
			Assert.AreEqual(new Rectangle(0, 70, 275, 10), Menu.MenuItems.Skip(4).First().WidgetRect);
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

			Assert.AreEqual(new Point(0, 50), Menu.ScrollOffset);
		}

		[TestMethod]
		public void ScrollToLastItemThenFirst()
		{
			Menu.ScrollToWidget(Menu.MenuItems.Last());
			Assert.AreEqual(new Point(0, 50), Menu.ScrollOffset);

			Menu.ScrollToWidget(Menu.MenuItems.First());
			Assert.AreEqual(new Point(0, 0), Menu.ScrollOffset);
		}
	}
}
