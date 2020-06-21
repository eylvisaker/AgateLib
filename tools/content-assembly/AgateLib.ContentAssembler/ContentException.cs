using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.ContentAssembler
{

    [Serializable]
    public class ContentException : Exception
    {
        public ContentException() { }
        public ContentException(string message) : base(message) { }
        public ContentException(string message, Exception inner) : base(message, inner) { }
        protected ContentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
