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
                Button btn = new Button(gui, new Rectangle(10, 10, 80, 20), "Hello world");
                Button btn2 = new Button(gui, new Rectangle(10, 40, 80, 20), "Chonk");

                btn2.BackColor = Color.Red;

                btn.Click += new EventHandler(btn_Click);

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