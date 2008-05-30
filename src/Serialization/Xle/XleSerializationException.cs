using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Serialization.Xle
{
    /// <summary>
    /// 
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

        public XleSerializationException() { }
        public XleSerializationException(string message) : base(message) { }
        public XleSerializationException(string message, Exception inner) : base(message, inner) { }
        protected XleSerializationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
