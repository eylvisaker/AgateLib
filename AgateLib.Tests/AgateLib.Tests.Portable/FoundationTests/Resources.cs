using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.Resources.Legacy;
using AgateLib.Sprites;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.FoundationTests
{
	class ResourceTester : Scene, ISceneModelTest
	{
		public string Name { get { return "Resources"; } }
		public string Category { get { return "Foundation"; } }
		
		Surface surf;
		ISprite sprite;
		FontSurface font;

		protected override void OnSceneStart()
		{
			AgateResourceCollection resources = new AgateResourceCollection("TestResourceFile.xml");

			surf = new Surface(resources, "sample_surf");
			sprite = new Sprite(resources, "sample_sprite");
			font = new FontSurface(resources, "sample_font");

			sprite.StartAnimation();
		}

		public override void Update(double delta_t)
		{
			sprite.Update();
		}

		public override void Draw()
		{
			Display.Clear(Color.Red);

			font.DrawText(0, 0, "FPS: " + Display.FramesPerSecond.ToString());

			surf.Draw(20, 20);

			sprite.Draw(100, 150);
		}

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}

		public Scene StartScene
		{
			get { return this; }
		}
	}
}
