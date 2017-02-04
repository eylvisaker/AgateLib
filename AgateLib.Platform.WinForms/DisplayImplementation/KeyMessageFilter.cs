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

		public bool PreFilterMessage(ref Message m)
		{
			var messageType = (KeyMessage)m.Msg;

			switch (messageType)
			{
				case KeyMessage.WM_KEYDOWN:
				case KeyMessage.WM_SYSKEYDOWN:
					KeyDown(m);
					break;

				case KeyMessage.WM_KEYUP:
				case KeyMessage.WM_SYSKEYUP:
					KeyUp(m);
					break;
			}

			return false;
		}

		private void KeyDown(Message m)
		{
			var agateKeyCode = ConvertKeyCode(m);

			SetModifierKeyState(agateKeyCode, true);

			Input.QueueInputEvent(AgateInputEventArgs.KeyDown(agateKeyCode, KeyModifiers));
		}

		private void KeyUp(Message m)
		{
			var agateKeyCode = ConvertKeyCode(m);

			SetModifierKeyState(agateKeyCode, false);

			Input.QueueInputEvent(AgateInputEventArgs.KeyUp(agateKeyCode, KeyModifiers));
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

		private KeyModifiers KeyModifiers
		{
			get { return new KeyModifiers(altKey, controlKey, shiftKey); }
		}

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