using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.ContentAssembler.Mocks
{
    public class MockLogger
    {
        List<LogRecord> errorLogs = new List<LogRecord>();
        List<LogRecord> warnLogs = new List<LogRecord>();

        public MockLogger()
        {
            Mock.Setup(x => x.LogError(It.IsAny<string>(),
                                       It.IsAny<string>(),
                                       It.IsAny<string>(),
                                       It.IsAny<string>(),
                                       It.IsAny<int>(),
                                       It.IsAny<int>(),
                                       It.IsAny<int>(),
                                       It.IsAny<int>(),
                                       It.IsAny<string>(),
                                       It.IsAny<object[]>()))
                .Callback<string, string, string, string, int, int, int, int, string, object[]>(
                    (subcategory, errorCode, helpKeyword, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, messageArgs) 
                    => errorLogs.Add(new LogRecord
                        {
                            Subcategory = subcategory,
                            ErrorCode = errorCode,
                            HelpKeyword = helpKeyword,
                            File = file,
                            LineNumber = lineNumber,
                            EndLineNumber = endLineNumber,
                            ColumnNumber = columnNumber,
                            EndColumnNumber = endColumnNumber,
                            Message = message,
                            MessageArgs = messageArgs
                        }));

            Mock.Setup(x => x.LogWarning(It.IsAny<string>(),
                                         It.IsAny<string>(),
                                         It.IsAny<string>(),
                                         It.IsAny<string>(),
                                         It.IsAny<int>(),
                                         It.IsAny<int>(),
                                         It.IsAny<int>(),
                                         It.IsAny<int>(),
                                         It.IsAny<string>(),
                                         It.IsAny<object[]>()))
                .Callback<string, string, string, string, int, int, int, int, string, object[]>(
                    (subcategory, errorCode, helpKeyword, file, lineNumber, columnNumber, endLineNumber, endColumnNumber, message, messageArgs)
                    => warnLogs.Add(new LogRecord
                    {
                        Subcategory = subcategory,
                        ErrorCode = errorCode,
                        HelpKeyword = helpKeyword,
                        File = file,
                        LineNumber = lineNumber,
                        EndLineNumber = endLineNumber,
                        ColumnNumber = columnNumber,
                        EndColumnNumber = endColumnNumber,
                        Message = message,
                        MessageArgs = messageArgs
                    }));
        }

        public Mock<ILogger> Mock { get; } = new Mock<ILogger>();

        public IReadOnlyList<LogRecord> ErrorLogs => errorLogs;

        public IReadOnlyList<LogRecord> WarnLogs => warnLogs;

        public ILogger Object => Mock.Object;
    }

    public class LogRecord
    {
        public string Subcategory { get; set; }
        public string ErrorCode { get; set; }
        public string HelpKeyword { get; set; }
        public string File { get; set; }
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }
        public int EndLineNumber { get; set; }
        public int EndColumnNumber { get; set; }
        public string Message { get; set; }
        public object[] MessageArgs { get; set; }

        public string FormattedMessage => string.Format(Message, MessageArgs);
    }
}
