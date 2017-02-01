using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.Test
{
	public class AgateUnitTestPlatform
	{
		private int joystickCount;
		private string assetPath;

		public UnitTestPlatform Initialize()
		{
			var result = new UnitTestPlatform(joystickCount: joystickCount);

			if (assetPath != null)
				AgateApp.SetAssetPath(assetPath);

			return result;
		}

		public AgateUnitTestPlatform JoystickCount(int count)
		{
			joystickCount = count;

			return this;
		}

		public AgateUnitTestPlatform AssetPath(string assets)
		{
			assetPath = assets;

			return this;
		}
	}
}
