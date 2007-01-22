using System;
using System.Collections.Generic;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.Gui;
using ERY.AgateLib.Gui.Styles;

namespace GuiTest
{
    static class GuiTest
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

                DisplayWindow wind = new DisplayWindow(CreateWindowParams.Windowed("Gui Tester", 800, 600, "", true));


                PlainStyle style = new PlainStyle();
                GuiManager gui = new GuiManager(style);

                Window window = new Window(gui, new Rectangle(300, 50, 300, 200), "Title");

                Button btn = new Button(window, new Point(10, 10), "Hello world");
                Label text = new Label(window, new Point(100, 10), "Test Label");

                bool toggle = false;

                btn.Anchor = Anchor.Bottom | Anchor.Left;
                btn.Click += delegate(object sender, EventArgs args)
                    {
                        if (!toggle)
                        {
                            window.Width += 100;
                            window.Height += 100;
                        }
                        else
                        {
                            window.Width -= 100;
                            window.Height -= 100;
                        }

                        toggle = !toggle;
                        window.SkipSizeTransition();

                    };

                while (wind.IsClosed == false)
                {
                    Display.BeginFrame();
                    Display.Clear(Color.Gray);

                    gui.Draw();

                    
                    Display.EndFrame();
                    Core.KeepAlive();
                }
            }
        }

        static void btn_Click(object sender, EventArgs e)
        {
            Display.BeginFrame();
            Display.Clear(Color.Blue);
            Display.EndFrame();
        }
    }
}