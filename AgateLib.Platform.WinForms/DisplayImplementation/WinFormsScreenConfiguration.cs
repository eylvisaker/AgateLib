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
using System.Windows.Forms;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	public class WinFormsScreenConfiguration : IScreenConfiguration
	{
		private readonly List<ScreenInfo> screens = new List<ScreenInfo>();
		private readonly Rectangle desktopBounds;

		public WinFormsScreenConfiguration()
		{
			var allScreens = System.Windows.Forms.Screen.AllScreens;

			for (int i = 0; i < allScreens.Length; i++)
			{
				screens.Add(CreateScreenInfo(allScreens[i], new IntPtr(i)));
			}

			screens.Sort((a, b) => -a.IsPrimary.CompareTo(b.IsPrimary));

			desktopBounds = Rectangle.Union(screens.Select(s => s.Bounds).ToArray());
		}

		public IReadOnlyList<ScreenInfo> AllScreens => screens;

		public ScreenInfo PrimaryScreen { get; private set; }

		public Rectangle DesktopBounds => desktopBounds;
		
		private ScreenInfo CreateScreenInfo(System.Windows.Forms.Screen wfScreen, IntPtr index)
		{
			ScreenInfo result = new ScreenInfo
			{
				DeviceName = wfScreen.DeviceName,
				Bounds = wfScreen.Bounds.ToGeometry(),
				IsPrimary = wfScreen.Primary,
				SystemIndex = index,
			};

			// It seems like Windows will only report scaling for the primary monitor, so this ends
			// up with the same result on all monitors.
			using (var frm = new Form())
			{
				frm.Location = wfScreen.Bounds.Location;

				using (var graphics = frm.CreateGraphics())
				{
					var dpi = graphics.DpiX;

					result.Scaling = dpi / 96.0;
				}
			}

			if (result.IsPrimary)
				PrimaryScreen = result;

			return result;
		}

	}
}