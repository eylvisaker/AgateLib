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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Platform.Test;

namespace AgateLib.Platform.IntegrationTest
{
	class IntegrationTestPlatformFactory : IPlatformFactory
	{
		FakePlatformInfo info = new FakePlatformInfo();
		private string appDirPath;

		public IntegrationTestPlatformFactory(string appDirPath)
		{
			this.appDirPath = appDirPath;
			ApplicationFolderFiles = new FileSystemProvider(appDirPath);
		}

		public IReadFileProvider ApplicationFolderFiles { get; private set; }

		public IPlatformInfo Info => info;
		
		public IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}

		public IReadFileProvider OpenAppFolder(string subpath)
		{
			return new FileSystemProvider(Path.Combine(appDirPath, subpath));
		}

		public IReadWriteFileProvider OpenUserAppStorage(string subpath)
		{
			return new FileSystemProvider(Path.Combine(appDirPath, subpath));
		}
	}
}
