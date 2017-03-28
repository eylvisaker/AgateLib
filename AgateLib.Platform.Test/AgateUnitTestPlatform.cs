//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

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
