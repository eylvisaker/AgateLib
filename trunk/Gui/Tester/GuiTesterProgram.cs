using System;
using System.Collections.Generic;
using System.Linq;
using AgateLib;
using AgateLib.DisplayLib;

namespace AgateLib.Gui.Tester
{
    static class GuiTesterProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AgateFileProvider.Assemblies.AddPath("../Drivers");

            using (AgateSetup setup = new AgateSetup())
            {
                setup.AskUser = true;
                setup.Initialize(true, false, false);
                if (setup.WasCanceled)
                    return;

                DisplayWindow wind = new DisplayWindow(CreateWindowParams.Windowed("GUI Test", 800, 600, null, true));

                RenderGui renderer = new RenderGui();

                renderer.Run();
            }
        }
    }
}
