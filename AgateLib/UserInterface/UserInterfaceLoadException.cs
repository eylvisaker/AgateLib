using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface
{
    [Serializable]
    public class UserInterfaceLoadException : Exception
    {
        public UserInterfaceLoadException() { }
        public UserInterfaceLoadException(string message) : base(message) { }
        public UserInterfaceLoadException(string message, Exception inner) : base(message, inner) { }
        protected UserInterfaceLoadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
