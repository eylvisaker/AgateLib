using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.StyleModel;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Layout
{
	[TestClass]
	public class MenuScrollingTests : FacetUnitTest, IUserInterfaceFacet
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

		WidgetStyle MenuStyle { get; set; }

		public override void InitializeTest()
		{
			MenuStyle = Adapter.StyleOf(Menu);
		}

		[TestMethod]
		public void MenuItemSizes()
		{
			Assert.AreEqual(new Rectangle(0, 0, 275, 10), Menu.Items.Skip(0).First().WidgetRect);
			Assert.AreEqual(new Rectangle(0, 10, 275, 20), Menu.Items.Skip(1).First().WidgetRect);
			Assert.AreEqual(new Rectangle(0, 30, 275, 30), Menu.Items.Skip(2).First().WidgetRect);
			Assert.AreEqual(new Rectangle(0, 60, 275, 10), Menu.Items.Skip(3).First().WidgetRect);
			Assert.AreEqual(new Rectangle(0, 70, 275, 10), Menu.Items.Skip(4).First().WidgetRect);
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
			MenuStyle.ScrollToWidget(Menu.Items.Last());

			Assert.AreEqual(new Point(0, 50), MenuStyle.View.ScrollOffset);
		}

		[TestMethod]
		public void ScrollToLastItemThenFirst()
		{
			MenuStyle.ScrollToWidget(Menu.Items.Last());
			Assert.AreEqual(new Point(0, 50), MenuStyle.View.ScrollOffset);

			MenuStyle.ScrollToWidget(Menu.Items.First());
			Assert.AreEqual(new Point(0, 0), MenuStyle.View.ScrollOffset);
		}
	}
}
