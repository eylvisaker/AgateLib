using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.CoordinateSystems;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.DisplayLib
{
	[TestClass]
	public class DisplayWindowBuilderTests 
	{
		[TestMethod]
		public void ApplyCoordinates()
		{
			using (new AgateUnitTestPlatform().Initialize())
			{
				var coordRect = new Rectangle(-10, -25, 200, 300);

				var displayWindow = new DisplayWindowBuilder()
					.BackbufferSize(1000, 1000)
					.WithCoordinates(new FixedCoordinateSystem(coordRect))
					.Build();

				displayWindow.FrameBuffer.CoordinateSystem.RenderTargetSize = new Size(1000, 1000);
				Assert.AreEqual(coordRect, displayWindow.FrameBuffer.CoordinateSystem.Coordinates);
			}
		}
	}
}
