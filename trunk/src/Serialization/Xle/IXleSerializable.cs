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
        /// <summary>
        /// This method is called when an object is to be serialized.  The object should
        /// call the Write() methods on the info object in order to write enough of its data
        /// so that it can be deserialized.
        /// </summary>
        /// <param name="info"></param>
        void WriteData(XleSerializationInfo info);
        /// <summary>
        /// This method is called when an object is to be deserialized.  The object should
        /// call the Read*() methods on the info object to reconstruct its internal structure.
        /// </summary>
        /// <param name="info"></param>
        void ReadData(XleSerializationInfo info);
    }
}
