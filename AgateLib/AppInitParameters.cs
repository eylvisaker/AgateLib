//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
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

	}
}
