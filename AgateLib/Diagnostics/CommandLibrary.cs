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

using System.Collections.Generic;

namespace AgateLib.Diagnostics
{
    /// <summary>
    /// Interface for a class which can process user input at the console.
    /// </summary>
    public interface ICommandLibrary
    {
        IConsoleShell Shell { get; set; }

        /// <summary>
        /// The path which contains the commands in this library.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets whether this library is global and can be accessed anywhere regardless of the path.
        /// </summary>
        bool IsGlobal { get; }

        /// <summary>
        /// Called when the user enters "help" on the console. This method should
        /// write a list of commands to the console.
        /// </summary>
        void Help();

        /// <summary>
        /// Returns the help string for the specified command.
        /// </summary>
        /// <param name="command">The command the user is asking for help on.</param>
        /// <returns>True if this command library is responsible for this command.</returns>
        bool Help(string command);

        /// <summary>
        /// Execute the specified command.
        /// Returns true if the command processor can
        /// execute the command. 
        /// </summary>
        /// <remarks>This method should return true
        /// even if the command cannot be executed due to malformed
        /// arguments or invalid state. False should be returned
        /// if other command processors should be tried.
        /// </remarks>
        /// <param name="command"></param>
        /// <returns></returns>
        bool Execute(string command);

        /// <summary>
        /// Performs autocompletion on the specified command.
        /// Returns a list of commands which match the input string.
        /// </summary>
        /// <param name="inputString">The text the user has typed so far.</param>
        /// <returns></returns>
        IEnumerable<string> AutoCompleteEntries(string inputString);
    }
}
