using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.Test.Display;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.DisplayLib
{
	[TestClass]
	public class DisplayWindowTest
	{
		FakeDisplayWindow wind;

		class FixedCoords : ICoordinateSystem
		{
			public Rectangle Coordinates { get; set; }
			public FixedCoords(Rectangle rect)
			{
				Coordinates = rect;
			}

			public Rectangle DetermineCoordinateSystem(Size displayWindowSize)
			{
				return Coordinates;
			}

			public Size RenderTargetSize { get; set; }
		}

		[TestInitialize]
		public void Initialize()
		{
			wind = new FakeDisplayWindow(new Size(500, 500));

			wind.FrameBuffer.CoordinateSystem = new FixedCoords(new Rectangle(50, 50, 1000, 1000));
		}

		[TestMethod]
		public void XCoordinateSystemRoundTrip()
		{
			for (int j = -50; j < 1000; j += 250)
			{
				for (int i = -50; i < 1000; i += 250)
				{
					Point p = new Point(i, j);

					Assert.AreEqual(p, wind.PixelToLogicalCoords(wind.LogicalToPixelCoords(p)));
				}
			}
		}

		[TestMethod]
		public void XCoordinateSystem()
		{
			Assert.AreEqual(new Point(50, 50), wind.PixelToLogicalCoords(Point.Zero));
			Assert.AreEqual(new Point(1050, 1050), wind.PixelToLogicalCoords(new Point(500, 500)));
		}
	}
}
