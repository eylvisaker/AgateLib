using AgateLib.Mathematics.CoordinateSystems;
using AgateLib.Mathematics.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.AgateAppTests
{
	[TestClass]
	public class CoordinateSystemTest
	{
		[TestMethod]
		public void FixedAspectRatioNoPreserveTest()
		{
			var fac = new FixedAspectRatioCoordinates();
			fac.PreserveDisplayAspectRatio = false;

			fac.RenderTargetSize = new Size(1280, 720);
			Assert.AreEqual(new Rectangle(0, 0, 1280, 720), fac.Coordinates);

			fac.RenderTargetSize = new Size(1500, 800);
			Assert.AreEqual(new Rectangle(0, 0, 1422, 800), fac.Coordinates);

			fac.Origin = new Point(-21, -52);
			Assert.AreEqual(new Rectangle(-21, -52, 1422, 800), fac.Coordinates);

			fac.MinWidth = 1280;
			fac.MaxWidth = 1920;

			fac.MinHeight = 720;
			fac.MaxHeight = 1080;

			fac.RenderTargetSize = new Size(1000, 500);
			Assert.AreEqual(new Rectangle(-21, -52, 1280, 720), fac.Coordinates);

			fac.RenderTargetSize = new Size(1280, 720);
			Assert.AreEqual(new Rectangle(-21, -52, 1280, 720), fac.Coordinates);

			fac.RenderTargetSize = new Size(1500, 800);
			Assert.AreEqual(new Rectangle(-21, -52, 1422, 800), fac.Coordinates);

			fac.RenderTargetSize = new Size(2000, 2000);
			Assert.AreEqual(new Rectangle(-21, -52, 1920, 1080), fac.Coordinates);

			fac.RenderTargetSize = new Size(1920, 1080);
			Assert.AreEqual(new Rectangle(-21, -52, 1920, 1080), fac.Coordinates);
		}

		[TestMethod]
		public void FixedAspectRatioPreserveTest()
		{
			var fac = new FixedAspectRatioCoordinates();

			fac.RenderTargetSize = new Size(1280, 720);
			Assert.AreEqual(new Rectangle(0, 0, 1280, 720), fac.Coordinates);

			fac.MinWidth = 1280;
			fac.MaxWidth = 1920;

			fac.MinHeight = 720;
			fac.MaxHeight = 1080;

			// the width will be fit to 500 pixels, so 1280 logical points = 500 pixels wide.
			// We want to preserve aspect ratio, so since the window is 500 pixels tall the 
			// total height in logical units will also be 1280. 
			// But the range from 0 to 720 will be centered
			// so the vertical axis will split the difference above and below that.
			fac.RenderTargetSize = new Size(500, 500);
			Assert.AreEqual(Rectangle.FromLTRB(0, -280, 1280, 1000), fac.Coordinates);

			// the height will be fit to 500 pixels, so 720 logical points = 500 pixels high.
			// We want to preserve aspect ratio, so since the window is 1500 pixels wide the 
			// total width in logical units will be 720*3 = 2160. 
			// But the range from 0 to 1280 will be centered
			// so the horizontal axis will split the difference of 880 to the left and right 
			// of that.
			fac.RenderTargetSize = new Size(1500, 500);
			Assert.AreEqual(Rectangle.FromLTRB(-440, 0, 1720, 720), fac.Coordinates);
		}
	}
}
