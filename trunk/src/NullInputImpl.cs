using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
{
    class NullInputImpl : InputImpl 
    {

        public static void Register()
        {
            Registrar.RegisterInputDriver(new InputDriverInfo(
                typeof(NullInputImpl), InputTypeID.Silent, "Silent", -100));
        }

        public override void Initialize()
        {
            
        }

        public override void Dispose()
        {
            
        }

        public override int CountJoysticks()
        {
            return 0;
        }

        public override IEnumerable<JoystickImpl> CreateJoysticks()
        {
            return new List<JoystickImpl>();
        }
    }
}
