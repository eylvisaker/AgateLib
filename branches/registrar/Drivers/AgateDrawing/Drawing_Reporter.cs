using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Drivers;

namespace AgateLib.DisplayLib.SystemDrawing
{
    class Drawing_Reporter : AgateDriverReporter 
    {
        public override IEnumerable<AgateDriverInfo> ReportDrivers()
        {
            yield return new AgateDriverInfo(
                DisplayTypeID.Reference,
                typeof(Drawing_Display).FullName,
                "System.Drawing", 
                0);
        }
    }
}
