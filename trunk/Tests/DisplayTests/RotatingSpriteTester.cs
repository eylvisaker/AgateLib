// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;

using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.Sprites;
using AgateLib.InputLib;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.InputLib.Legacy;

namespace Tests.RotatingSpriteTester
{
	class RotatingSprite : IAgateTest
	{
		#region IAgateTest Members

		public string Name { get { return "Rotating Sprite"; } }
		public string Category { get { return "Display"; } }

		#endregion

		public void Main(string[] args)
		{
			new PassiveModel(args).Run( () =>
			{
				DisplayWindow wind = DisplayWindow.CreateWindowed("Rotating sprite", 300, 300);
				Sprite sp = new Sprite("spike.png", 16, 16);

				sp.RotationCenter = OriginAlignment.Center;
				sp.DisplayAlignment = OriginAlignment.Center;

				sp.RotationAngleDegrees = 90;
				sp.SetScale(2, 2);

				Point location = new Point(150, 100);

				while (wind.IsClosed == false)
				{
					Display.BeginFrame();
					Display.Clear(Color.DarkRed);

					sp.RotationAngleDegrees += 180 * Display.DeltaTime / 1000.0;
					sp.Draw(location);

					Display.DrawRect(location.X, location.Y, 1, 1, Color.YellowGreen);

					Display.EndFrame();
					Core.KeepAlive();

					if (Keyboard.Keys[KeyCode.F5])
					{
						if (wind.IsFullScreen)
						{
							wind.SetWindowed();
							wind.Size = new Size(500, 500);
						}
						else
						{
							wind.SetFullScreen(800, 600, 32);
						}

						Keyboard.ReleaseKey(KeyCode.F5);
					}
					if (Keyboard.Keys[KeyCode.Escape])
						return;
				}
			});
		}
	}
}