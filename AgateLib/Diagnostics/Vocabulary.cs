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


namespace AgateLib.Diagnostics
{
    /// <summary>
    /// Interface for a class which provides commands to a LibraryVocabulary object.
    /// </summary>
    public interface IVocabulary
    {
        /// <summary>
        /// If this value is not empty, then commands must be prefixed by "namespace."
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets whether this vocabulary should be available regardless of the current path.
        /// </summary>
        bool IsGlobal { get; }

        /// <summary>
        /// If false, none of the commands in this vocabulary will be available to
        /// the user.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// This allows that game to enable and disable vocabularies based on context.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Provides a means to interact with the console.
        /// </summary>
        IConsoleShell Shell { get; set; }
    }

    public abstract class Vocabulary : IVocabulary
    {
        public abstract string Path { get; }

        public IConsoleShell Shell { get; set; }

        public virtual bool IsValid => true;

        public virtual bool IsGlobal => false;

        public bool IsEnabled { get; set; }


        protected void WriteLine(string message = "")
        {
            Shell.WriteLine(message);
        }
    }
}
