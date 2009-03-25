using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using AgateLib;
using AgateLib.Drivers;

namespace AgateXNA
{
    public class Reporter : AgateDriverReporter
    {
        public override IEnumerable<AgateDriverInfo> ReportDrivers()
        {
            yield break;
        }
    }
}
