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
		public PlatformFactory(string appRootPath)
		{
			Info = new FormsPlatformInfo();

			ApplicationFolderFileProvider = new FileSystemProvider(appRootPath);
		}

		public PlatformInfo Info { get; private set; }
		public IReadFileProvider ApplicationFolderFileProvider { get; private set; }

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

		static public string AssemblyLoadDirectory
		{
			get
			{
				string codeBase = Assembly.GetCallingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		public IEnumerable<Assembly> GetSerializationSearchAssemblies(Type objectType)
		{
			var assembly = Assembly.GetEntryAssembly();

			yield return assembly;

			// add names of assemblies referenced by the current assembly.
			Assembly[] loaded = AppDomain.CurrentDomain.GetAssemblies();
			AssemblyName[] referenced = assembly.GetReferencedAssemblies();

			foreach (AssemblyName assname in referenced)
			{
				Assembly ass = loaded.FirstOrDefault(x => x.FullName == assname.FullName);

				if (ass != null)
					yield return ass;
			}
		}


		public IPlatformSerialization CreateDefaultSerializationConstructor()
		{
			return new PlatformSerialization();
		}
	}
}
