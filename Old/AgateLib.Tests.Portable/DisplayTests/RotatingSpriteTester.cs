// The contents of this file are public domain.
// You may use them as you wish.
//

using System;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Sprites;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform;

namespace AgateLib.Tests.DisplayTests
{
	class RotatingSprite : Scene, IAgateTest
	{
		Point location = new Point(150, 100);
		Sprite sp;

		public string Name => "Rotating Sprite";

		public string Category => "Display";
		
		protected override void OnSceneStart()
		{
			sp = new Sprite("Images/spike.png", 16, 16)
			{
				RotationCenter = OriginAlignment.Center,
				DisplayAlignment = OriginAlignment.Center,
				RotationAngleDegrees = 90
			};

			sp.SetScale(2, 2);
		}

		protected override void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
			if (Input.Unhandled.Keys[KeyCode.Escape])
				IsFinished = true;
		}

		protected override void OnRedraw()
		{
			Display.Clear(Color.DarkRed);

			sp.RotationAngleDegrees += 180 * AgateApp.GameClock.Elapsed.TotalSeconds;
			sp.Draw(location);
		}

		public void Run(string[] args)
		{
			using (new DisplayWindowBuilder(args)
				.BackbufferSize(800, 600)
				.QuitOnClose()
				.Build())
			{
				new SceneStack().Start(this);
			}
		}
	}
}