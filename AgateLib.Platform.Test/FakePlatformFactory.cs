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

using System.Linq;
using System.Reflection;
using AgateLib.Drivers;
using AgateLib.IO;

namespace AgateLib.Platform.Test
{
	public class FakePlatformFactory : IPlatformFactory
	{
		public FakePlatformFactory()
		{
			Info = new FakePlatformInfo();
			ApplicationFolderFiles = new FakeReadFileProvider();
			UserAppDataFiles = new FakeReadWriteFileProvider();
		}

		public Platform.IPlatformInfo Info { get; }

		public FakeReadFileProvider ApplicationFolderFiles { get; }

		public FakeReadWriteFileProvider UserAppDataFiles { get; }

		IReadFileProvider IPlatformFactory.ApplicationFolderFiles => ApplicationFolderFiles;

		public Platform.IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}

		public IReadFileProvider OpenAppFolder(string subpath)
		{
			if (string.IsNullOrEmpty(subpath))
				return UserAppDataFiles;

			return SubdirectoryProvider.ReadOnly(ApplicationFolderFiles, subpath);
		}

		public IReadWriteFileProvider OpenUserAppStorage(string subpath)
		{
			if (string.IsNullOrEmpty(subpath))
				return UserAppDataFiles;

			return SubdirectoryProvider.ReadWrite(UserAppDataFiles, subpath);
		}
	}
}
