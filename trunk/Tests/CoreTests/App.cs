using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

namespace Tests.AppTester
{
	class App : AgateGame, IAgateTest
	{
		public void Main(string[] args)
		{
			Run(args);
		}

		protected override void  AdjustAppInitParameters(ref AppInitParameters initParams)
		{
			initParams.AllowResize = true;
		}

		
		#region IAgateTest Members

		public string Name { get { return "App Tester"; } }
		public string Category { get { return "Core"; } }

		#endregion

		protected override void Update(double time_ms)
		{
			base.Update(time_ms);

			if (Keyboard.Keys[KeyCode.Space])
				Quit();
		}
	}
}
