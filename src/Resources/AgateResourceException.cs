using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Resources
{
    [global::System.Serializable]
    public class AgateResourceException : AgateException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public AgateResourceException() { }
        public AgateResourceException(string message) : base(message) { }
        public AgateResourceException(string message, Exception inner) : base(message, inner) { }
        protected AgateResourceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
