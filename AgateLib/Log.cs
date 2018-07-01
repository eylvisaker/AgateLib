//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib
{
    /// <summary>
    /// A basic logging interface.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// A list of listeners that receive messages from the log.
        /// Defaults to logging to standard out.
        /// </summary>
        public static List<ILogListener> Listeners { get; } = new List<ILogListener> { new SystemConsoleLogger() };

        /// <summary>
        /// Writes a message to all listeners that are listening at the specified level
        /// or below.
        /// </summary>
        /// <param name="level">The minimum LogLevel value for a listener to received the message.</param>
        /// <param name="textFunc">A callback function to generate the log message. This function will only
        /// be called once, and only if a listener is configured to receive messages at the current log level.</param>
        public static void WriteLine(LogLevel level, Func<string> textFunc)
        {
            if (textFunc == null)
                return;

            string text = null;

            foreach (var listener in Listeners)
            {
                if (level >= listener.Level)
                {
                    if (text == null)
                        text = textFunc();

                    listener?.WriteLine(level, text);
                }
            }
        }

        /// <summary>
        /// Writes a message to all listeners that are listening at the specified level
        /// or below.
        /// </summary>
        /// <param name="level">The minimum LogLevel value for a listener to received the message.</param>
        /// <param name="text">The message to send.</param>
        public static void WriteLine(LogLevel level, string text)
        {
            foreach (var listener in Listeners)
            {
                if (level >= listener.Level)
                {
                    listener?.WriteLine(level, text);
                }
            }
        }

        public static void Debug(string text)
        {
            WriteLine(LogLevel.Debug, text);
        }

        public static void Info(string text)
        {
            WriteLine(LogLevel.Info, text);
        }

        public static void Warn(string text)
        {
            WriteLine(LogLevel.Warn, text);
        }

        public static void Error(string text)
        {
            WriteLine(LogLevel.Error, text);
        }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Performance,
        Warn,
        Error,
    }

    public interface ILogListener
    {
        LogLevel Level { get; set; }

        void WriteLine(LogLevel level, string text);
    }

    public class SystemConsoleLogger : ILogListener
    {
        public LogLevel Level { get; set; }
#if DEBUG
        = LogLevel.Debug;
#else
        = LogLevel.Warn;
#endif

        public void WriteLine(LogLevel level, string text)
        {
            Console.WriteLine(text);
        }
    }

}
