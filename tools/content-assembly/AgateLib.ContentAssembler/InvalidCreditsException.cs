using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.ContentAssembler
{
    [Serializable]
    public class InvalidCreditsException : Exception
    {
        public InvalidCreditsException() { }
        public InvalidCreditsException(string message) : base(message) { }
        public InvalidCreditsException(string message, Exception inner) : base(message, inner) { }
        protected InvalidCreditsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
