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

		Dictionary<KeyCode, GuiInput> mKeyMap = new Dictionary<KeyCode, GuiInput>();
		Dictionary<int, GuiInput> mJoystickButtonMap = new Dictionary<int, GuiInput>();
		
		public InputMap()
		{ }

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

		public void Clear()
		{
			mKeyMap.Clear();
			mJoystickButtonMap.Clear();
		}
	}
}
