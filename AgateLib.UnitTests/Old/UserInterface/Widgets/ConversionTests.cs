using System.Linq;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.Test.UserInterface;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Widgets
{
	[TestClass]
	public class ConversionTests
	{
		FacetScene facetScene;
		Window window;
		Label label1;
		Label label2;
		Label label3;
		Panel panel1;
		Panel panel2;
		Label label4;

		[TestInitialize]
		public void initialize()
		{
			facetScene = new FacetScene(new FakeRenderer(), new FakeLayout());
			facetScene.Desktop.Windows.Add(new Window { X = 40, Y = 50 });

			window = facetScene.Desktop.Windows.First();

			panel1 = new Panel() { X = 15, Y = 80 };
			panel2 = new Panel() { X = 88, Y = 44 };

			label1 = new Label("label1") { X = 10, Y = 15 };
			label2 = new Label("label2") { X = 40, Y = 18 };
			label3 = new Label("label3") { X = 5, Y = 6 };
			label4 = new Label("label4") { X = 35, Y = 16 };

			window.Children.Add(label1);
			window.Children.Add(panel1);

			panel1.Children.Add(label2);
			panel1.Children.Add(label3);
			panel1.Children.Add(panel2);

			panel2.Children.Add(label4);
		}

		[TestMethod]
		public void PointToScreen()
		{
			Assert.AreEqual(new Point(55, 67), label1.ClientToScreen(new Point(5, 2)));
			Assert.AreEqual(new Point(100, 150), label2.ClientToScreen(new Point(5, 2)));
		}

		[TestMethod]
		public void RectToScreen()
		{
			Assert.AreEqual(new Rectangle(55, 67, 88, 37), label1.ClientToScreen(new Rectangle(5, 2, 88, 37)));
			Assert.AreEqual(new Rectangle(100, 150, 48, 87), label2.ClientToScreen(new Rectangle(5, 2, 48, 87)));
	
		}

		[TestMethod]
		public void PointToParent()
		{
			Assert.AreEqual(new Point(48, 25), label2.ClientToParent(new Point(8, 7)));
		}

		void TestRoundTrip(Widget widget, Point pt)
		{
			Assert.AreEqual(pt, widget.ScreenToClient(widget.ClientToScreen(pt)));
		}
		[TestMethod]
		public void RoundTripPoint()
		{
			TestRoundTrip(label3, new Point(8, 24));
		}

		[TestMethod]
		public void ClientLocationOf()
		{
			Assert.AreEqual(new Point(88 + 35, 44 + 16), panel1.ClientLocationOf(label4));
		}
	}
}
