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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using AgateLib.Geometry;

namespace AgateLib.InputLib
{
	/// <summary>
	/// Static class which represents Keyboard input.
	/// </summary>
	[CLSCompliant(true)]
	public static class Keyboard
	{
		static KeyState mKeyState = new KeyState();

		static Keyboard()
		{
			DisplayLib.Display.DisposeDisplay += new AgateLib.DisplayLib.Display.DisposeDisplayHandler(Display_DisposeDisplay);
		}

		static void Display_DisposeDisplay()
		{
			ClearEvents();
		}


		/// <summary>
		/// Class which represents the state of all keys on the keyboard.
		/// </summary>
		[CLSCompliant(true)]
		public class KeyState
		{
			private static int[] mKeyState;
			private static bool[] mWaitForKeyUp;

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

						//System.Diagnostics.Debug.Print("Set key {0} to {1}, repeat count {2}.",
						//    id, value, mKeyState[(int)id] - 1);

						Keyboard.OnKeyDown(id,
								new KeyModifiers(this[KeyCode.Alt], this[KeyCode.Control], this[KeyCode.Shift]),
								mKeyState[(int)id] - 1);
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
				//System.Diagnostics.Debug.Print("Set key {0} to {1}.", id, false);

				Keyboard.OnKeyUp(id,
					  new KeyModifiers(this[KeyCode.Alt], this[KeyCode.Control], this[KeyCode.Shift]));
			}
			internal bool AnyKeyPressed
			{
				get
				{
					for (int i = 0; i < mKeyState.Length; i++)
						if (mKeyState[i] > 0)
							return true;

					return false;
				}
			}

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

		/// <summary>
		/// Gets an object representing the state of all keys on the keyboard.
		/// </summary>
		public static KeyState Keys
		{
			get { return mKeyState; }
		}

		/// <summary>
		/// Resets all keys to being in the up state (not pushed).
		/// Does generate KeyUp events.
		/// 
		/// This also makes it so any keys which were depressed must be released
		/// before KeyDown events are raised again.
		/// </summary>
		public static void ReleaseAllKeys()
		{
			mKeyState.ReleaseAllKeys(true);
		}
		/// <summary>
		/// Resets all keys to being in the up state (not pushed).
		/// Does generate KeyUp events.
		/// <para>
		/// This can also make it so any keys which were depressed must be released
		/// before KeyDown events are raised again.
		/// </para>
		/// </summary>
		/// <param name="waitForKeyUp">If true, then keys currently depressed will 
		/// not generate KeyDown events until they are released.</param>
		public static void ReleaseAllKeys(bool waitForKeyUp)
		{
			mKeyState.ReleaseAllKeys(waitForKeyUp);
		}

		/// <summary>
		/// Resets a particular key to being in the up state (not pushed).  
		/// Generates a KeyUp event for that key.
		/// <para>
		/// This also makes it so that the key must be physically depressed by the user
		/// before it will register a KeyDown event again.  If it is already depressed,
		/// it must be released first.
		/// </para>
		/// </summary>
		/// <param name="key"></param>
		public static void ReleaseKey(KeyCode key)
		{
			mKeyState.ReleaseKey(key, true);
		}
		/// <summary>
		/// Resets a particular key to being in the up state (not pushed).  
		/// Generates a KeyUp event for that key.
		/// </summary>
		/// <param name="key">The key to be released.</param>
		/// <param name="waitForKeyUp">If true, then keys currently depressed will 
		/// not generate KeyDown events until they are released.</param>
		public static void ReleaseKey(KeyCode key, bool waitForKeyUp)
		{
			mKeyState.ReleaseKey(key, waitForKeyUp);
		}

		/// <summary>
		/// Checks to see if the user pressed the "Any" key.
		/// </summary>
		public static bool AnyKeyPressed
		{
			get
			{
				return Keys.AnyKeyPressed;
			}
		}
		private static void OnKeyDown(KeyCode id, KeyModifiers mods, int repeatCount)
		{
			if (KeyDown != null)
				KeyDown(new InputEventArgs(id, mods, repeatCount));
		}
		private static void OnKeyUp(KeyCode id, KeyModifiers mods)
		{
			if (KeyUp != null)
				KeyUp(new InputEventArgs(id, mods));
		}

