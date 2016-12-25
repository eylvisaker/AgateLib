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
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.DisplayTests.RotatingSpriteTester
{
	class RotatingSprite : Scene, ISceneModelTest
	{
		public string Name { get { return "Rotating Sprite"; } }
		public string Category { get { return "Display"; } }

		Point location = new Point(150, 100);
		Sprite sp;

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}
		public Scene StartScene
		{
			get { return this; }
		}

		protected override void OnSceneStart()
		{
			sp = new Sprite("spike.png", 16, 16);

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
	}
}