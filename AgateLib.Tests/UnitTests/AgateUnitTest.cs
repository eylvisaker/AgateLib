using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgateLib.Platform.Test;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.UnitTests
{
	[TestClass]
	public class AgateUnitTest : IDisposable
	{
		UnitTestPlatform platform;
		DisplayWindow window;

		public AgateUnitTest()
		{
			platform = new UnitTestPlatform();
			platform.DesiredDisplayWindowResolution = new Size(1920, 1080);
			platform.InitializeAgateLib();

			window = DisplayWindow.CreateWindowed("AgateLib", new Size(1920, 1080));
		}

		public void Dispose()
		{
			Dispose(true);

			platform.Dispose();
			Core.Dispose();
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
