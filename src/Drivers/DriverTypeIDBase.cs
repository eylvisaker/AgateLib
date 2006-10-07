using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Drivers
{
    public abstract class DriverTypeIDBase
        : IEquatable<int>
    {
        #region --- IEquatable<int> Members ---

        public abstract bool Equals(int other);

        #endregion
    }
}
