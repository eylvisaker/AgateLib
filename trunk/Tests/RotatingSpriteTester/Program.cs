using System;
using System.Collections.Generic;
using ERY.AgateLib;

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
            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);

                if (setup.Cancel)
                    return;

                DisplayWindow wind = new DisplayWindow("Rotating sprite", 500, 500, false);
                Sprite sp = new Sprite("spike.png", 16, 16);

                sp.RotationCenter = OriginAlignment.Center;
                sp.DisplayAlignment = OriginAlignment.Center;

                sp.RotationAngleDegrees = 90;
                sp.SetScale(2, 2);

                while (wind.Closed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.Black);


                    sp.RotationAngleDegrees += 1;
                    sp.Draw(200, 200);

                    Display.DrawRect(200, 200, 1, 1, Color.YellowGreen);

                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }
        }
    }
}