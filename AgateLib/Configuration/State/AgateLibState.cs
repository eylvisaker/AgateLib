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
using AgateLib.ApplicationModels;

namespace AgateLib.Configuration.State
{
	class AgateLibState
	{
		public AgateLibState()
		{
			Input.FirstHandler = Console.Instance;
		}

		public AppModelState AppModel { get; private set; } = new AppModelState();
		public CoreState Core { get; private set; } = new CoreState();
		public ConsoleState Console { get; private set; } = new ConsoleState();

		public AudioState Audio { get; private set; } = new AudioState();
		public DisplayState Display { get; private set; } = new DisplayState();
		public InputState Input { get; private set; } = new InputState();

		public IOState IO { get; private set; } = new IOState();

		public bool Debug { get; internal set; }

		internal List<Scene> Scenes { get; private set; } = new List<Scene>();
	}
}
