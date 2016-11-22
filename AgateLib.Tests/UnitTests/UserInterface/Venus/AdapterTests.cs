using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.Resources;
using AgateLib.Resources.Managers;
using AgateLib.UnitTests.Resources;
using AgateLib.UserInterface.DataModel;
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
		
		[TestMethod]
		public void AdapterBorderProperties()
		{
			const string image = "abc123.png";
			LayoutBox borderSlice = new LayoutBox { Left = 2, Top = 3, Right = 4, Bottom = 5 };

			var windowTheme = uiManager.Adapter.ThemeData.First().Value["window"];
			windowTheme.Border = new WidgetBorderModel();
			windowTheme.Border.Image = image;
			windowTheme.Border.Slice = borderSlice;

			var facet = new ResourceManagerInitializer.TestFacet();
			uiManager.InitializeFacet(facet);

			var style = adapter.StyleOf(facet.WindowA);
			var border = style.Border;

			Assert.AreEqual(image, style.Border.Image);
			Assert.AreEqual(borderSlice, style.Border.ImageSlice);
			Assert.AreEqual("ui_back_1.png", style.Border.Left.Width);
			Assert.AreEqual("ui_back_1.png", style.Border.Left.Color);
			Assert.AreEqual("ui_back_1.png", style.Border.Top.Width);
			Assert.AreEqual("ui_back_1.png", style.Border.Top.Color);
			Assert.AreEqual("ui_back_1.png", style.Border.Right.Width);
			Assert.AreEqual("ui_back_1.png", style.Border.Right.Color);
			Assert.AreEqual("ui_back_1.png", style.Border.Bottom.Width);
			Assert.AreEqual("ui_back_1.png", style.Border.Bottom.Color);
		}
	}
}
