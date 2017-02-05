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
using AgateLib.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Diagnostics;
using AgateLib.IO;

namespace AgateLib.Drivers
{
	public interface IPlatformFactory
	{
		IPlatformInfo Info { get; }

		IStopwatch CreateStopwatch();

		/// <summary>
		/// Gets a file provider which points to the application directory.
		/// </summary>
		IReadFileProvider ApplicationFolderFiles { get; }

		/// <summary>
		/// Creates a file provider which points to a subfolder of the application
		/// directory.
		/// </summary>
		/// <param name="subpath"></param>
		/// <returns></returns>
		IReadFileProvider OpenAppFolder(string subpath);

		/// <summary>
		/// Creates a file provider which points to a subfolder of the user's
		/// app storage folder.
		/// </summary>
		/// <param name="subpath"></param>
		/// <returns></returns>
		IReadWriteFileProvider OpenUserAppStorage(string subpath);
	}
}
