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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets
{
	public class InputMap
	{
		Dictionary<KeyCode, GuiInput> mKeyMap = new Dictionary<KeyCode, GuiInput>();
		Dictionary<int, GuiInput> mJoystickButtonMap = new Dictionary<int, GuiInput>();

		public void Clear()
		{
			mKeyMap.Clear();
			mJoystickButtonMap.Clear();
		}

		public InputMap()
		{ }
		public static InputMap CreateDefaultInputMap()
		{
			InputMap result = new InputMap();

			result.AddKey(KeyCode.Up, GuiInput.Up);
			result.AddKey(KeyCode.Down, GuiInput.Down);
			result.AddKey(KeyCode.Left, GuiInput.Left);
			result.AddKey(KeyCode.Right, GuiInput.Right);
			result.AddKey(KeyCode.Enter, GuiInput.Accept);
			result.AddKey(KeyCode.Escape, GuiInput.Cancel);
			result.AddKey(KeyCode.PageUp, GuiInput.PageUp);
			result.AddKey(KeyCode.PageDown, GuiInput.PageDown);

			result.AddKey(KeyCode.NumPad8, GuiInput.Up);
			result.AddKey(KeyCode.NumPad4, GuiInput.Left);
			result.AddKey(KeyCode.NumPad6, GuiInput.Right);
			result.AddKey(KeyCode.NumPad2, GuiInput.Down);
			result.AddKey(KeyCode.NumPad0, GuiInput.Cancel);
			result.AddKey(KeyCode.NumPadPlus, GuiInput.Menu);
			result.AddKey(KeyCode.NumPadMinus, GuiInput.Switch);
			result.AddKey(KeyCode.NumPad9, GuiInput.PageUp);
			result.AddKey(KeyCode.NumPad3, GuiInput.PageDown);

			return result;
		}

		public void AddKey(KeyCode keyCode, GuiInput guiInput)
		{
			mKeyMap[keyCode] = guiInput;
		}

		public GuiInput MapKey(KeyCode keyCode)
		{
			if (mKeyMap.ContainsKey(keyCode) == false)
				return GuiInput.None;

			return mKeyMap[keyCode];
		}
	}
}
