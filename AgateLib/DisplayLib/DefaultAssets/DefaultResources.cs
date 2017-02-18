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
using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.DisplayLib.DefaultAssets
{
	/// <summary>
	/// Represents resources that are available on all platforms AgateLib supports.
	/// </summary>
	public class DefaultResources : IDisposable
	{
		internal DefaultResources()
		{ }

		/// <summary>
		/// Sans serif font.
		/// </summary>
		public Font AgateSans { get; set; }

		/// <summary>
		/// Serif font.
		/// </summary>
		public Font AgateSerif { get; set; }

		/// <summary>
		/// Monospace font.
		/// </summary>
		public Font AgateMono { get; set; }

		/// <summary>
		/// Disposes of the resources.
		/// </summary>
		public void Dispose()
		{
			AgateSans?.Dispose();
			AgateSerif?.Dispose();
			AgateMono?.Dispose();

			AgateSans = null;
			AgateSerif = null;
			AgateMono = null;
		}
	}
}
