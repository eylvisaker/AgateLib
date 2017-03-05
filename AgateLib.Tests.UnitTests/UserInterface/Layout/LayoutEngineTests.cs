using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.Mathematics.Geometry;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Diagnostics;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Layout
{
	[TestClass]
	public class LayoutEngineTests : AgateUnitTest
	{
		[TestMethod]
		public void LayoutNaturalSizes()
		{
			const int windowSize = 600;
			var initializer = new TestFacetInitializer();

			initializer.InitializeWindow(windowSize, windowSize);

			initializer.Facet.InterfaceRoot.LogWidgetStructure();

			Assert.AreEqual(windowSize, initializer.Facet.window.Width);
			Assert.AreEqual(windowSize, initializer.Facet.window.Height);

			Assert.AreEqual(488, initializer.Facet.container_1.Width);
			Assert.AreEqual(72, initializer.Facet.container_1.Height);
			Assert.AreEqual(new Size(568, 152), initializer.Facet.container_1.WidgetSize);

			Assert.AreEqual("hello", initializer.Facet.label_1.Text);
			Assert.AreEqual(25, initializer.Facet.label_1.Width);
			Assert.AreEqual(10, initializer.Facet.label_1.Height);
		}

		[TestMethod]
		public void LayoutLabelScreenPosition()
		{
			const int windowSize = 15000;
			var initializer = new TestFacetInitializer() { WindowPosition = Point.Zero };

			initializer.InitializeWindow(windowSize, windowSize);

			var screenPt = initializer.Facet.label_1.ClientToScreen(Point.Zero);

			Assert.AreEqual(63, screenPt.X);
			Assert.AreEqual(63, screenPt.Y);
		}

	}
}
