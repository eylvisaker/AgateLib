using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Layout
{
	[TestClass]
	public class AdapterBoxModelTests : AgateUnitTest
	{
		[TestMethod]
		public void AdapterPanelBoxModel()
		{
			const int windowSize = 15000;
			var initializer = new TestFacetInitializer();

			initializer.InitializeWindow(windowSize, windowSize);

			var style = initializer.Adapter.StyleOf(initializer.Facet.container_1);

			Assert.AreEqual(32, style.BoxModel.Padding.Left);
			Assert.AreEqual(16, style.BoxModel.Margin.Left);
			Assert.AreEqual(8, style.BoxModel.Border.Left);
		}
	}
}