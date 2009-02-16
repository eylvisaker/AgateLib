using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.ImplementationBase;
using AgateLib.Drivers;

namespace AgateLib.InputLib
{
    /// <summary>
    /// Static class which contains a list of the joystick input devices attached
    /// to the computer.
    /// </summary>
    public static class JoystickList 
    {
        static InputImpl impl;
        static List<JoystickImpl> mRawJoysticks = new List<JoystickImpl>();

        /// <summary>
        /// Initializes the input system by instantiating the driver with the given
        /// InputTypeID.  The input driver must be registered with the Registrar
        /// class.
        /// </summary>
        /// <param name="inputType"></param>
        public static void Initialize(InputTypeID inputType)
        {
            Core.Initialize();

            impl = Registrar.CreateInputDriver(inputType);
            impl.Initialize();

            Old.Input.LegacyInitialize(impl);

            InitializeJoysticks();
        }

        private static void InitializeJoysticks()
        {
            mRawJoysticks.Clear();
            mRawJoysticks.AddRange(impl.CreateJoysticks());
        }


        internal static void PollTimer()
        {
        	Old.Input.PollTimer();
        }
    }
}
