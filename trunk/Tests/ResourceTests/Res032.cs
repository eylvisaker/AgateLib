using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Resources;

namespace Tests.ResourceTests
{
	class Resources032: AgateGame, IAgateTest
	{
		public void Main(string[] args)
		{
			Run(args);
		}

		protected override void  AdjustAppInitParameters(ref AppInitParameters initParams)
		{
			initParams.AllowResize = true;
		}

		public string Name { get { return "Resources 0.3.2"; } }
		public string Category { get { return "Resources"; } }

		AgateResourceCollection resources;
		Surface btn;

		protected override void Initialize()
		{
			resources = AgateResourceCollection.FromZipArchive("Data/gui.zip");

			btn = new Surface(resources, "Button");
		}

		protected override void Update(double time_ms)
		{
			base.Update(time_ms);

			if (Keyboard.Keys[KeyCode.Space])
				Quit();
		}

		protected override void Render()
		{
			btn.Draw(5, 5);
		}
	}
}
