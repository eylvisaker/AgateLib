using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib
{
	public class AppInitParameters
	{
		public AppInitParameters()
		{
			WindowSize = new Size(800, 600);
			InitializeAudio = true;
			InitializeDisplay = true;
			InitializeJoysticks = true;
		}

		public Size WindowSize { get; set; }
		public bool FullScreen { get; set; }
		public bool ShowSplashScreen { get; set; }
		public bool AllowResize { get; set; }
		public string IconFile { get; set; }

		public bool InitializeDisplay { get; set; }
		public bool InitializeAudio { get; set; }
		public bool InitializeJoysticks { get; set; }
	}
}
