using System;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Platform.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests
{
	[TestClass]
	public class AgateUnitTest : IDisposable
	{
		private readonly UnitTestPlatform platform;
		private readonly DisplayWindow window;

		public AgateUnitTest()
		{
			platform = new UnitTestPlatform {DesiredDisplayWindowResolution = new Size(1920, 1080)};
			platform.InitializeAgateLib();

			window = DisplayWindow.CreateWindowed("AgateLib", new Size(1920, 1080));
		}

		public void Dispose()
		{
			Dispose(true);

			window.Dispose();
			platform.Dispose();
			AgateApp.Dispose();
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
}