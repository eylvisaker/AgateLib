using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.WindowsPhone.PlatformImplementation
{
	class WPPlatformInfo : PlatformInfo
	{
		public WPPlatformInfo()
		{
			DeviceType = Platform.DeviceType.Handheld;
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
