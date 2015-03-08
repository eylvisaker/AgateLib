﻿using AgateLib.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Platform.Test
{
    public class FakeAgateConsole : AgateConsole
    {
        protected override void WriteLineImpl(string text)
        {
        }

        protected override void WriteImpl(string text)
        {
        }

        protected override long CurrentTime
        {
            get { return 0; }
        }
    }
}
