using AgateLib.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.UnitTests.Fakes
{
	class FakePlatformInfo : PlatformInfo
	{
		public override string AppDataDirectory
		{
			get { throw new NotImplementedException(); }
		}

		protected internal override void SetFolderPaths(string mCompanyName, string mAppName)
		{
			throw new NotImplementedException();
		}

		protected internal override void EnsureAppDataDirectoryExists()
		{
			throw new NotImplementedException();
		}
	}
}
