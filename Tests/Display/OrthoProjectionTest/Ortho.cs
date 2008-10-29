using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Core;
using AgateLib.Display;
using AgateLib.Geometry;
using AgateLib.Input;

namespace OrthoProjectionTest
{
    static class Ortho
    {
        static int ortho = 0;
        static DisplayWindow wind;

        static void Main()
        {
            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.Cancel)
                    return;

                Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

                wind = new DisplayWindow("Ortho Projection Test", 640, 480, "", false, true);
                
                Surface surf = new Surface("test.png");
                surf.Color = Color.Cyan;

                while (wind.IsClosed == false)
                {
                    AgateDisplay.BeginFrame();
                    AgateDisplay.Clear();

                    switch (ortho)
                    {
                        case 1:
                            AgateDisplay.SetOrthoProjection(0, 0, surf.SurfaceWidth * 2, surf.SurfaceHeight * 2);
                            break;

                        case 2:
                            AgateDisplay.SetOrthoProjection(-surf.SurfaceWidth, -surf.SurfaceHeight,
                                surf.SurfaceWidth, surf.SurfaceHeight);
                            break;
                    }

                    AgateDisplay.FillRect(-2, -2, 4, 4, Color.Red);

                    surf.Draw();

                    AgateDisplay.EndFrame();

                    AgateCore.KeepAlive();
                }
            }
        }

        static void Keyboard_KeyDown(InputEventArgs e)
        {
            if (e.KeyID == KeyCode.Space)
            {
                ortho++;
                if (ortho > 2)
                    ortho = 0;

                Keyboard.ReleaseKey(KeyCode.Space);
            }
            else if (e.KeyID == KeyCode.Escape)
                wind.Dispose();

        }
    }
}
