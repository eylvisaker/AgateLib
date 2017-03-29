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
using System.Collections;
using System.Collections.Generic;
using AgateLib.Quality;

namespace AgateLib.InputLib.GamepadModel
{
	/// <summary>
	/// Provides a mapping from keyboard keys to gamepad inputs.
	/// </summary>
	public class KeyboardGamepadMap : IDictionary<KeyCode, KeyMapItem>
	{
		private const string SettingsKey = "AgateLib.DefaultKeyGamepadMap";

		private static KeyboardGamepadMap defaultValue
		{
			get { return AgateApp.State.Input.DefaultKeyboardGamepadMap; }
			set { AgateApp.State.Input.DefaultKeyboardGamepadMap = value; }
		}

		public static KeyboardGamepadMap Default
		{
			get
			{
				if (defaultValue == null)
				{
					defaultValue = AgateApp.Settings.GetOrCreate<KeyboardGamepadMap>(SettingsKey, () => new KeyboardGamepadMap
					{
						{ KeyCode.W, KeyMapItem.ToLeftStick(0, Axis.Y, -1) },
						{ KeyCode.A, KeyMapItem.ToLeftStick(0, Axis.X, -1) },
						{ KeyCode.D, KeyMapItem.ToLeftStick(0, Axis.X, 1) },
						{ KeyCode.S, KeyMapItem.ToLeftStick(0, Axis.Y, 1) },
						{ KeyCode.F, KeyMapItem.ToButton(0,  GamepadButton.LeftStickButton) },

						{ KeyCode.I, KeyMapItem.ToRightStick(0, Axis.Y, -1) },
						{ KeyCode.J, KeyMapItem.ToRightStick(0, Axis.X, -1) },
						{ KeyCode.L, KeyMapItem.ToRightStick(0, Axis.X, 1) },
						{ KeyCode.K, KeyMapItem.ToRightStick(0, Axis.Y, 1) },
						{ KeyCode.Semicolon, KeyMapItem.ToButton(0,  GamepadButton.RightStickButton) },

						{ KeyCode.Up, KeyMapItem.ToDirectionPad(0, Axis.Y, -1) },
						{ KeyCode.Down, KeyMapItem.ToDirectionPad(0, Axis.Y, 1) },
						{ KeyCode.Left, KeyMapItem.ToDirectionPad(0, Axis.X, -1) },
						{ KeyCode.Right, KeyMapItem.ToDirectionPad(0, Axis.X, 1) },

						{ KeyCode.BackSpace, KeyMapItem.ToButton(0, GamepadButton.Back) },
						{ KeyCode.Enter, KeyMapItem.ToButton(0, GamepadButton.Start) },

						{ KeyCode.Space, KeyMapItem.ToButton(0, GamepadButton.A) },
						{ KeyCode.ControlLeft, KeyMapItem.ToButton(0, GamepadButton.X) },
						{ KeyCode.AltLeft, KeyMapItem.ToButton(0, GamepadButton.B) },
						{ KeyCode.ShiftLeft, KeyMapItem.ToButton(0, GamepadButton.Y) },
						{ KeyCode.Z, KeyMapItem.ToButton(0, GamepadButton.LeftBumper) },
						{ KeyCode.X, KeyMapItem.ToButton(0, GamepadButton.RightBumper) },

						{ KeyCode.Q, KeyMapItem.ToLeftTrigger(0) },
						{ KeyCode.E, KeyMapItem.ToRightTrigger(0) },
					});
				}

				return defaultValue;
			}
			set
			{
				Require.ArgumentNotNull(value, nameof(Default), "Default keymap must not be null.");

				defaultValue = value;
				AgateApp.Settings.Set(SettingsKey, value);
			}
		}

		private readonly Dictionary<KeyCode, KeyMapItem> keyMap = new Dictionary<KeyCode, KeyMapItem>();

		/// <summary>
		/// The number of items in the keymap.
		/// </summary>
		public int Count => keyMap.Count;

		/// <summary>
		/// Adds a new mapping.
		/// </summary>
		/// <param name="keyCode"></param>
		/// <param name="keyMapItem"></param>
		public void Add(KeyCode keyCode, KeyMapItem keyMapItem)
		{
			keyMap.Add(keyCode, keyMapItem);
		}

		/// <summary>
		/// Returns true if the key is mapped.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(KeyCode key)
		{
			return keyMap.ContainsKey(key);
		}

		/// <summary>
		/// Removes a key mapping.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(KeyCode key)
		{
			return keyMap.Remove(key);
		}

		/// <summary>
		/// Attemps to get a key mapping, returning true if successful.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(KeyCode key, out KeyMapItem value)
		{
			return keyMap.TryGetValue(key, out value);
		}

		/// <summary>
		/// Gets or sets a key mapping.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public KeyMapItem this[KeyCode key]
		{
			get { return keyMap[key]; }
			set { keyMap[key] = value; }
		}

		/// <summary>
		/// Gets the collection of mapped keys.
		/// </summary>
		public ICollection<KeyCode> Keys => keyMap.Keys;

		/// <summary>
		/// Get the collection of map values.
		/// </summary>
		public ICollection<KeyMapItem> Values => keyMap.Values;

		/// <summary>
		/// Enumerates the key-value pairs.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<KeyValuePair<KeyCode, KeyMapItem>> GetEnumerator()
		{
			return keyMap.GetEnumerator();
		}

		/// <summary>
		/// Clears the key map.
		/// </summary>
		public void Clear()
		{
			keyMap.Clear();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)keyMap).GetEnumerator();
		}

		void ICollection<KeyValuePair<KeyCode, KeyMapItem>>.Add(KeyValuePair<KeyCode, KeyMapItem> item)
		{
			keyMap.Add(item.Key, item.Value);
		}

		bool ICollection<KeyValuePair<KeyCode, KeyMapItem>>.Contains(KeyValuePair<KeyCode, KeyMapItem> item)
		{
			return ((IDictionary<KeyCode, KeyMapItem>)keyMap).Contains(item);
		}

		void ICollection<KeyValuePair<KeyCode, KeyMapItem>>.CopyTo(KeyValuePair<KeyCode, KeyMapItem>[] array, int arrayIndex)
		{
			((IDictionary<KeyCode, KeyMapItem>)keyMap).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<KeyCode, KeyMapItem>>.Remove(KeyValuePair<KeyCode, KeyMapItem> item)
		{
			return ((IDictionary<KeyCode, KeyMapItem>)keyMap).Remove(item);
		}

		bool ICollection<KeyValuePair<KeyCode, KeyMapItem>>.IsReadOnly => ((ICollection<KeyValuePair<KeyCode, KeyMapItem>>)keyMap).IsReadOnly;
	}
}