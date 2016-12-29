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
namespace AgateLib.Configuration
{
	/// <summary>
	/// Enum which is used to indicate how auto-created DisplayWindow
	/// objects have their size scaled to match the monitor's aspect ratio.
	/// </summary>
	public enum WindowExpansionType
	{
		/// <summary>
		/// Indicates that the vertical dimension will remain fixed, and the 
		/// horizontal dimension will be expanded or contracted to match the 
		/// display aspect ratio.
		/// </summary>
		/// <remarks>
		/// For example, if the desired resolution is set to 640x480 but the 
		/// monitor is a widescreen 1920x1080, the actual logical resolution
		/// of the DisplayWindow will be set to 853x480, so that the logical
		/// resolution has the same aspect ratio as the physical monitor.
		/// </remarks>
		VerticalSizeFixed,

		/// <summary>
		/// Indicates that the horizontal dimension will remain fixed, and the 
		/// vertical dimension will be expanded or contracted to match the 
		/// display aspect ratio.
		/// </summary>
		HorizontalSizeFixed,

		/// <summary>
		/// Indicates that the logical dimensions will remain fixed, and the
		/// display will be scaled without regard to the physical aspect ratio
		/// of the monitor (not recommended).
		/// </summary>
		Scale,
	}
}