using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.Resources;
using AgateLib.Sprites;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.Platform.WindowsForms;

namespace Tests.ResourceTester
{
	class ResourceTester : IAgateTest
	{
		public void Main(string[] args)
		{
			PassiveModel.Run(args, () =>
			{
				Configuration.Resources.AddPath("Data");

				AgateResourceCollection resources = new AgateResourceCollection("TestResourceFile.xml");

				DisplayWindow wind = new DisplayWindow(resources, "main_window");
				Surface surf = new Surface(resources, "sample_surf");
				ISprite sprite = new Sprite(resources, "sample_sprite");
				FontSurface font = new FontSurface(resources, "sample_font");

				sprite.StartAnimation();

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.Red);

					font.DrawText(0, 0, "FPS: " + Display.FramesPerSecond.ToString());

					surf.Draw(20, 20);

					sprite.Update();
					sprite.Draw(100, 100);

					Display.EndFrame();
					Core.KeepAlive();
				}
			});
		}


		#region IAgateTest Members

		public string Name { get { return "Resources"; } }
		public string Category { get { return "Core"; } }

		#endregion
	}
}
