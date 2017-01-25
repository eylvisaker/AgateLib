﻿using System;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Platform.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests
{
	[TestClass]
	public class AgateUnitTest : IDisposable
	{
		private readonly AgateUnitTestPlatform platform;
		private readonly DisplayWindow window;

		public AgateUnitTest()
		{
			platform = AgateUnitTestPlatform.Initialize();

			window = new DisplayWindowBuilder()
				.BackbufferSize(1920, 1080)
				.Build();
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