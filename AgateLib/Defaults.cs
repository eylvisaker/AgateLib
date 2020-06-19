using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib
{
    public static class Defaults
    {
        static Defaults()
        {
            Reset();
        }

        public static string MonospaceFont { get; set; }

        public static void Reset()
        {
            MonospaceFont = "AgateLib/AgateMono";
        }
    }
}
