using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.Resources;
using AgateLib.Resources.Managers;
using AgateLib.UnitTests.Resources;
using AgateLib.UserInterface.Rendering;
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
		public void AdapterBackgroundProperties()
		{
			var facet = new ResourceManagerInitializer.TestFacet();
			uiManager.InitializeFacet(facet);

			var style = adapter.StyleOf(facet.WindowA);

			Assert.AreEqual("ui_back_1.png", style.Background.Image);
			Assert.AreEqual(Color.Blue, style.Background.Color);
			Assert.AreEqual(BackgroundRepeat.None, style.Background.Repeat);
			Assert.AreEqual(BackgroundClip.Content, style.Background.Clip);
			Assert.AreEqual(new Point(4, 3), style.Background.Position);
		}
	}
}