		/// <summary>
		/// Creates a string from the specified KeyCode and KeyModifiers.
		/// Unfortunately this is tied to the US English keyboard, so it needs a better solution.
		/// </summary>
		/// <param name="keyID"></param>
		/// <param name="mods"></param>
		/// <returns></returns>
		public static string GetKeyString(KeyCode keyID, KeyModifiers mods)
		{
			if ((int)keyID >= 'A' && (int)keyID <= 'Z')
			{
				char result;

				if (mods.Shift)
					result = (char)keyID;
				else
					result = (char)((int)keyID + 'a' - 'A');

				return result.ToString();
			}


			switch (keyID)
			{
				case KeyCode.Tab:
					return "\t";

				case KeyCode.Return:
					//case KeyCode.Enter:
					return "\n";

				case KeyCode.Space:
					return " ";

				// I'd love a better way of doing this:
				// likely, this is not very friendly to non US keyboard layouts.
				case KeyCode.D0:
					if (mods.Shift)
						return ")";
					else
						return "0";

				case KeyCode.NumPad0:
					return "0";

				case KeyCode.D1:
					if (mods.Shift)
						return "!";
					else return "1";


				case KeyCode.NumPad1:
					return "1";

				case KeyCode.D2:
					if (mods.Shift)
						return "@";
					else return "2";

				case KeyCode.NumPad2:
					return "2";

				case KeyCode.D3:
					if (mods.Shift)
						return "#";
					else return "3";

				case KeyCode.NumPad3:
					return "3";

				case KeyCode.D4:
					if (mods.Shift)
						return "$";
					else return "4";

				case KeyCode.NumPad4:
					return "4";

				case KeyCode.D5:
					if (mods.Shift)
						return "%";
					else return "5";

				case KeyCode.NumPad5:
					return "5";

				case KeyCode.D6:
					if (mods.Shift)
						return "^";
					else return "6";

				case KeyCode.NumPad6:
					return "6";

				case KeyCode.D7:
					if (mods.Shift)
						return "&";
					else return "7";

				case KeyCode.NumPad7:
					return "7";

				case KeyCode.D8:
					if (mods.Shift)
						return "*";
					else return "8";


				case KeyCode.NumPad8:
					return "8";

				case KeyCode.D9:
					if (mods.Shift)
						return "(";
					else return "9";

				case KeyCode.NumPad9:
					return "9";

				case KeyCode.NumPadMinus:
					return "-";
				case KeyCode.NumPadMultiply:
					return "*";
				case KeyCode.NumPadPeriod:
					return ".";
				case KeyCode.NumPadPlus:
					return "+";
				case KeyCode.NumPadSlash:
					return "/";

				case KeyCode.Semicolon:
					if (mods.Shift)
						return ":";
					else
						return ";";

				case KeyCode.Plus:
					if (mods.Shift)
						return "+";
					else
						return "=";

				case KeyCode.Comma:
					if (mods.Shift)
						return "<";
					else
						return ",";

				case KeyCode.Minus:
					if (mods.Shift)
						return "_";
					else
						return "-";

				case KeyCode.Period:
					if (mods.Shift)
						return ">";
					else
						return ".";

				case KeyCode.Slash:
					if (mods.Shift)
						return "?";
					else
						return "/";

				case KeyCode.Tilde:
					if (mods.Shift)
						return "~";
					else
						return "`";

				case KeyCode.OpenBracket:
					if (mods.Shift)
						return "{";
					else
						return "[";

				case KeyCode.BackSlash:
					if (mods.Shift)
						return "|";
					else
						return @"\";

				case KeyCode.CloseBracket:
					if (mods.Shift)
						return "}";
					else
						return "]";

				case KeyCode.Quotes:
					if (mods.Shift)
						return "\"";
					else
						return "'";

			}

			return "";
		}

		/// <summary>
		/// Event which occurs when a key is pressed.
		/// </summary>
		public static event InputEventHandler KeyDown;
		/// <summary>
		/// Event which occurs when a key is released.
		/// </summary>
		public static event InputEventHandler KeyUp;


		private static void ClearEvents()
		{
			KeyDown = null;
			KeyUp = null;
		}
	}
}
