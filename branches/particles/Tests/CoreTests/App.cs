using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;

namespace Tests.AppTester
{
	class App : AgateApplication, IAgateTest
	{
		public void Main(string[] args)
		{
			Run(args);
		}

		protected override AppInitParameters GetAppInitParameters()
		{
			var retval = base.GetAppInitParameters();
			retval.AllowResize = true;
			return retval;
		}

		#region IAgateTest Members

		public string Name { get { return "App Tester"; } }
		public string Category { get { return "Core"; } }

		#endregion
	}
}
