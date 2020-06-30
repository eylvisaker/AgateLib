using System;

namespace AgateLib.Demo.WindowsDX
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new AgateLibDemoGame_WindowsDX())
                game.Run();
        }
    }
}
