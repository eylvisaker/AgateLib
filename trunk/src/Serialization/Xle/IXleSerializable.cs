using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Serialization.Xle
{
    public interface IXleSerializable
    {
        void WriteData(XleSerializationInfo info);
        void ReadData(XleSerializationInfo info);
    }
}
