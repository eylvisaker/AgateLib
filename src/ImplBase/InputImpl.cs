using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.ImplBase
{
    /// <summary>
    /// Implementation for Input Manager.
    /// </summary>
    public abstract class InputImpl : IDisposable 
    {
        /// <summary>
        /// Initializes
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Destroyes
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Polls and counts joysticks
        /// </summary>
        /// <returns></returns>
        public abstract int CountJoysticks();

        /// <summary>
        /// Creates joystick impls.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<JoystickImpl> CreateJoysticks();
    }
}
