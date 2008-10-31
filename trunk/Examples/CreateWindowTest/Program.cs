// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using AgateLib.Core;
using AgateLib.Display;
using AgateLib.Geometry;
using AgateLib.Input;

namespace CreateWindowTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // If you are debugging this in visual studio 2005, be sure to turn off
            // LoaderLock exceptions from occuring.
            // It's under Debug->Exceptions->Managed Debug Assistance->LoaderLock.

            // Creates an AgateSetup object to create and setup the Agate library.
            using (AgateSetup setup = new AgateSetup())
            {
                // initialize the display, asking the user what display driver to use.
                setup.Initialize(true, false, false);
                
                // normally, the display should initialize fine.
                // However, here we are asking the user what display mode they want to pick,
                // and they may push the cancel button.  If they do, then exit the program.
                if (setup.Cancel)
                    return;

                // This creates the window that we will be drawing in.
                // 640x480 are the dimensions of the screen area that we will write to
                DisplayWindow wind = new DisplayWindow(CreateWindowParams.Windowed(
                    "Initialize Example", 640, 480, null, true));

                // create a random number generation object 
                // so that we can make pretty colors.
                Random rand = new Random();

                while (wind.IsClosed == false)
                {
                    // Display.BeginFrame must be called before any rendering takes place.
                    AgateDisplay.BeginFrame();

                    // Clear back buffer with red
                    AgateDisplay.Clear(Color.Red);

                    // draw random lines and boxes

                    // line drawn starts at (0, 0), the upper left corner.
                    AgateDisplay.DrawLine(
                        0, 0,
                        rand.Next(10, 700), rand.Next(10, 700),
                        Color.Black);

                    AgateDisplay.DrawRect(
                        new Rectangle(
                        rand.Next(0, 540), rand.Next(0, 380),
                        rand.Next(50, 200), rand.Next(50, 200)),
                        Color.Black);

                    AgateDisplay.FillRect(
                        new Rectangle(
                        rand.Next(0, 540), rand.Next(0, 380),
                        rand.Next(50, 200), rand.Next(50, 200)),
                        Color.Black);

                    // Display.EndFrame must be called after rendering is done
                    // in order to actually update the display.
                    AgateDisplay.EndFrame();

                    // Core.KeepAlive is where we play nice window the OS, 
                    // allowing events to be processed and such.
                    // This is also required to process events that happen in our OWN 
                    // code (ie. user input), so be sure to call this once a frame.
                    AgateCore.KeepAlive();

                    // This gives a 100 millisecond delay between each frame.
                    // Using the Sleep() call causes this application to
                    // relinquish CPU time.
                    System.Threading.Thread.Sleep(100);

                    // toggle full screen if the user pressed F5
                    if (Keyboard.Keys[KeyCode.F5])
                    {
                        if (AgateDisplay.CurrentWindow.IsFullScreen)
                            AgateDisplay.CurrentWindow.SetWindowed();
                        else
                            AgateDisplay.CurrentWindow.SetFullScreen();

                        // make that we used this keypress
                        Keyboard.Keys[KeyCode.F5] = false;
                    }
                    
                    // and exit the application if the user pressed escape.
                    if (Keyboard.Keys[KeyCode.Escape])
                    {
                        return;
                    }
                }

            }
        }
    }
}
