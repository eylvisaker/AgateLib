using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Serialization.Xle
{
    public interface ITypeBinder
    {
        Type GetType(string typename);
    }
}
