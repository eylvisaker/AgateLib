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
            if (FmodInstalled())
            {
                yield return new AgateDriverInfo(
                    AudioTypeID.FMod,
                    typeof(FMOD_Audio),
                    "FMOD",
                    1);
            }
        }

        private bool FmodInstalled()
        {
            try
            {
                FMOD_Audio audio = new FMOD_Audio();

                audio.Initialize();
                audio.Dispose();

                return true;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
        }
    }
}
