﻿//     The contents of this file are subject to the Mozilla Public License
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
	public interface IAgateConsole : IInputHandler
	{
		event EventHandler VisibleChanged;
		event EventHandler KeyProcessed;

		bool IsVisible { get; set; }

		IReadOnlyList<ConsoleMessage> Messages { get; }

		IList<ICommandLibrary> CommandLibraries { get; set; }
		string InputText { get; }
		int ViewShift { get; }
		int InsertionPoint { get; }

		void WriteLine(string text);
		void WriteMessage(ConsoleMessage message);

		/// <summary>
		/// Executes the command as if the user had typed it in.
		/// </summary>
		/// <param name="command"></param>
		void Execute(string command);
	}
}
