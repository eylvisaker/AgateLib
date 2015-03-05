using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgateLib.Drivers;

namespace AgateLib.UnitTests.Fakes
{
    public class FakeInputFactory : IInputFactory
    {
        public InputLib.ImplementationBase.InputImpl CreateJoystickInputImpl()
        {
            throw new NotImplementedException();
        }
    }
}
