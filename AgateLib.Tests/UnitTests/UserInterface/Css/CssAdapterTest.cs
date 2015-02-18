using AgateLib.Geometry;
using AgateLib.UserInterface.Css;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Widgets;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Css
{
	[TestClass]
	public class CssAdapterTest : CssTestBase
	{
		[TestMethod]
		public void DocumentConstruction()
		{
			CssDocument doc = CssDocument.FromText("window { color: red; left: 20em; padding: 4px; margin: 8px; } label { color: green; }");
			CssAdapter adapter = new CssAdapter(doc);

			var window = new Window();
			var style = adapter.GetStyle(window);

			Assert.AreEqual(Color.Red, style.Data.Font.Color);
			DistanceAssert(false, 20, DistanceUnit.FontHeight, style.Data.PositionData.Left);
			DistanceAssert(false, 4, DistanceUnit.Pixels, style.Data.Padding.Left);
			DistanceAssert(false, 4, DistanceUnit.Pixels, style.Data.Padding.Right);
			DistanceAssert(false, 4, DistanceUnit.Pixels, style.Data.Padding.Top);
			DistanceAssert(false, 4, DistanceUnit.Pixels, style.Data.Padding.Bottom);

			DistanceAssert(false, 8, DistanceUnit.Pixels, style.Data.Margin.Left);
			DistanceAssert(false, 8, DistanceUnit.Pixels, style.Data.Margin.Right);
			DistanceAssert(false, 8, DistanceUnit.Pixels, style.Data.Margin.Top);
			DistanceAssert(false, 8, DistanceUnit.Pixels, style.Data.Margin.Bottom);

		}

		[TestMethod]
		public void MarginPaddingTest()
		{
			CssDocument doc = CssDocument.FromText(
				@"window { color: red; margin-top: 4px; } 
				  panel { margin-bottom: 5px; margin-right: 3em; padding-left: 18%; }");
			CssAdapter adapter = new CssAdapter(doc);
			Window wind = new Window();
			Panel pnl = new Panel();

			var style = adapter.GetStyle(wind);
			Assert.AreEqual(new CssDistance { Amount = 4, DistanceUnit = DistanceUnit.Pixels }, style.Data.Margin.Top);

			style = adapter.GetStyle(pnl);
			Assert.AreEqual(new CssDistance { Amount = 3, DistanceUnit = DistanceUnit.FontHeight }, style.Data.Margin.Right);
			Assert.AreEqual(new CssDistance { Amount = 18, DistanceUnit = DistanceUnit.Percent }, style.Data.Padding.Left);
		}


		[TestMethod]
		public void BorderImageSetting()
		{
			CssDocument doc = CssDocument.FromText("window { border-image-repeat: stretch; border-image-width: 4px 8px; border-image-outset: 34px; }");
			CssAdapter adapter = new CssAdapter(doc);
			Window wind = new Window();

			var style = adapter.GetStyle(wind);

			DistanceAssert(false, 8, DistanceUnit.Pixels, style.Data.Border.Image.Width.Right);
			DistanceAssert(false, 4, DistanceUnit.Pixels, style.Data.Border.Image.Width.Bottom);
			DistanceAssert(false, 34, DistanceUnit.Pixels, style.Data.Border.Image.Outset.Left);

			Assert.AreEqual(CssBorderImageRepeat.Stretch, style.Data.Border.Image.Repeat);
		}

	}
}
