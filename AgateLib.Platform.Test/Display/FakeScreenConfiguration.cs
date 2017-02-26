using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Platform.Test.Display
{
	public class FakeScreenConfiguration : IScreenConfiguration
	{
		public List<ScreenInfo> Screens { get; set; }

		public ScreenInfo PrimaryScreen { get; set; }

		IReadOnlyList<ScreenInfo> IScreenConfiguration.AllScreens => Screens;

		ScreenInfo IScreenConfiguration.PrimaryScreen => PrimaryScreen;

		public Rectangle DesktopBounds => Rectangle.Union(Screens.Select(x => x.Bounds).ToArray());
	}
}