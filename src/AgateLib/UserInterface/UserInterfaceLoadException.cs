using System;

namespace AgateLib.UserInterface
{
    public class UserInterfaceLoadException : Exception
    {
        public UserInterfaceLoadException() { }
        public UserInterfaceLoadException(string message) : base(message) { }
        public UserInterfaceLoadException(string message, Exception inner) : base(message, inner) { }
    }
}
