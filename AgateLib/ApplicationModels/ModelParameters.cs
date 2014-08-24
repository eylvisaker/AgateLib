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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.Geometry;
using AgateLib.Geometry.CoordinateSystems;
using AgateLib.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	/// <summary>
	/// Class which contains the parameters which are used to initialize AgateLib
	/// and start up the application model.
	/// </summary>
	public class ModelParameters
	{
		/// <summary>
		/// Constructs a ModelParameters object with default parameters.
		/// </summary>
		public ModelParameters()
		{
			AssetLocations = new AssetLocations();
			CoordinateSystem = new NativeCoordinates();

			AutoCreateDisplayWindow = true;
			CreateFullScreenWindow = true;
			ApplicationName = "AgateLib Application";
			VerticalSync = true;
		}
		/// <summary>
		/// Constructs a ModelParameters object with default parameters. 
		/// Also stores the command line arguments.
		/// </summary>
		/// <param name="args"></param>
		public ModelParameters(string[] args) : this()
		{
			Arguments = args;
		}

		/// <summary>
		/// The name of your application. This is used for the title bar of the
		/// automatically created DisplayWindow.
		/// </summary>
		public string ApplicationName { get; set; }

		/// <summary>
		/// Indicates whether AgateLib should automatically create a display window when starting up.
		/// This defaults to true for every application model except the passive model.
		/// </summary>
		public bool AutoCreateDisplayWindow { get; set; }
		/// <summary>
		/// Indicates what size of display window to set. If this is set to Size.Empty (the default)
		/// then AgateLib will automatically choose a resolution. This will typically be the same
		/// resolution as the display device.
		/// </summary>
		public Size DisplayWindowSize { get; set; }
		/// <summary>
		/// Indicates whether AgateLib should create a full screen display window when 
		/// automatically creating a DisplayWindow. Defaults to true.
		/// </summary>
		public bool CreateFullScreenWindow { get; set; }

		/// <summary>
		/// Indicates whether the frame rate should be restricted to the natural frequency 
		/// of the display device.
		/// </summary>
		public bool VerticalSync { get; set; }

		/// <summary>
		/// The command line arguments which were passed in to the application.
		/// </summary>
		public string[] Arguments { get; set; }

		/// <summary>
		/// Object which indicates the locations of the various assets that are loaded by AgateLib.
		/// </summary>
		public AssetLocations AssetLocations { get; set; }

		/// <summary>
		/// Gets or sets the object which determines how to set the coordinate
		/// system for the automatically created display window. If 
		/// AutoCreateDisplayWindow is false, this property is not used.
		/// </summary>
		public ICoordinateSystemCreator CoordinateSystem { get; set; }

		/// <summary>
		/// Indicates the device type to emulate. This primarily affects the built-in user interface
		/// implementation.
		/// </summary>
		public DeviceType EmulateDeviceType { get; set; }
	}
}
