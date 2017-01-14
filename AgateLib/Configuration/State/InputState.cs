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
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.ImplementationBase;

namespace AgateLib.Configuration.State
{
	class InputState
	{
		internal InputImpl Impl;

		internal List<AgateInputEventArgs> EventQueue = new List<AgateInputEventArgs>();
		internal InputHandlerList Handlers = new InputHandlerList();
		internal List<Joystick> RawJoysticks = new List<Joystick>();

		internal bool LegacyIsMouseHidden;
		internal Point LegacyMousePosition;

		internal SimpleInputHandler Unhandled = new SimpleInputHandler();
		internal IInputHandler FirstHandler;

		internal IInputHandler MouseInputOwner;
		internal HashSet<KeyCode> KeysPressed = new HashSet<KeyCode>();

		internal Dictionary<IInputHandler, HandlerState> HandlerStates = new Dictionary<IInputHandler, HandlerState>();
	}
}
