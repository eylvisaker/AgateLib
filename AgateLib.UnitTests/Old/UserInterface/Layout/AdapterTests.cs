using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Resources;
using AgateLib.Resources.DataModel;
using AgateLib.Resources.Managers;
using AgateLib.Resources.Managers.UserInterface;
using AgateLib.UnitTests.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Layout;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Layout
{
	[TestClass]
	public class AdapterTests : AgateUnitTest
	{
		private ResourceDataModel data;
		private AgateResourceManager resources;
		private UserInterfaceResourceManager uiManager;

		[TestInitialize]
		public void Initialize()
		{
			ResourceManagerInitializer initializer = new ResourceManagerInitializer();

			data = initializer.DataModel;
			resources = initializer.Manager;
			uiManager = (UserInterfaceResourceManager)resources.UserInterface;
		}

		[TestMethod]
		public void AdapterBackgroundProperties()
		{
			var facet = new ResourceManagerInitializer.TestFacet();
			uiManager.InitializeFacet(facet);

			var adapter = facet.InterfaceRoot.Renderer.Adapter;
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

			InitializeBorder(image, borderSlice);

			var facet = new ResourceManagerInitializer.TestFacet();
			uiManager.InitializeFacet(facet);

			var adapter = facet.InterfaceRoot.Renderer.Adapter;
			var style = adapter.StyleOf(facet.WindowA);
			
			Assert.AreEqual(image, style.Border.Image);
			Assert.AreEqual(borderSlice, style.Border.ImageSlice);
			//Assert.AreEqual("ui_back_1.png", style.Border.Left.Width);
			//Assert.AreEqual("ui_back_1.png", style.Border.Left.Color);
			//Assert.AreEqual("ui_back_1.png", style.Border.Top.Width);
			//Assert.AreEqual("ui_back_1.png", style.Border.Top.Color);
			//Assert.AreEqual("ui_back_1.png", style.Border.Right.Width);
			//Assert.AreEqual("ui_back_1.png", style.Border.Right.Color);
			//Assert.AreEqual("ui_back_1.png", style.Border.Bottom.Width);
			//Assert.AreEqual("ui_back_1.png", style.Border.Bottom.Color);
		}

		[TestMethod]
		public void AdapterBoxModel()
		{
			const string image = "abc123.png";
			LayoutBox borderSlice = new LayoutBox { Left = 2, Top = 3, Right = 4, Bottom = 5 };

			InitializeBorder(image, borderSlice);

			var facet = new ResourceManagerInitializer.TestFacet();
			uiManager.InitializeFacet(facet);

			var adapter = facet.InterfaceRoot.Renderer.Adapter;
			var style = adapter.StyleOf(facet.WindowA);

			Assert.AreEqual(2, style.BoxModel.Border.Left);
			Assert.AreEqual(3, style.BoxModel.Border.Top);
			Assert.AreEqual(4, style.BoxModel.Border.Right);
			Assert.AreEqual(5, style.BoxModel.Border.Bottom);
		}

		[TestMethod]
		public void AdapterFontProperties()
		{
			const string fontname = "Times New Roman";
			const int fontSize = 14;
			const FontStyles fontStyles = FontStyles.Bold | FontStyles.Italic;

			InitializeFontProperties(fontname, fontSize, fontStyles, Color.LightBlue);

			var facet = new ResourceManagerInitializer.TestFacet();
			uiManager.InitializeFacet(facet);

			var adapter = facet.InterfaceRoot.Renderer.Adapter;
			var style = adapter.StyleOf(facet.WindowA);

			Assert.AreEqual(fontname, style.Font.Family);
			Assert.AreEqual(fontSize, style.Font.Size);
			Assert.AreEqual(fontStyles, style.Font.Style);
			Assert.AreEqual(Color.LightBlue, style.Font.Color);
		}

		private void InitializeFontProperties(string fontName, int fontSize, FontStyles? fontStyle = null, Color? textColor = null)
		{
			WindowTheme.Font.Family = fontName;
			WindowTheme.Font.Size = fontSize;
			WindowTheme.Font.Style = fontStyle;
			WindowTheme.TextColor = textColor;
		}

		private void InitializeBorder(string image, LayoutBox borderSlice)
		{
			WindowTheme.Border = new WidgetBorderModel
			{
				Image = image,
				Size = borderSlice
			};
		}

		private WidgetThemeModel WindowTheme
		{
			get
			{
				var windowTheme = data.Themes.First().Value["window"];
				return windowTheme;
			}
		}
	}
}
