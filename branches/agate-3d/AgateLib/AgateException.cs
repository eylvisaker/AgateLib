using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib
{
    /// <summary>
    /// Base exception class for exceptions which are thrown by AgateLib.
    /// </summary>
    [global::System.Serializable]
    public class AgateException : Exception
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
        public AgateException(Exception inner, string message) : base(message, inner) { }
        /// <summary>
        /// Constructs an AgateException, calling string.Format on the arguments.
        /// </summary>
        public AgateException(string format, params object[] args)
            : base(string.Format(format, args)) { }
        /// <summary>
        /// Constructs an AgateException.
        /// </summary>
        public AgateException(Exception inner, string format, params object[] args)
            : base(string.Format(format, args), inner) { }

        /// <summary>
        /// Deserializes an AgateException.
        /// </summary>
        protected AgateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class AgateCrossPlatformException : AgateException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public AgateCrossPlatformException() { }
        public AgateCrossPlatformException(string message) : base(message) { }
        public AgateCrossPlatformException(string message, Exception inner) : base(inner, message) { }
        protected AgateCrossPlatformException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}