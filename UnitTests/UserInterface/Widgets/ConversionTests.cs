using AgateLib.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets.Tests
{

	class FakeRenderer : IGuiRenderer
	{
		public void Draw(Gui gui)
		{
		}
		public void Update(Gui gui, double deltaTime)
		{ }
	}
	class FakeLayout : IGuiLayoutEngine
	{
		public void UpdateLayout(Gui gui)
		{
		}
	}

	[TestClass]
	public class ConversionTests
	{
		Gui gui;
		Window window;
		Label label1;
		Label label2;
		Label label3;
		Panel panel;

		[TestInitialize]
		public void initialize()
		{
			gui = new Gui(new FakeRenderer(), new FakeLayout());
			gui.Desktop.Children.Add(new Window { X = 40, Y = 50 });

			window = gui.Desktop.Windows.First();

			panel = new Panel() { X = 15, Y = 80 };

			label1 = new Label("label1") { X = 10, Y = 15 };
			label2 = new Label("label2") { X = 40, Y = 18 };
			label3 = new Label("label3") { X = 5, Y = 6 };

			window.Children.Add(label1);
			window.Children.Add(panel);

			panel.Children.Add(label2);
			panel.Children.Add(label3);
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
	}
}
