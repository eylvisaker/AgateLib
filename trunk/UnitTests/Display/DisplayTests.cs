using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgateLib;
using AgateLib.Drivers;
using AgateLib.DisplayLib;

namespace AgateLib.UnitTests.DisplayTest
{
	[TestClass]
	public class DisplayTest
	{
		[TestMethod]
		public void InitializeDisplay()
		{
			using (AgateSetup setup = new AgateSetup())
			{
				setup.PreferredDisplay =  (DisplayTypeID) 1000;
				setup.InitializeDisplay((DisplayTypeID)1000);

				Assert.IsFalse(setup.WasCanceled);

				DisplayWindow wind = DisplayWindow.CreateWindowed("Title", 400, 400);
				
			}
		}
	}
}
