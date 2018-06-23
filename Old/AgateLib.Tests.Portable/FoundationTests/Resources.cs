using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Sprites;
using AgateLib.Platform;
using AgateLib.Resources;

namespace AgateLib.Tests.FoundationTests
{
	class ResourceTester : Scene, IAgateTest
	{
		public string Name => "Resources";

		public string Category => "Foundation";

		[BindTo("sample_surf")] public Surface surf { get; set; }

		[BindTo("sample_sprite")] public ISprite sprite { get; set; }

		[BindTo("sample_font")] public Font font { get; set; }

		protected override void OnSceneStart()
		{
			var resources = new AgateResourceManager("ResourceTester.yaml");
			resources.InitializeContainer(this);

			sprite.StartAnimation();
		}

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
			sprite.Update();
		}

		protected override void OnRedraw()
		{
			Display.Clear(Color.Red);

			font.DrawText(0, 0, "FPS: " + Display.FramesPerSecond.ToString());

			surf.Draw(20, 20);

			sprite.Draw(100, 150);
		}

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				new SceneStack().Start(this);
			}
		}
	}
}
