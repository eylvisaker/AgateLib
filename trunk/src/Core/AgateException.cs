using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
{
    [global::System.Serializable]
    public class AgateException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public AgateException() { }
        public AgateException(string message) : base(message) { }
        public AgateException(string message, Exception inner) : base(message, inner) { }
        protected AgateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}
