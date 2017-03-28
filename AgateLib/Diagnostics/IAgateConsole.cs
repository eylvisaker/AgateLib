//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.InputLib;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Interface for a class providing an implementation of the console window.
	/// </summary>
	public interface IAgateConsole : IInputHandler
	{
		/// <summary>
		/// Event raised when the visibility of the console window is changed, usually by the
		/// user pressing the toggle key.
		/// </summary>
		event EventHandler VisibleChanged;

		/// <summary>
		/// Event raised after the console toggles a key input from the user.
		/// </summary>
		event EventHandler KeyProcessed;

		/// <summary>
		/// Gets or sets the visibility of the console window.
		/// </summary>
		bool IsVisible { get; set; }

		/// <summary>
		/// Returns the list of messages displayed in the console.
		/// </summary>
		IReadOnlyList<ConsoleMessage> Messages { get; }

		/// <summary>
		/// Gets or sets the list of command libraries available to process user input.
		/// </summary>
		IList<ICommandLibrary> CommandLibraries { get; set; }

		/// <summary>
		/// Gets the input text the user has typed.
		/// </summary>
		string InputText { get; }

		/// <summary>
		/// Gets the amount of scrollback from the user pressing the scrolling keys.
		/// </summary>
		int ViewShift { get; }

		/// <summary>
		/// Gets the location of the insertion point in the input text.
		/// </summary>
		int InsertionPoint { get; }

		/// <summary>
		/// Writes a line to the console.
		/// </summary>
		/// <param name="text"></param>
		void WriteLine(string text);

		/// <summary>
		/// Writes a message to the console.
		/// </summary>
		/// <param name="message"></param>
		void WriteMessage(ConsoleMessage message);

		/// <summary>
		/// Executes the command as if the user had typed it in.
		/// </summary>
		/// <param name="command"></param>
		void Execute(string command);
	}
}
