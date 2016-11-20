using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources;
using AgateLib.UnitTests.Resources;
using AgateLib.UserInterface.Venus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Venus
{
	[TestClass]
	public class AdapterTests : AgateUnitTest
	{
		private ResourceManager resources;
		private VenusLayoutEngine layoutEngine;
		private VenusWidgetAdapter adapter;

		[TestInitialize]
		public void Initialize()
		{
			ResourceManagerInitializer initializer = new ResourceManagerInitializer();

			resources = initializer.Manager;

			layoutEngine = (VenusLayoutEngine)ResourceManager.LayoutEngine;
			adapter = (VenusWidgetAdapter)ResourceManager.Adapter;
		}

		[TestMethod]
		public void AdapterDisplayLocation()
		{
			var facet = new ResourceManagerInitializer.TestFacet();
			resources.InitializeFacet(facet);

			var style = adapter.GetStyle(facet.WindowA);

			Assert.AreEqual("ui_back_1.png", style.Background.Image);

		}
	}
}
