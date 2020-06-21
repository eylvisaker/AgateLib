using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.ContentAssembler.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void LogError(string subcategory, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs)
            => Console.WriteLine(message);
        public void LogError(string message) => Console.WriteLine(message);

        public void LogWarning(string subcategory, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs)
            => Console.WriteLine(message);
        public void LogWarning(string message) => Console.WriteLine(message);
    }
}
