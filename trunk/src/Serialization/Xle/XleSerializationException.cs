using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Serialization.Xle
{
    /// <summary>
    /// Exception thrown when there is a problem in the XleSerializer.
    /// </summary>
    [global::System.Serializable]
    public class XleSerializationException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        internal XleSerializationException() { }
        internal XleSerializationException(string message) : base(message) { }
        internal XleSerializationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Constructs an XleSerializationException object when deserializing it.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected XleSerializationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
