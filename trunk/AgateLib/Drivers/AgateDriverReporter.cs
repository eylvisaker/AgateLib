using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Drivers
{
    /// <summary>
    /// Class which is used by the <c>Registrar</c> to enumerate the drivers available
    /// in a satellite assembly.  Any assembly with drivers for use by AgateLib should
    /// have a class inheriting from AgateDriverReporter and should construct AgateDriverInfo
    /// objects for each driver in the assembly.
    /// </summary>
    [Serializable]
    public abstract class AgateDriverReporter
    {
        /// <summary>
        /// Constructs and returns AgateDriverInfo objects for each driver
        /// in the hosted assembly.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<AgateDriverInfo> ReportDrivers();
    }
}
