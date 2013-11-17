using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExampleUnitTests
{
	[TestClass]
	public class BbxUnitTests
	{
		[TestMethod]
		public void ColorFromHsv()
		{
			for (double h = 0; h < 360; h += 10)
			{
				for (double s = 0; s < 1; s += 0.05)
				{
					for (double v = 0; v < 1; v += 0.05)
					{
						var bbclr = BBUtility.HSVtoRGB((float)h, (float)s, (float)v);

						var agclr = AgateLib.Geometry.Color.FromHsv(h, s, v);

						Assert.AreEqual(agclr.R, bbclr.R, 1.0);
						Assert.AreEqual(agclr.G, bbclr.G, 1.0);
						Assert.AreEqual(agclr.B, bbclr.B, 1.0);
						Assert.AreEqual(agclr.A, 255);
						Assert.AreEqual(bbclr.A, 255);
					}
				}
				
			}
			
		}
	}
}
