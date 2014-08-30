using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsStore.PlatformImplementation
{
	public class WindowsStorePlatformInfo : PlatformInfo
	{
		public WindowsStorePlatformInfo(bool handheld)
		{
			if (handheld)
				DeviceType = Platform.DeviceType.Handheld;
			else
				DeviceType = Platform.DeviceType.Tablet;

			this.PlatformType = Platform.PlatformType.Windows;
			this.Runtime = DotNetRuntime.MicrosoftDotNet;
		}
		public override string AppDataDirectory
		{
			get { return string.Empty; }
		}

		protected override void SetFolderPaths(string mCompanyName, string mAppName)
		{
		}

		protected override void EnsureAppDataDirectoryExists()
		{
		}
	}
}
