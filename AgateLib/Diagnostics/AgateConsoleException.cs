using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Diagnostics
{
    [Serializable]
    public class AgateConsoleException : Exception
    {
        public AgateConsoleException() { }
        public AgateConsoleException(string message) : base(message) { }
        public AgateConsoleException(string message, Exception inner) : base(message, inner) { }
        protected AgateConsoleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
