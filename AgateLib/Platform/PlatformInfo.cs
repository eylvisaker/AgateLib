using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform
{
	public abstract class PlatformInfo
	{
		public abstract string AppDataDirectory { get; }

		protected internal abstract void SetFolderPaths(string mCompanyName, string mAppName);
		protected internal abstract void EnsureAppDataDirectoryExists();

		public abstract PlatformType PlatformType { get; }

		public abstract DotNetRuntime Runtime { get; }
	}
}
