using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Platform.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.AgateAppTests
{
	[TestClass]
	public class AssetLocationTests
	{
		[TestMethod]
		public void NoAssetLocationSet()
		{
			using (var platform = UnitTestPlatform.Initialize())
			{
				platform.AppFolderFileProvider.Add("test", "");

				// Verify that surface constructor does not throw an exception
				new Surface("test");
			}
		}
	}
}
