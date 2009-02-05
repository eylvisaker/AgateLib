using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Drivers;

namespace AgateFMOD
{
    class Reporter : AgateDriverReporter 
    {
        public override IEnumerable<AgateDriverInfo> ReportDrivers()
        {
            yield return new AgateDriverInfo(
                AudioTypeID.FMod,
                typeof(FMOD_Audio),
                "FMOD",
                1);
        }
    }
}
