using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib.Sprites;
using AgateLib.ApplicationModels;

namespace AgateLib.Tests.DisplayTests
{
	class BasicSprite : Scene, ISceneModelTest
	{
		Sprite p;

		public string Name
		{
			get { return "Basic Sprite"; }
		}

		public string Category
		{
			get { return "Display"; }
		}
		
		protected override void OnSceneStart()
		{
			p = new Sprite("boxsprite.png", new Size(96, 96));
			p.AnimationType = SpriteAnimType.PingPong;
			p.TimePerFrame = 250;
			p.StartAnimation();
		}
		protected override void OnSceneEnd()
		{
			p.Dispose();
		}

		public override void Update(double deltaT)
		{
			p.Update(deltaT);
		}
	
		public override void Draw()
		{
			Display.Clear(Color.Blue);

			p.Draw(0, 0);
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
