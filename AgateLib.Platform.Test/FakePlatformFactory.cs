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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Platform.Common.PlatformImplementation;

namespace AgateLib.Platform.Test
{
	public class FakePlatformFactory : IPlatformFactory
	{
		private Dictionary<string, IReadFileProvider> appFolders = new Dictionary<string, IReadFileProvider>();
		private Dictionary<string, FakeReadWriteFileProvider> userFolders = new Dictionary<string, FakeReadWriteFileProvider>();

		public FakePlatformFactory()
		{
			Info = new FakePlatformInfo();
			ApplicationFolderFiles = new FakeReadFileProvider();
		}

		public Platform.IPlatformInfo Info { get; }

		public FakeReadFileProvider ApplicationFolderFiles { get; }

		IReadFileProvider IPlatformFactory.ApplicationFolderFiles => ApplicationFolderFiles;

		public Platform.IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}

		public virtual void Initialize(IO.FileSystemObjects fileSystemObjects)
		{
		}

		public IReadFileProvider OpenAppFolder(string subpath)
		{
			if (!appFolders.ContainsKey(subpath))
			{
				appFolders[subpath] = new SubdirectoryProvider(ApplicationFolderFiles, subpath);
			}

			return appFolders[subpath];
		}

		public IReadWriteFileProvider OpenUserAppStorage(string subpath)
		{
			if (!userFolders.ContainsKey(subpath))
			{
				userFolders[subpath] = new FakeReadWriteFileProvider();
			}

			return userFolders[subpath];
		}
	}

	public class FakeReadWriteFileProvider : IReadWriteFileProvider
	{
		public bool IsLogicalFilesystem
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void CreateDirectory(string folder)
		{
			throw new NotImplementedException();
		}

		public bool FileExists(string filename)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetAllFiles()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			throw new NotImplementedException();
		}

		public bool IsRealFile(string filename)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenReadAsync(string filename)
		{
			throw new NotImplementedException();
		}

		public Task<Stream> OpenWriteAsync(string file)
		{
			throw new NotImplementedException();
		}

		public string ReadAllText(string filename)
		{
			throw new NotImplementedException();
		}

		public string ResolveFile(string filename)
		{
			throw new NotImplementedException();
		}
	}
}
