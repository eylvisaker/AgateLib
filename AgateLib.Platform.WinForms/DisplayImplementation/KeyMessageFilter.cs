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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms.Controls;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	public class KeyMessageFilter : IMessageFilter
	{
		private enum KeyMessage
		{
			WM_KEYFIRST = 0x100,
			WM_KEYDOWN = 0x100,
			WM_KEYUP = 0x101,
			WM_CHAR = 0x102,
			WM_SYSKEYDOWN = 0x0104,
			WM_SYSKEYUP = 0x0105,
			WM_SYSCHAR = 0x0106,
		}
		
		private bool altKey;
		private bool controlKey;
		private bool shiftKey;

		private bool shiftLeft;
		private bool shiftRight;

		public bool PreFilterMessage(ref Message m)
		{
			var messageType = (KeyMessage)m.Msg;

			switch (messageType)
			{
				case KeyMessage.WM_KEYDOWN:
				case KeyMessage.WM_SYSKEYDOWN:
					KeyDown(m);
					return true;

				case KeyMessage.WM_KEYUP:
				case KeyMessage.WM_SYSKEYUP:
					KeyUp(m);
					return true;
			}

			return false;
		}

		private void KeyDown(Message m)
		{
			var agateKeyCode = ConvertKeyCode(m);

			SetModifierKeyState(agateKeyCode, true);

			if (agateKeyCode == KeyCode.ShiftLeft) shiftLeft = true;
			if (agateKeyCode == KeyCode.ShiftRight) shiftRight = true;

			Input.QueueInputEvent(AgateInputEventArgs.KeyDown(agateKeyCode, KeyModifiers));
		}

		private void KeyUp(Message m)
		{
			var agateKeyCode = ConvertKeyCode(m);

			SetModifierKeyState(agateKeyCode, false);

			if (agateKeyCode == KeyCode.ShiftLeft || agateKeyCode == KeyCode.ShiftRight)
			{
				// It seems on windows if you press both shift keys, you won't get a key
				// up event until both are released. I haven't found any good documentation
				// on this so far, but it looks as if this is a legacy thing so as to not
				// confuse applications that lower case characters should be typed when there
				// is still a shift key being held.
				// The workaround for AgateLib is to send a key up for both shift keys if they
				// are down. At least this way the app won't think a shift key was left behind.

				if (shiftLeft)
					Input.QueueInputEvent(AgateInputEventArgs.KeyUp(KeyCode.ShiftLeft, KeyModifiers));
				if (shiftRight)
					Input.QueueInputEvent(AgateInputEventArgs.KeyUp(KeyCode.ShiftRight, KeyModifiers));

				shiftLeft = false;
				shiftRight = false;
			}
			else
			{
				Input.QueueInputEvent(AgateInputEventArgs.KeyUp(agateKeyCode, KeyModifiers));
			}
		}

		private static KeyCode ConvertKeyCode(Message m)
		{
			Keys key = (Keys)m.WParam;
			bool extended = ((long)m.LParam & 0x01000000) != 0;
			
			if (key == Keys.ControlKey)
				return extended ? KeyCode.ControlRight : KeyCode.ControlLeft;
			if (key == Keys.Menu)
				return extended ? KeyCode.AltRight : KeyCode.AltLeft;
			if (key == Keys.ShiftKey)
			{
				// Obtained from information in this answer:
				// http://stackoverflow.com/a/15967904/3856907

				var scanCode = ((long) m.LParam >> 16) & 0xFF;

				if (scanCode == 42)
					return KeyCode.ShiftLeft;
				if (scanCode == 54)
					return KeyCode.ShiftRight;
			}

			var agateKeyCode = FormUtil.TransformWinFormsKey(key);

			return agateKeyCode;
		}

		private KeyModifiers KeyModifiers => new KeyModifiers(altKey, controlKey, shiftKey);

		private void SetModifierKeyState(KeyCode agateKeyCode, bool value)
		{
			switch (agateKeyCode)
			{
				case KeyCode.Alt:
					altKey = value;
					break;

				case KeyCode.Shift:
				case KeyCode.ShiftLeft:
				case KeyCode.ShiftRight:
					shiftKey = value;
					break;

				case KeyCode.Control:
				case KeyCode.ControlLeft:
				case KeyCode.ControlRight:
					controlKey = value;
					break;
			}
		}
	}
}