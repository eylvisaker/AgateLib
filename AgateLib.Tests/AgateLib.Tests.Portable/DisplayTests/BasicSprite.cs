using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.ApplicationModels;
using AgateLib.Configuration;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Sprites;
using AgateLib.Geometry;

namespace AgateLib.Tests.DisplayTests
{
	class BasicSprite : Scene, IAgateTest
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
		
		public AgateConfig Configuration { get; set; }

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

		public void ModifySetup(IAgateSetup setup)
		{
			setup.DesiredDisplayWindowResolution = new Size(800, 600);
		}

		public void Run()
		{
			SceneStack.Start(this);
		}
	}
}
