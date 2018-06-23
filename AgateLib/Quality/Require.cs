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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Quality
{
    public static class Require
    {
        /// <summary>
        /// Throws an ArgumentNull Exception if the specified
        /// argument is null.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="param">The argument which should not be null.</param>
        /// <param name="paramName">The nameof(param).</param>
        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T param, string paramName) where T : class
        {
            ArgumentNotNull(param, paramName, paramName + " must not be null");
        }

        /// <summary>
        /// Throws an ArgumentNull Exception if the specified
        /// argument is null.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="param">The argument which should not be null.</param>
        /// <param name="paramName">The nameof(param).</param>
        /// <param name="message">Message of the exception should param be null.</param>
        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T param, string paramName, string message) where T : class
        {
            if (param != null)
                return;

            throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Throws an ArgumentNull Exception if the specified
        /// argument is null.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="param">The argument which should not be null.</param>
        /// <param name="paramName">The nameof(param).</param>
        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T? param, string paramName) where T : struct
        {
            ArgumentNotNull(param, paramName, paramName + " must not be null");
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the condition is not met.
        /// </summary>
        /// <param name="condition">If false, this method throws an exception.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">Message for the exception.</param>
        [DebuggerStepThrough]
        public static void ArgumentInRange(bool condition, string paramName, string message)
        {
            if (condition)
                return;

            throw new ArgumentOutOfRangeException(paramName, message);
        }
        /// <summary>
        /// Throws an ArgumentNull Exception if the specified
        /// argument is null.
        /// </summary>
        /// <typeparam name="T">The type of the argument.</typeparam>
        /// <param name="param">The argument which should not be null.</param>
        /// <param name="paramName">The nameof(param).</param>
        /// <param name="message">Message of the exception should param be null.</param>
        [DebuggerStepThrough]
        public static void ArgumentNotNull<T>(T? param, string paramName, string message) where T : struct
        {
            if (param != null)
                return;

            throw new ArgumentNullException(paramName, message);
        }

        /// <summary>
        /// Throws an exception if the value of state is false.
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="state">If this value is false, an exception is thrown.</param>
        /// <param name="message"></param>
        [DebuggerStepThrough]
        public static void That<TE>(bool state, string message)
            where TE : Exception, new()
        {
            if (state)
                return;

            var exception = (TE)Activator.CreateInstance(typeof(TE), message);
            throw exception;
        }

        /// <summary>
        /// Throws an InvalidOperationException if the value of state is false.
        /// </summary>
        /// <param name="state">If this value is false, an InvalidOperationException is thrown.</param>
        /// <param name="message">Exception message.</param>
        [DebuggerStepThrough]
        public static void That(bool state, string message)
        {
            That<InvalidOperationException>(state, message);
        }

        /// <summary>
        /// Throws an exception if the value of state is true.
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="state">If this value is true, an exception is thrown.</param>
        /// <param name="message"></param>
        [DebuggerStepThrough]
        public static void Not<TE>(bool state, string message)
            where TE : Exception, new()
        {
            if (!state)
                return;

            var exception = (TE)Activator.CreateInstance(typeof(TE), message);
            throw exception;
        }

        /// <summary>
        /// Throws an exception if the value of state is true.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="message"></param>
        [DebuggerStepThrough]
        public static void Not(bool state, string message)

        {
            Not<InvalidOperationException>(state, message);
        }

        [DebuggerStepThrough]
        public static void Not(bool state, string message, Func<string, Exception> activator)
        {
            if (!state)
                return;

            var exception = activator(message);
            throw exception;
        }
    }

    public static class CommonException
    {
        public static readonly Func<string, Exception> InvalidOperationException 
            = message => new InvalidOperationException(message);
        public static readonly Func<string, ArgumentException> ArgumentException 
            = message => new ArgumentException(message);
    }
}
