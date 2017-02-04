using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AgateLib.InputLib;
using AgateLib.Platform.WinForms.Controls;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	public class KeyMessageFilter : IMessageFilter
	{
		private enum KeyMessages
		{
			WM_KEYFIRST = 0x100,
			WM_KEYDOWN = 0x100,
			WM_KEYUP = 0x101,
			WM_CHAR = 0x102,
			WM_SYSKEYDOWN = 0x0104,
			WM_SYSKEYUP = 0x0105,
			WM_SYSCHAR = 0x0106,
		}

		[DllImport("user32.dll")]
		private static extern IntPtr GetParent(IntPtr hwnd);

		// We check the events agains this control to only handle
		// key event that happend inside this control.
		Control _control;

		private bool altKey;
		private bool controlKey;
		private bool shiftKey;

		public KeyMessageFilter()
		{ }

		public KeyMessageFilter(Control c)
		{
			_control = c;
		}

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == (int)KeyMessages.WM_KEYDOWN)
			{
				Keys key = (Keys)m.WParam;
				var agateKeyCode = FormUtil.TransformWinFormsKey(key);

				SetModifierKeyState(agateKeyCode, true);

				Input.QueueInputEvent(AgateInputEventArgs.KeyDown(agateKeyCode, KeyModifiers));
			}
			else if (m.Msg == (int) KeyMessages.WM_KEYUP)
			{
				Keys key = (Keys)m.WParam;
				var agateKeyCode = FormUtil.TransformWinFormsKey(key);

				SetModifierKeyState(agateKeyCode, false);

				Input.QueueInputEvent(AgateInputEventArgs.KeyUp(agateKeyCode, KeyModifiers));
			}

			return false;
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

		private bool ValidateControl(ref Message m)
		{
			if (_control != null)
			{
				IntPtr hwnd = m.HWnd;
				IntPtr handle = _control.Handle;
				while (hwnd != IntPtr.Zero && handle != hwnd)
				{
					hwnd = GetParent(hwnd);
				}
				if (hwnd == IntPtr.Zero) // Didn't found the window. We are not interested in the event.
					return false;
			}

			return true;
		}
	}
}