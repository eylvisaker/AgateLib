// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.InputLib.Old;

namespace InputStateTester
{
    static class InputStateTester
    {
        enum Stuff
        {
            up,
            down,
            left,
            right,

            one,
            two,
            three,
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string []args)
		{		
			// These two lines are used by AgateLib tests to locate
			// driver plugins and images.
			AgateLib.Utility.FileManager.AssemblyPath.Add("../Libraries");
			AgateLib.Utility.FileManager.ImagePath.Add("../../../Tests/TestImages");

            using (AgateSetup setup = new AgateSetup("My app", args))
            {
                setup.AskUser = true;
                setup.UseAudio = false;
                setup.InitializeAll();

                if (setup.WasCanceled)
                    return;

                DisplayWindow wind = new DisplayWindow("My app", 200, 300);
                FontSurface font = new FontSurface("Times new roman", 14);

                InputState<Stuff> state = new InputState<Stuff>();

                state[Stuff.up].AddJoystickAxis(0, 1, false);
                state[Stuff.up].AddKey(KeyCode.Up);

                state[Stuff.down].AddJoystickAxis(0, 1, true);
                state[Stuff.down].AddKey(KeyCode.Down);

                state[Stuff.one].AddJoystickButton(0, 0);
                state[Stuff.one].AddKey(KeyCode.Enter);

                state[Stuff.two].AddJoystickButton(0, 1);
                state[Stuff.two].AddKey(KeyCode.Space);

                double fontheight = font.StringDisplayHeight("X");

                while (wind.IsClosed == false)
                {
                    Display.BeginFrame();
                    Display.Clear();

                    state.Update();

                    if (state[Stuff.up].Value)
                        font.DrawText("Up");
                    if (state[Stuff.down].Value)
                        font.DrawText(0, fontheight, "Down");
                    if (state[Stuff.one].Value)
                        font.DrawText(0, fontheight * 4, "Button 1");
                    if (state[Stuff.two].Value)
                        font.DrawText(0, fontheight * 5, "Button 2");

                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }
        }
    }
}