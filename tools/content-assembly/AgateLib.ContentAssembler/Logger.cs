namespace AgateLib.ContentAssembler
{
    public interface ILogger
    {
        void LogError(string subcategory, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs);

        void LogError(string message);

        void LogWarning(string subcategory, string errorCode, string helpKeyword, string file, int lineNumber, int columnNumber, int endLineNumber, int endColumnNumber, string message, params object[] messageArgs);

        void LogWarning(string message);
    }
}