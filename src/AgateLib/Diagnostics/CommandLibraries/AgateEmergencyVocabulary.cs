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
using System.Linq;

namespace AgateLib.Diagnostics.CommandLibraries
{
    internal class AgateEmergencyVocabulary : Vocabulary
    {
        private readonly ConsoleShell agateConsoleCore;

        public AgateEmergencyVocabulary(ConsoleShell agateConsoleCore)
        {
            this.agateConsoleCore = agateConsoleCore;
        }

        private bool CallHelpOnCommandLibraries()
        {
            var commandLibraries = agateConsoleCore.CommandLibrarySet.ToList();

            if (!commandLibraries.Any())
                return false;

            WriteLine("Available Commands:");

            foreach (var commandLibrary in commandLibraries)
            {
                commandLibrary.Shell = Shell;
                commandLibrary.Help();
            }


            return true;
        }

        [ConsoleCommand("Provides help for commands. You can type 'help' or 'help <command>' to get more information.", Hidden = true)]
        public void Help([JoinArgs] string command = null)
        {
            if (!string.IsNullOrEmpty(command))
            {
                bool helped = false;

                foreach (var commandLibrary in agateConsoleCore.CommandLibrarySet)
                {
                    commandLibrary.Shell = Shell;
                    helped |= commandLibrary.Help(command);
                }

                if (!helped)
                {
                    WriteLine("Command not found.");
                }

                return;
            }

            if (!CallHelpOnCommandLibraries())
            {
                WriteLine("No command libraries installed.");
                WriteLine("Available Commands:");
                WriteLine("    debug [on|off]");
                WriteLine("    quit");
            }

            WriteLine("Use up / down arrow keys to navigate input history.");
            WriteLine("Use shift+up / shift+down to view output history.");
        }

        [ConsoleCommand("Quits the application. No option to save is given.", Hidden = true)]
        public void Quit()
        {
            Action quit = Shell.State.Quit;

            if (quit == null)
            {
                WriteLine("Sorry, no quit handler is installed.");
            }

            quit?.Invoke();
        }

        [ConsoleCommand("Enable or disable debug info", Hidden = true)]
        public void Debug([Param("Either \"on\" or \"off\"")] string mode = "")
        {
            mode = mode.ToLowerInvariant();

            if (mode == "off")
            {
                Shell.WriteLine("Disabling debug information.");
                Shell.State.Debug = false;
                return;
            }
            if (mode == "on")
            {
                Shell.WriteLine("Enabling debug information. Type 'debug off' to turn it off.");
                Shell.State.Debug = true;
                return;
            }

            Shell.WriteLine("Type 'debug on' to enable debug information.");
            Shell.WriteLine("Type 'debug off' to disable debug information.");
            Shell.WriteLine($"Debug information is currently {(Shell.State.Debug ? "ON" : "OFF")}");
        }
    }
}
