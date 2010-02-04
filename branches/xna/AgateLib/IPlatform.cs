using System;
namespace AgateLib
{
	public interface IPlatform
	{
		string AppDataDirectory { get; }
		void EnsureAppDataDirectoryExists();
		AgateLib.PlatformType PlatformType { get; }
		AgateLib.DotNetRuntime Runtime { get; }
		void SetFolderPaths(string companyName, string appName);
		AgateLib.WindowsVersion WindowsVersion { get; }
	}
}
