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
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.IO;

namespace AgateLib.Configuration
{
	public interface IAgateSetup : IDisposable
	{
		/// <summary>
		/// Sets the title of the display windows.
		/// If this is empty, AgateLib will automatically 
		/// set the title from the assembly attributes.
		/// </summary>
		string ApplicationName { get; set; }

		/// <summary>
		/// Sets file paths for AgateLib.IO.Assets.
		/// </summary>
		AssetLocations AssetLocations { get; set; }

		/// <summary>
		/// Value that indicates the size of the smallest dimension of the
		/// auto created display windows. This is usually the vertical 
		/// dimension. If this value is not set, the display windows will 
		/// simply have a coordinate system that matches their size in pixels.
		/// </summary>
		Size DesiredDisplayWindowResolution { get; set; }

		/// <summary>
		/// A value which indicates how a display window should be expanded.
		/// This is used if DesiredDisplayWindowResolution does not match the
		/// physical aspect ratio of the user's monitor.
		/// </summary>
		WindowExpansionType DisplayWindowExpansionType { get; set; }

		/// <summary>
		/// Set to true to ignore desktop scaling when creating a window. 
		/// Use this if you want to provide your own scaling logic.
		/// </summary>
		bool IgnoreDesktopScaling { get; set; }

		/// <summary>
		/// Set to true to force video refreshes to wait for the vertical blank.
		/// Defaults to true. This can be set to false by the user 
		/// via a command line argument of -novsync.
		/// </summary>
		bool VerticalSync { get; set; }

		/// <summary>
		/// Sets the physical size of the created window. This parameter is
		/// not required to be set programmatically. 
		/// </summary>
		/// <remarks>Unless this is set programmatically, this will usually match 
		/// the value of DesiredDisplayWindowResolution with the following exceptions:
		/// <list type="bullet">
		/// <item><description>The user specifies -window &lt;widthxheight&gt;</description></item>
		/// <item><description>The desktop scaling is defined for this machine - this will
		/// result in the physical size of the window being scaled by the same amount.</description></item>
		/// </list>
		/// </remarks>
		Size? DisplayWindowPhysicalSize { get; set; }

		/// <summary>
		/// Set to false to prevent the setup system from automatically creating display
		/// windows. In this case you must manage your own DisplayWindow objects.
		/// </summary>
		bool CreateDisplayWindow { get; set; }

		/// <summary>
		/// Set to indicate whether the window created will be full screen. This
		/// can be turned off on the command line by specifying '-window'
		/// </summary>
		bool CreateFullScreenWindow { get; set; }

		/// <summary>
		/// Set to true to create a display window for each monitor.
		/// </summary>
		bool CreateWindowForEachMonitor { get; set; }
	}
}
