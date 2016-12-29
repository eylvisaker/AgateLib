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
using AgateLib.InputLib;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface
{
	public static class GuiStack
	{
		static List<Gui> mStack = new List<Gui>();

		internal static void Add(Gui gui)
		{
			mStack.Add(gui);
		}
		internal static bool Remove(Gui gui)
		{
			return mStack.Remove(gui);
		}

		public static IEnumerable<Gui> Items { get { return mStack; } }

		internal static void ListenEvent(object sender, AgateInputEventArgs args)
		{
			if (GuiEvent != null)
				GuiEvent(sender, args);
		}

		public static event EventHandler<AgateInputEventArgs> GuiEvent;
	}
}
