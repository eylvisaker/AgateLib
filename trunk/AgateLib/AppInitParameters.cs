﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib
{
	/// <summary>
	/// Class which contains the parameters for initializing the application.
	/// </summary>
	public class AppInitParameters
	{
		/// <summary>
		/// Constructs an AppInitParameters object.
		/// </summary>
		public AppInitParameters()
		{
			InitializeAudio = true;
			InitializeDisplay = true;
			InitializeJoysticks = true;
			ShowSplashScreen = true;
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the splash screen on startup.
		/// </summary>
		public bool ShowSplashScreen { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the main window should be user resizable.
		/// </summary>
		public bool AllowResize { get; set; }
		/// <summary>
		/// Gets or sets a file name which points to an icon file to use for the main window.
		/// </summary>
		public string IconFile { get; set; }

		/// <summary>
		/// Gets or sets a boolean value indicating whether to initialize the display or not.
		/// </summary>
		public bool InitializeDisplay { get; set; }
		/// <summary>
		/// Gets or sets a boolean value indicating whether to initialize the audio system or not.
		/// </summary>
		public bool InitializeAudio { get; set; }
		/// <summary>
		/// Gets or sets a boolean value indicating whether to initialize the joystick system or not.
		/// </summary>
		public bool InitializeJoysticks { get; set; }

		/// <summary>
		/// Gets or sets the preferred display driver.
		/// </summary>
		public Drivers.DisplayTypeID PreferredDisplay { get; set; }
		/// <summary>
		/// Gets or sets the preferred audio driver.
		/// </summary>
		public Drivers.AudioTypeID PreferredAudio { get; set; }
		/// <summary>
		/// Gets or sets the preferred input driver.
		/// </summary>
		public Drivers.InputTypeID PreferredInput { get; set; }

	}
}
