// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;

using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.Sprites.Old;
using AgateLib.InputLib;

namespace RotatingSpriteTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
		{		
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateFileProvider.Assemblies.AddPath("../Drivers");
			AgateFileProvider.Images.AddPath("../../../Tests/TestImages");

            using (AgateSetup setup = new AgateSetup())
            {
                //setup.AskUser = true;
                setup.Initialize(true, false, false);

                if (setup.WasCanceled)
                    return;

                DisplayWindow wind = new DisplayWindow("Rotating sprite", 300, 300);
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
            }
        }
    }
}