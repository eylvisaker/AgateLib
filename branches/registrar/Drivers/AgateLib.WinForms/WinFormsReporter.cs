using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Drivers;

namespace AgateLib.WinForms
{
    class WinFormsReporter:AgateDriverReporter 
    {
        public override IEnumerable<AgateDriverInfo> ReportDrivers()
        {
            yield return new AgateDriverInfo(
                 DesktopTypeID.WinForms,
                 typeof(WinFormsDriver).FullName,
                 "Windows.Forms",
                 0);
        }
    }
}
