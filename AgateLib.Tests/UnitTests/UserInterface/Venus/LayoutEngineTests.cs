using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Venus
{
	[TestClass]
	public class LayoutEngineTests : AgateUnitTest
	{
		[TestMethod]
		public void LayoutNaturalSizes()
		{
			const int windowSize = 15000;
			var initializer = new TestFacetInitializer();

			initializer.InitializeWindow(windowSize, windowSize);

			Assert.AreEqual(windowSize, initializer.Facet.window.Width);
			Assert.AreEqual(windowSize, initializer.Facet.window.Height);

			Assert.AreEqual(68, initializer.Facet.container_1.Width);
			Assert.AreEqual(110, initializer.Facet.container_1.Height);

			Assert.AreEqual("hello", initializer.Facet.label_1.Text);
			Assert.AreEqual(96, initializer.Facet.label_1.Width);
			Assert.AreEqual(6, initializer.Facet.label_1.Height);
		}
		
	}
}
