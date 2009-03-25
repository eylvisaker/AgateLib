//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
