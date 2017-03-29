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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.InputLib
{
	/// <summary>
	/// Provides input handling of various events.
	/// </summary>
	public class SimpleInputHandler : IInputHandler, IDisposable
	{
		/// <summary>
		/// Class which represents the state of all keys on the keyboard.
		/// </summary>
		public class KeyState
		{
			private int[] mKeyState;
			private bool[] mWaitForKeyUp;

			internal KeyState()
			{
				mKeyState = new int[256];
				mWaitForKeyUp = new bool[256];
			}

			/// <summary>
			/// Gets or sets the state of the given key.
			/// </summary>
			/// <param name="id"></param>
			/// <returns></returns>
			public bool this[KeyCode id]
			{
				get
				{
					if (mKeyState[(int)id] > 0)
						return true;
					else
						return false;
				}
				set
				{
					int intID = (int)id;

					if (value == true)
					{
						if (mKeyState[intID] == 0 && mWaitForKeyUp[intID])
							return;

						mKeyState[intID]++;
						mWaitForKeyUp[intID] = true;

						//System.Diagnostics.Debug.WriteLine("Set key {0} to {1}, repeat count {2}.",
						//    id, value, mKeyState[(int)id] - 1);
					}
					// value is false here:
					else if (mKeyState[(int)id] > 0)
					{
						Release(id, false);
					}
					else
					{
						mWaitForKeyUp[intID] = false;
					}
				}
			}

			/// <summary>
			/// Returns the list of keys which are currently
			/// pressed.
			/// </summary>
			public IEnumerable<KeyCode> PressedKeys
			{
				get
				{
					for (int i = 0; i < mKeyState.Length; i++)
						if (mKeyState[i] > 0)
							yield return (KeyCode)i;
				}
			}

			/// <summary>
			/// Returns true if any key is pressed.
			/// </summary>
			public bool Any => mKeyState.Any(x => x > 0);

			public KeyModifiers CurrentModifiers
			{
				get { return new KeyModifiers(this[KeyCode.Alt], this[KeyCode.Control], this[KeyCode.Shift]); }
			}

			/// <summary>
			/// Clears the key-down status of a key, and generates a KeyUp event.  
			/// If waitKeyUp is true, the key is marked so that KeydDown events will not be generated until 
			/// it has been physically released by the user.
			/// </summary>
			/// <param name="id">KeyCode identifier of key to release.</param>
			/// <param name="waitForKeyUp">Boolean flag indicating whether or not
			/// keydown events should be suppressed until the key is physically released.</param>
			public void Release(KeyCode id, bool waitForKeyUp = true)
			{
				mKeyState[(int)id] = 0;
				mWaitForKeyUp[(int)id] = waitForKeyUp;
			}

			internal bool AnyKeyPressed => mKeyState.Any(x => x > 0);

			/// <summary>
			/// Resets all keys to being in the up state (not pushed).
			/// Does generate KeyUp events.
			/// 
			/// This also makes it so any keys which are still physically depressed 
			/// must be released before KeyDown events are raised again.
			/// </summary>
			/// <param name="waitKeyUp">Boolean flag indicating whether or not
			/// keydown events should be suppressed until the key is physically released.</param>
			public void ReleaseAll(bool waitForKeyUp = true)
			{
				for (int i = 0; i < mKeyState.Length; i++)
				{
					if (mKeyState[i] > 0)
						Release((KeyCode)i, waitForKeyUp);
				}
			}
		}

		KeyState keys = new KeyState();
		bool[] mouseButtons;

		public SimpleInputHandler()
		{
			var enumValues = (MouseButton[])Enum.GetValues(typeof(MouseButton));

			mouseButtons = new bool[enumValues.Select(x => (int)x).Max()];
		}

		public event EventHandler<AgateInputEventArgs> KeyDown;
		public event EventHandler<AgateInputEventArgs> KeyUp;
		public event EventHandler<AgateInputEventArgs> MouseDown;
		public event EventHandler<AgateInputEventArgs> MouseMove;
		public event EventHandler<AgateInputEventArgs> MouseUp;
		public event EventHandler<AgateInputEventArgs> MouseWheel;
		public event EventHandler<AgateInputEventArgs> MouseDoubleClick;
		public event EventHandler<AgateInputEventArgs> JoystickAxisChanged;
		public event EventHandler<AgateInputEventArgs> JoystickButtonPressed;
		public event EventHandler<AgateInputEventArgs> JoystickButtonReleased;
		public event EventHandler<AgateInputEventArgs> JoystickHatChanged;

		/// <summary>
		/// Tracks the state of keys (true for depressed, false otherwise).
		/// This tracks key state only via keydown/keyup events that are handled
		/// by this input handler. 
		/// </summary>
		public KeyState Keys => keys;

		public void Dispose()
		{
			Input.Handlers.Remove(this);
		}

		public bool IsMouseButtonDown(MouseButton button)
		{
			return mouseButtons[(int)button];
		}

		private void ProcessEvent(AgateInputEventArgs args)
		{
			args.Handled = true;

			switch (args.InputEventType)
			{
				case InputEventType.KeyDown:
					keys[args.KeyCode] = true;
					KeyDown?.Invoke(this, args);
					break;

				case InputEventType.KeyUp:
					keys[args.KeyCode] = false;
					KeyUp?.Invoke(this, args);
					break;

				case InputEventType.MouseDown:
					SetMouseButton(args.MouseButton, true);
					MouseDown?.Invoke(this, args);
					break;

				case InputEventType.MouseMove:
					MouseMove?.Invoke(this, args);
					break;

				case InputEventType.MouseUp:
					SetMouseButton(args.MouseButton, false);
					MouseUp?.Invoke(this, args);
					break;

				case InputEventType.MouseWheel:
					MouseWheel?.Invoke(this, args);
					break;

				case InputEventType.MouseDoubleClick:
					MouseDoubleClick?.Invoke(this, args);
					break;

				case InputEventType.JoystickAxisChanged:
					JoystickAxisChanged?.Invoke(this, args);
					break;

				case InputEventType.JoystickButtonPressed:
					JoystickButtonPressed?.Invoke(this, args);
					break;

				case InputEventType.JoystickButtonReleased:
					JoystickButtonReleased?.Invoke(this, args);
					break;

				case InputEventType.JoystickHatChanged:
					JoystickHatChanged?.Invoke(this, args);
					break;

				default:
					args.Handled = false;
					return;
			}
		}

		private void SetMouseButton(MouseButton button, bool value)
		{
			mouseButtons[(int) button] = value;
		}

		bool IInputHandler.ForwardUnhandledEvents => false;

		void IInputHandler.ProcessEvent(AgateInputEventArgs args)
		{
			ProcessEvent(args);
		}
	}
}
