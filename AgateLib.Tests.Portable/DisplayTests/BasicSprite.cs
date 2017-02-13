﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Sprites;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.Tests.DisplayTests
{
	class BasicSprite : Scene, IAgateTest
	{
		Sprite p;

		public string Name => "Basic Sprite";

		public string Category => "Display";

		protected override void OnSceneStart()
		{
			p = new Sprite("Images/boxsprite.png", new Size(96, 96))
			{
				AnimationType = SpriteAnimType.PingPong,
				TimePerFrame = 250
			};

			p.StartAnimation();
		}

		protected override void OnSceneEnd()
		{
			p.Dispose();
		}

		public override void Update(TimeSpan elapsed)
		{
			p.Update(elapsed);
		}
	
		public override void Draw()
		{
			Display.Clear(Color.Blue);

			p.Draw(0, 0);
		}

		public void Run(string[] args)
		{
			using (var window = new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				SceneStack.Start(this);
			}
		}
	}
}
