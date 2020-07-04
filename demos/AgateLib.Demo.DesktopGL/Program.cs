using System;

namespace AgateLib.Demo.DesktopGL
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new AgateLibDemoGame_DesktopGL())
                game.Run();
        }
    }
}
