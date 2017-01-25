using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.IO;
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
			using (var platform = AgateUnitTestPlatform.Initialize())
			{
				platform.AppFolderFileProvider.Add("test", "");

				// Verify that surface constructor does not throw an exception
				new Surface("test");
			}
		}

		[TestMethod]
		public void SetAssetLocationUnitTestPlatform()
		{
			using (var platform = AgateUnitTestPlatform.Initialize())
			{
				AgateApp.SetAssetPath("Assets");

				platform.AppFolderFileProvider.Add(
					"Assets/Images/Sprites.yaml",
					"Sprite.yaml contents go here");

				Assets.OpenRead("Images/Sprites.yaml");
				AgateApp.Assets.OpenRead("Images/Sprites.yaml");
			}
		}
	}
}