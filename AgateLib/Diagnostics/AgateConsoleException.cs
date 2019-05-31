using System;

namespace AgateLib.Diagnostics
{
    public class AgateConsoleException : Exception
    {
        public AgateConsoleException() { }
        public AgateConsoleException(string message) : base(message) { }
        public AgateConsoleException(string message, Exception inner) : base(message, inner) { }
    }
}
