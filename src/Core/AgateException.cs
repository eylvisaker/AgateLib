using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
{
    /// <summary>
    /// Base exception class for exceptions which are thrown by AgateLib.
    /// </summary>
    [global::System.Serializable]
    public class AgateException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// Constructs an AgateException.
        /// </summary>
        public AgateException() { }
        /// <summary>
        /// Constructs an AgateException.
        /// </summary>
        public AgateException(string message) : base(message) { }
        /// <summary>
        /// Constructs an AgateException.
        /// </summary>
        public AgateException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Constructs an AgateException.
        /// </summary>
        protected AgateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

}
