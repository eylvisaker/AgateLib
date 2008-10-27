using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Serialization.Xle
{
    /// <summary>
    /// Interface that must be implemented in order to make an object serializable with
    /// the XleSerializer.
    /// </summary>
    public interface IXleSerializable
    {
        void WriteData(XleSerializationInfo info);
        void ReadData(XleSerializationInfo info);
    }
}
