using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.Test
{
	public class AgateUnitTestPlatform
	{
		private int joystickCount = 1;
		private string assetPath;

		public UnitTestPlatform Initialize()
		{
			var result = new UnitTestPlatform(joystickCount: joystickCount);

			if (assetPath != null)
				AgateApp.SetAssetPath(assetPath);

			return result;
		}

		/// <summary>
		/// Sets the number of joysticks that are connected to the test system. Defaults to 1.
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
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
