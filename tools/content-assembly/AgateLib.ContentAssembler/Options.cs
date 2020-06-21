using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace VermilionTower.ContentPipeline
{
    public class Options
    {
        [Option]
        public string ContentBuild { get; set; }
    }
}
