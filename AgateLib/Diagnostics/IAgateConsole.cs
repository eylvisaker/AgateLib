//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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
