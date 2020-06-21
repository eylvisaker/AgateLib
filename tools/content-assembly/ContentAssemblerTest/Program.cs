using Microsoft.Xna.Framework;
using System;

namespace ContentPipelineTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new ContentAssemblerTestGame())
                game.Run();
        }
    }
}
