using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
{
    /// <summary>
    /// Base class all driver classes should implement.
    /// </summary>
    public abstract class DriverImplBase: IDisposable 
    {
        /// <summary>
        /// Initialization beyond what the constructor does.
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Disposes of unmanaged resources.
        /// </summary>
        public abstract void Dispose();
    }
}
