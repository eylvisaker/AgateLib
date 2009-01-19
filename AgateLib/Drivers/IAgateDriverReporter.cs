using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Drivers
{
    [Serializable]
    public abstract class AgateDriverReporter
    {
        public abstract IEnumerable<AgateDriverInfo> ReportDrivers();
    }
}
