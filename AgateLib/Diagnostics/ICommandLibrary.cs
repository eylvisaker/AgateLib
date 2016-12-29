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
using AgateLib.Diagnostics.ConsoleSupport;

namespace AgateLib.Diagnostics
{
	public interface ICommandLibrary
	{
		/// <summary>
		/// Called when the user enters "help" on the console. This method should
		/// write a list of commands to the console.
		/// </summary>
		void Help();

		/// <summary>
		/// Returns the help string for the specified command.
		/// </summary>
		/// <param name="command">The command the user is asking for help on.</param>
		void Help(string command);

		/// <summary>
		/// Execute the specified command.
		/// Returns true if the command processor can
		/// execute the command. 
		/// </summary>
		/// <remarks>This method should return true
		/// even if the command cannot be executed due to malformed
		/// arguments or invalid state. False should only be returned
		/// if this ICommandProcessor object cannot ever execute this
		/// command and other command processors should be tried.
		/// </remarks>
		/// <param name="command"></param>
		/// <returns></returns>
		bool Execute(string command);
	}
}
