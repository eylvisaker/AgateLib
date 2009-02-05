using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Drivers;

namespace AgateOTK
{
    class Otk_Reporter : AgateDriverReporter 
    {
        public override IEnumerable<AgateDriverInfo> ReportDrivers()
        {
            yield return new AgateDriverInfo(
                 DisplayTypeID.OpenGL, typeof(GL_Display), "OpenGL through OpenTK 0.9.2", 1120);

            bool reportOpenAL = true;

            try
            {
                // test for the presence of working OpenAL.
                new OpenTK.Audio.AudioContext().Dispose();

            }
            catch (Exception e)
            {
                reportOpenAL = false;
            }

            if (reportOpenAL)
            {
                yield return new AgateDriverInfo(
                    AudioTypeID.OpenAL, typeof(AL_Audio), "OpenAL through OpenTK 0.9.2", 100);
            }
        }
    }
}
