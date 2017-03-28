//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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

			return ApplicationFolderFiles.Subdirectory(subpath);
		}

		public IReadWriteFileProvider OpenUserAppStorage(string subpath)
		{
			if (string.IsNullOrEmpty(subpath))
				return UserAppDataFiles;

			return UserAppDataFiles.Subdirectory(subpath);
		}
	}
}
