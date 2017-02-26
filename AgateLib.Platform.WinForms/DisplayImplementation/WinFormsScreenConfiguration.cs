using System;
using System.Collections.Generic;
using System.Linq;
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

			if (result.IsPrimary)
				PrimaryScreen = result;

			return result;
		}

	}
}