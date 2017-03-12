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
using AgateLib.Quality;

namespace AgateLib.IO
{
	/// <summary>
	/// Extension methods which provoide subdirectory providers for IReadFileProvider and IReadWriteFileProvider objects.
	/// </summary>
	public static class SubdirectoryProvider
	{
		/// <summary>
		/// Returns an IReadWriteFileProvider object which works from a subdirectory of this IReadWriteFileProvider object.
		/// </summary>
		/// <param name="fileProvider"></param>
		/// <param name="subpath"></param>
		/// <returns></returns>
		public static IReadWriteFileProvider Subdirectory(this IReadWriteFileProvider fileProvider, string subpath)
		{
			return new SubdirectoryProviderReadWrite(fileProvider, subpath);
		}

		/// <summary>
		/// Returns an IReadFileProvider object which works from a subdirectory of this IReadFileProvider object.
		/// </summary>
		/// <param name="fileProvider"></param>
		/// <param name="subpath"></param>
		/// <returns></returns>
		public static IReadFileProvider Subdirectory(this IReadFileProvider fileProvider, string subpath)
		{
			return new SubdirectoryProviderReadOnly(fileProvider, subpath);
		}
	}
}
