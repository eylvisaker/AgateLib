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
        public AgateException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Constructs an AgateException.
        /// </summary>
        protected AgateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception which is thrown when AgateLib detects that it is used in a way that
    /// may not be portable to different platforms, 
    /// and Core.CrossPlatformDebugLevel is set to Exception.
    /// </summary>
    [global::System.Serializable]
    public class AgateCrossPlatformException : AgateException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// Constructs a new AgateCrossPlatformException object.
        /// </summary>
        public AgateCrossPlatformException() { }
        /// <summary>
        /// Constructs a new AgateCrossPlatformException object.
        /// </summary>
        public AgateCrossPlatformException(string message) : base(message) { }
        /// <summary>
        /// Constructs a new AgateCrossPlatformException object.
        /// </summary>
        public AgateCrossPlatformException(string message, Exception inner) : base(message, inner) { }
        protected AgateCrossPlatformException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
