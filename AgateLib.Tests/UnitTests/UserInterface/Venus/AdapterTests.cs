using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Resources;
using AgateLib.Resources.Managers;
using AgateLib.UnitTests.Resources;
using AgateLib.UserInterface.Venus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Venus
{
	[TestClass]
	public class AdapterTests : AgateUnitTest
	{
		private AgateResourceManager resources;
		private UserInterfaceResourceManager uiManager;
		private VenusLayoutEngine layoutEngine;
		private VenusWidgetAdapter adapter;

		[TestInitialize]
		public void Initialize()
		{
			ResourceManagerInitializer initializer = new ResourceManagerInitializer();

			resources = initializer.Manager;
			uiManager = (UserInterfaceResourceManager)resources.UserInterface;
			
			layoutEngine = (VenusLayoutEngine)uiManager.LayoutEngine;
			adapter = (VenusWidgetAdapter)uiManager.Adapter;
		}

		[TestMethod]
		public void AdapterBackgroundImage()
		{
			var facet = new ResourceManagerInitializer.TestFacet();
			uiManager.InitializeFacet(facet);

			var style = adapter.StyleOf(facet.WindowA);

			Assert.AreEqual("ui_back_1.png", style.Background.Image);
		}
	}
}
