using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Drivers;

namespace AgateSDL
{
    using Audio;
    using Input;

    class Reporter : AgateDriverReporter 
    {

        public override IEnumerable<AgateDriverInfo> ReportDrivers()
        {
            yield return new AgateDriverInfo(
                AudioTypeID.SDL,
                typeof(SDL_Audio),
                "SDL with SDL_mixer",
                300);

            yield return new AgateDriverInfo(
                InputTypeID.SDL,
                typeof(SDL_Input),
                "SDL Input",
                300);
        }
    }
}
