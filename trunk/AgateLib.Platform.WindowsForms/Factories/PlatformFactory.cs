﻿using AgateLib.Diagnostics;
using AgateLib.Drivers;
using AgateLib.IO;
using AgateLib.Platform.WindowsForms.PlatformImplementation;
using AgateLib.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.Factories
{
	class PlatformFactory : IPlatformFactory
	{
		public PlatformInfo CreatePlatformInfo()
		{
			return new FormsPlatformInfo();
		}

		public IStopwatch CreateStopwatch()
		{
			return new DiagnosticsStopwatch();
		}


		public IFile CreateFile()
		{
			return new SysIoFile();
		}


		public Diagnostics.AgateConsole CreateConsole()
		{
			return new AgateConsoleImpl();
		}


		public IPath CreatePath()
		{
			return new SysIoPath();
		}

		public IReadFileProvider CreateAssetFileProvider()
		{
			//return new FileSystemProvider(AssemblyLoadDirectory);
			return new FileSystemProvider(".");
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


		public IObjectConstructor CreateDefaultSerializationConstructor()
		{
			return new ActivatorConstructor();
		}
	}
}