using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.Test.UserInterface;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Layout
{
	[TestClass]
	public class BasicScrollingTests : AgateUnitTest
	{
		FacetScene facetScene;
		Window window;
		Panel panel1;
		Label label0;
		Label label1;
		Label label2;
		Label label3;
		Label label4;

		[TestInitialize]
		public void Initialize()
		{
			facetScene = new FacetScene(new FakeRenderer(), new FakeLayout());
			facetScene.Desktop.Windows.Add(window = new Window { X = 40, Y = 50 });

			panel1 = new Panel() { X = 15, Y = 80, Width = 140, Height = 150 };

			label0 = new Label("label0") { X = 10, Y = 15, Width = 48, Height = 8 };
			label1 = new Label("label1") { X = 5048, Y = 15, Width = 48, Height = 8 };
			label2 = new Label("label2") { X = -50, Y = 18, Width = 48, Height = 8 };
			label3 = new Label("label3") { X = 5, Y = 600, Width = 48, Height = 8 };
			label4 = new Label("label4") { X = 35, Y = 16, Width = 48, Height = 8 };

			window.Children.Add(panel1);

			panel1.Children.Add(label0, label1, label2, label3, label4);
			panel1.WidgetStyle.View.AllowScroll = ScrollAxes.Both;

			foreach (var widget in facetScene.Desktop.Descendants)
				widget.WidgetSize = widget.ClientRect.Size;
		}

		[TestMethod]
		public void ScrollToWidgetAlreadyVisible()
		{
			panel1.WidgetStyle.ScrollToWidget(label0);

			Assert.AreEqual(new Point(), panel1.WidgetStyle.View.ScrollOffset);
		}

		[TestMethod]
		public void ScrollToWidgetAtRight()
		{
			panel1.WidgetStyle.ScrollToWidget(label1);
			Assert.AreEqual(new Point(5048+48-140,0), panel1.WidgetStyle.View.ScrollOffset, "Failed to scroll right.");

			panel1.WidgetStyle.ScrollToWidget(label0);
			Assert.AreEqual(new Point(10, 0), panel1.WidgetStyle.View.ScrollOffset, "Failed to scroll left.");
		}

		[TestMethod]
		public void ScrollToWidgetAtLeft()
		{
			panel1.WidgetStyle.ScrollToWidget(label2);

			Assert.AreEqual(new Point(-50, 0), panel1.WidgetStyle.View.ScrollOffset);
		}

		[TestMethod]
		public void ScrollToWidgetAtBottom()
		{
			panel1.WidgetStyle.ScrollToWidget(label3);
			Assert.AreEqual(new Point(0, 608-150), panel1.WidgetStyle.View.ScrollOffset, "Failed to scroll down.");

			panel1.WidgetStyle.ScrollToWidget(label0);
			Assert.AreEqual(new Point(0, 15), panel1.WidgetStyle.View.ScrollOffset, "Failed to scroll up.");
		}
	}
}
