using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Resources
{
    /// <summary>
    /// AgateException derived exception class used when there is a problem reading
    /// from a resource file.
    /// </summary>
    [global::System.Serializable]
    public class AgateResourceException : AgateException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        /// <summary>
        /// 
        /// </summary>
        public AgateResourceException() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AgateResourceException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public AgateResourceException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected AgateResourceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
