using System;
using System.Collections.Generic;

namespace BallBuster.Net
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            BBX bbx = new BBX();

            bbx.Main(args);
        }
    }
}