using System;
using System.Collections.Generic;
using AgateLib.Geometry;
using AgateLib.Platform.WinForms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.ApplicationModels
{
	[TestClass]
	public class ArgumentProcessorTest : AgateUnitTest
	{
		[TestMethod]
		public void SetWindowSizeTest()
		{
			var setup = new AgateSetup(Args("-window 640x480"));

			Assert.IsFalse(setup.FullScreen);
			Assert.AreEqual(new Size(640, 480), setup.DisplayWindowPhysicalSize);
		}

		[TestMethod]
		public void ExtraArguments()
		{
			var setup = new AgateSetup(Args("-window 640x480 -something -else 14 -nothing"));

			Assert.AreEqual(new Size(640, 480), setup.DisplayWindowPhysicalSize);
		}

		private string[] Args(string v)
		{
			return v.Split(' ');
		}
	}
}
