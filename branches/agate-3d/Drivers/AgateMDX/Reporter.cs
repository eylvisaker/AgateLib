using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Drivers;

namespace AgateMDX
{
    class Reporter : AgateDriverReporter 
    {
        public override IEnumerable<AgateDriverInfo> ReportDrivers()
        {
            yield return new AgateDriverInfo(
                DisplayTypeID.Direct3D_MDX_1_1,
                typeof(MDX1_Display),
                "Managed DirectX 1.1 - Direct3D",
                100);

            yield return new AgateDriverInfo(
                AudioTypeID.DirectSound,
                typeof(MDX1_Audio),
                "Managed DirectX 1.1 - DirectSound",
                100);

            yield return new AgateDriverInfo(
                InputTypeID.DirectInput,
                typeof(MDX1_Input),
                "Managed DirectX 1.1 - DirectInput",
                100);
        }
    }
}
