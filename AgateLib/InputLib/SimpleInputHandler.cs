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
						ReleaseKey(id, false);
					}
					else
					{
						mWaitForKeyUp[intID] = false;
					}
				}
			}

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
			/// <param name="waitKeyUp">Boolean flag indicating whether or not
			/// keydown events should be suppressed until the key is physically released.</param>
			internal void ReleaseKey(KeyCode id, bool waitKeyUp)
			{
				mKeyState[(int)id] = 0;
				mWaitForKeyUp[(int)id] = waitKeyUp;

				Input.QueueInputEvent(AgateInputEventArgs.KeyUp(id, CurrentModifiers));
			}

			internal bool AnyKeyPressed => mKeyState.Any(x => x > 0);

			/// <summary>
			/// Resets all keys to being in the up state (not pushed).
			/// Does generate KeyUp events.
			/// 
			/// This also makes it so any keys which were depressed must be released
			/// before KeyDown events are raised again.
			/// </summary>
			internal void ReleaseAllKeys(bool waitForKeyUp)
			{
				for (int i = 0; i < mKeyState.Length; i++)
				{
					if (mKeyState[i] > 0)
						ReleaseKey((KeyCode)i, waitForKeyUp);
				}
			}
		}

		KeyState keys = new KeyState();
		
		public event EventHandler<AgateInputEventArgs> KeyDown;
		public event EventHandler<AgateInputEventArgs> KeyUp;
		public event EventHandler<AgateInputEventArgs> MouseDown;
		public event EventHandler<AgateInputEventArgs> MouseMove;
		public event EventHandler<AgateInputEventArgs> MouseUp;
		public event EventHandler<AgateInputEventArgs> MouseWheel;
		public event EventHandler<AgateInputEventArgs> MouseDoubleClick;
		public event EventHandler<AgateInputEventArgs> JoystickAxisChanged;
		public event EventHandler<AgateInputEventArgs> JoystickButton;
		public event EventHandler<AgateInputEventArgs> JoystickPovHat;

		public KeyState Keys => keys;

		public void Dispose()
		{
			Input.Handlers.Remove(this);
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
					MouseDown?.Invoke(this, args);
					break;

				case InputEventType.MouseMove:
					MouseMove?.Invoke(this, args);
					break;

				case InputEventType.MouseUp:
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

				case InputEventType.JoystickButton:
					JoystickButton?.Invoke(this, args);
					break;

				case InputEventType.JoystickPovHat:
					JoystickPovHat?.Invoke(this, args);
					break;

				default:
					args.Handled = false;
					return;
			}
		}

		bool IInputHandler.ForwardUnhandledEvents => false;

		void IInputHandler.ProcessEvent(AgateInputEventArgs args)
		{
			ProcessEvent(args);
		}
	}
}
