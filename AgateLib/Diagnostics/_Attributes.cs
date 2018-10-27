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

namespace AgateLib.Diagnostics
{
    /// <summary>
    /// Use this attribute on public methods of a IVocabulary object to signify that
    /// those methods are commands the user can enter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ConsoleCommandAttribute : Attribute
    {
        /// <summary>
        /// Constructs a ConsoleMethodAttribute
        /// </summary>
        /// <param name="description">The description of the command give to the user when they type 'help &lt;command&gt;'.</param>
        public ConsoleCommandAttribute(string description)
        {
            Description = description;
        }

        /// <summary>
        /// A description of the command given to the user when they type 'help &lt;command&gt;'
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The name of the command the user types to execute this method.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If true, indicates that the command should not be printed in the list of
        /// commands when the user types 'help'.
        /// </summary>
        public bool Hidden { get; set; }
    }

    /// <summary>
    /// Signifies that a ConsoleCommand should have all its arguments joined 
    /// into the decorated argument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class JoinArgsAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class AliasAttribute : Attribute
    {
        public AliasAttribute(string value) {
            this.Value = value;
        }

        public string Value { get; }
    }
}
