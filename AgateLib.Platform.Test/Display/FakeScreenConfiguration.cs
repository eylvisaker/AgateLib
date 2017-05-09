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
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeScreenConfiguration : IScreenConfiguration
	{
		public FakeScreenConfiguration()
		{
			Screens.Add(new ScreenInfo
			{
				Bounds = new Rectangle(0, 0, 1920, 1080),
				DeviceName = "Fake Screen",
				IsPrimary = true,
				Scaling = 1,
			});

			PrimaryScreen = Screens.First();
		}

		public List<ScreenInfo> Screens { get; set; } = new List<ScreenInfo>();

		public ScreenInfo PrimaryScreen { get; set; }

		IReadOnlyList<ScreenInfo> IScreenConfiguration.AllScreens => Screens;

		ScreenInfo IScreenConfiguration.PrimaryScreen => PrimaryScreen;

		public Rectangle DesktopBounds => Rectangle.Union(Screens.Select(x => x.Bounds).ToArray());
	}
}