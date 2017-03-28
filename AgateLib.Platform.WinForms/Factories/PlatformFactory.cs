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

using AgateLib.Diagnostics;
using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Platform.WinForms.PlatformImplementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AgateLib.Platform.WinForms.Factories
{
	class PlatformFactory : IPlatformFactory
	{
		private readonly Assembly entryAssembly;
		private readonly string applicationDirectory;

		private string userAppDataPath;
		private string appCompanyName;
		private string appProductName;
		private string documentsPath;

		public PlatformFactory()
		{
			entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

			var companyAttribute = GetCustomAttribute<AssemblyCompanyAttribute>(entryAssembly);
			var nameAttribute = GetCustomAttribute<AssemblyProductAttribute>(entryAssembly);

			appCompanyName = companyAttribute?.Company ?? string.Empty;
			appProductName = nameAttribute?.Product ?? string.Empty;

			userAppDataPath = GetUserAppDataDirectory();
			SetUserDataDirectory();

			Info = new FormsPlatformInfo();

			applicationDirectory = GetApplicationDirectory();
			documentsPath = GetUserDocumentsDirectory();

			ApplicationFolderFiles = OpenAppFolder("");
		}

		public string AppCompanyName
		{
			get { return appCompanyName; }
			set
			{
				appCompanyName = value;
				SetUserDataDirectory();
			}
		}

		public string AppProductName
		{
			get { return appProductName; }
			set { appProductName = value; SetUserDataDirectory(); }
		}


		public IPlatformInfo Info { get; private set; }

		public IReadFileProvider ApplicationFolderFiles { get; private set; }

		public IReadWriteFileProvider UserAppStorage { get; private set; }

		public IReadFileProvider OpenAppFolder(string subpath)
		{
			return new FileSystemProvider(Path.Combine(applicationDirectory, subpath));
		}

		public IReadWriteFileProvider OpenUserAppStorage(string subpath)
		{
			return new FileSystemProvider(Path.Combine(userAppDataPath, subpath));
		}

		public IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}

		private string GetApplicationDirectory()
		{
			string fqn = entryAssembly.GetLoadedModules()[0].FullyQualifiedName;

			var dir = Path.GetDirectoryName(fqn);
			Log.WriteLine($"App Dir: {dir}");

			return dir;
		}

		private string GetUserAppDataDirectory()
		{
			string combDir;
			string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			// If only one is non null, use that one. Otherwise,
			// combine them.
			if (AppCompanyName == null) combDir = AppProductName;
			else if (AppProductName == null) combDir = AppCompanyName;
			else 
				combDir = Path.Combine(AppCompanyName, AppProductName);

			if (combDir == null)
				return rootPath;

			return Path.Combine(rootPath, combDir);
		}

		private string GetUserDocumentsDirectory()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		}

		private static T GetCustomAttribute<T>(Assembly ass) where T : Attribute
		{
			try
			{
				return ass.GetCustomAttributes(typeof(T), false)[0] as T;
			}
			catch
			{
				return null;
			}
		}

		private void SetUserDataDirectory()
		{
			UserAppStorage = OpenUserAppStorage("");
		}
	}
}
