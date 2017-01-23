// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Sprites;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Tests.DisplayTests.RotatingSpriteTester
{
	class RotatingSprite : Scene, IAgateTest
	{
		Point location = new Point(150, 100);
		Sprite sp;

		public string Name
		{
			get { return "Rotating Sprite"; }
		}

		public string Category
		{
			get { return "Display"; }
		}

		public Scene StartScene
		{
			get { return this; }
		}

		protected override void OnSceneStart()
		{
			sp = new Sprite("Images/spike.png", 16, 16);

			sp.RotationCenter = OriginAlignment.Center;
			sp.DisplayAlignment = OriginAlignment.Center;

			sp.RotationAngleDegrees = 90;
			sp.SetScale(2, 2);
		}

		public override void Update(double deltaT)
		{
			if (Input.Unhandled.Keys[KeyCode.Escape])
				SceneFinished = true;
		}

		public override void Draw()
		{
			Display.Clear(Color.DarkRed);

			sp.RotationAngleDegrees += 180 * Display.DeltaTime / 1000.0;
			sp.Draw(location);
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