using System;
using System.Collections.Generic;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform.WinForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.AgateAppTests
{
	[TestClass]
	public class ArgumentProcessorTest : AgateUnitTest
	{
		[TestMethod]
		public void SetWindowSizeTest()
		{
			var setup = new DisplayWindowBuilder(Args("-window 640x480"));
			setup.Build();

			Assert.IsFalse(setup.CreateWindowParams.IsFullScreen);
			Assert.AreEqual(new Size(640, 480), setup.CreateWindowParams.PhysicalSize);
		}

		[TestMethod]
		public void ExtraArguments()
		{
			var setup = new DisplayWindowBuilder(Args("-window 640x480 -something -else 14 -nothing"));
			setup.Build();

			Assert.AreEqual(new Size(640, 480), setup.CreateWindowParams.PhysicalSize);
		}

		private string[] Args(string v)
		{
			return v.Split(' ');
		}
	}
}
