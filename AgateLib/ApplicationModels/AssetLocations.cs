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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	/// <summary>
	/// Structure which indicates where various assets are stored.
	/// </summary>
	public class AssetLocations
	{
		public AssetLocations()
		{
		}

		/// <summary>
		/// Path prefix to load surfaces from.
		/// </summary>
		public string Surfaces { get; set; }
		/// <summary>
		/// Path prefix to load sound effects from.
		/// </summary>
		public string Sound { get; set; }
		/// <summary>
		/// Path prefix to load music files from.
		/// </summary>
		public string Music { get; set; }
		/// <summary>
		/// Path prefix to load resource files from.
		/// </summary>
		public string Resources { get; set; }
		/// <summary>
		/// Path prefix to load user interface files from.
		/// </summary>
		public string UserInterface { get; set; }
	}
}
