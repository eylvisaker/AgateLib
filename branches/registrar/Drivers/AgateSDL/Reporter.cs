using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Drivers;

namespace AgateSDL
{
    class Reporter : AgateDriverReporter 
    {

        public override IEnumerable<AgateDriverInfo> ReportDrivers()
        {
            yield return new AgateDriverInfo(
                InputTypeID.SDL,
                typeof(Input_SDL),
                "SDL Input",
                300);
        }
    }
}
