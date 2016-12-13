using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.Sprites;
using AgateLib.ApplicationModels;
using AgateLib.Resources;

namespace AgateLib.Testing.FoundationTests
{
	class ResourceTester : Scene, ISceneModelTest
	{
		public string Name { get { return "Resources"; } }
		public string Category { get { return "Foundation"; } }
		
		[BindTo("sample_surf")]
		public Surface surf;
		
		[BindTo("sample_sprite")]
		public ISprite sprite;

		[BindTo("sample_font")]
		public Font font;

		protected override void OnSceneStart()
		{
			var resources = new AgateResourceManager("ResourceTester.yaml");
			resources.InitializeContainer(this);
			
			sprite.StartAnimation();
		}

		public override void Update(double deltaT)
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
