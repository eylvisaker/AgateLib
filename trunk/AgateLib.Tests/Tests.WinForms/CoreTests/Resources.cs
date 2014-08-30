using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.Resources.Legacy;
using AgateLib.Sprites;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Platform.WinForms;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.CoreTests
{
	class ResourceTester : IDiscreteAgateTest
	{
		public void Main(string[] args)
		{
			new PassiveModel(new PassiveModelParameters(args)).Run( () =>
			{
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

		public string Name { get { return "Resources"; } }
		public string Category { get { return "Core"; } }
	}
}
