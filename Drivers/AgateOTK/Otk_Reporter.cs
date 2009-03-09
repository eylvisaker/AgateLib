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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
                 DisplayTypeID.OpenGL, typeof(GL_Display), "OpenGL through OpenTK 0.9.5", 1120);

            if (ReportOpenAL())
            {
                yield return new AgateDriverInfo(
                    AudioTypeID.OpenAL, typeof(AL_Audio), "OpenAL through OpenTK 0.9.5", 100);
            }
        }

        bool ReportOpenAL()
        {
            try
            {
                // test for the presence of working OpenAL.
                new OpenTK.Audio.AudioContext().Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
