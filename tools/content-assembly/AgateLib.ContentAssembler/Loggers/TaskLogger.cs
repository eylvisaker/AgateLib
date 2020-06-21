using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;

namespace VermilionTower.ContentPipeline.Loggers
{
    class TaskLogger : ILogger
    {
        private TaskLoggingHelper log;

        public TaskLogger(TaskLoggingHelper log)
        {
            this.log = log;
        }

        public void LogError(string subcategory, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs)
            => log.LogError(subcategory, errorCode, helpKeyword, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, messageArgs);
        public void LogError(string message) => log.LogError(message);

        public void LogWarning(string subcategory, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs)
            => log.LogWarning(subcategory, errorCode, helpKeyword, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, messageArgs);
        public void LogWarning(string message) => log.LogWarning(message);
    }
}
