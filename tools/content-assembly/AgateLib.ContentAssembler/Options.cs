using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.ContentAssembler
{
    public class Options
    {
        [Option]
        public string ContentBuild { get; set; }
    }
}
