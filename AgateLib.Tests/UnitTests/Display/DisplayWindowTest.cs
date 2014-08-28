using AgateLib.ApplicationModels;
using AgateLib.Geometry;
using AgateLib.UnitTests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.Display
{
	[TestClass]
	public class DisplayWindowTest
	{
		FakeDisplayWindow wind;

		class FixedCoords : ICoordinateSystemCreator
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
			Assert.AreEqual(new Point(50, 50), wind.PixelToLogicalCoords(Point.Empty));
			Assert.AreEqual(new Point(1050, 1050), wind.PixelToLogicalCoords(new Point(500, 500)));
		}
	}
}
