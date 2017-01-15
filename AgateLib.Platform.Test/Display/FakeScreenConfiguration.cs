using System;
using System.Collections.Generic;
using AgateLib.DisplayLib;

namespace AgateLib.Platform.Test.Display
{
	public class FakeScreenConfiguration : IScreenConfiguration
	{
		public List<ScreenInfo> Screens { get; set; }

		public ScreenInfo PrimaryScreen { get; set; }

		IReadOnlyList<ScreenInfo> IScreenConfiguration.Screens => Screens;

		ScreenInfo IScreenConfiguration.PrimaryScreen => PrimaryScreen;
	}
}