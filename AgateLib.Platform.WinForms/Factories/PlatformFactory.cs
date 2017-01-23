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
using AgateLib.Diagnostics;
using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Platform.Common.PlatformImplementation;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Platform.WinForms.PlatformImplementation;
using AgateLib.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms.Factories
{
	class PlatformFactory : IPlatformFactory
	{
		private Assembly entryAssembly;
		private string applicationDirectory;
		private string userAppDataPath;
		private string documentsPath;

		public PlatformFactory(string appRootPath = null)
		{
			entryAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

			var companyAttribute = GetCustomAttribute<AssemblyCompanyAttribute>(entryAssembly);
			var nameAttribute = GetCustomAttribute<AssemblyProductAttribute>(entryAssembly);

			SetPaths(
				companyAttribute != null ? companyAttribute.Company : string.Empty,
				nameAttribute != null ? nameAttribute.Product : string.Empty);

			Info = new FormsPlatformInfo();

			applicationDirectory = appRootPath ?? GetApplicationDirectory();
			documentsPath = GetUserDocumentsDirectory();

			ApplicationFolderFiles = OpenAppFolder("");

			UserAppStorage = OpenUserAppStorage("");
		}

		public string AppCompanyName { get; private set; }

		public string AppProductName { get; private set; }

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

		public void SetPaths(string applicationName, string companyName)
		{
			userAppDataPath = GetUserAppDataDirectory();

			AppCompanyName = companyName;
			AppProductName = applicationName;

			UserAppStorage = OpenUserAppStorage("");
		}

		public IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}

		public void Initialize(FileSystemObjects fileSystemObjects)
		{
			fileSystemObjects.File = new SysIoFile();
			fileSystemObjects.Path = new SysIoPath();
			fileSystemObjects.Directory = new SysIoDirectory();
		}

		private string GetApplicationDirectory()
		{
			string fqn = entryAssembly.GetLoadedModules()[0].FullyQualifiedName;

			var dir = Path.GetDirectoryName(fqn);
			Log.WriteLine("App Dir: {0}", dir);

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
	}
}
