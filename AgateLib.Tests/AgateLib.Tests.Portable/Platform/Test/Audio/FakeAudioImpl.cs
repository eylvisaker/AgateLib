using AgateLib.AudioLib.ImplementationBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.Test.Audio
{
    public class FakeAudioImpl : AudioImpl
    {
        protected override bool CapsBool(AudioLib.AudioBoolCaps audioBoolCaps)
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
        }
    }
}
