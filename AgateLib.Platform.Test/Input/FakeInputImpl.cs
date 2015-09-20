using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.InputLib.ImplementationBase;

namespace AgateLib.Platform.Test.Input
{
    public class FakeInputImpl : InputImpl
    {
        public override int JoystickCount
        {
            get { return 0; }
        }

        public override IEnumerable<JoystickImpl> CreateJoysticks()
        {
            yield break;
        }

        public override void Poll()
        {
        }

        public override void Initialize()
        {
        }
    }
}
