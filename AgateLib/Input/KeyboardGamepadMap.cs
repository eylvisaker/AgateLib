//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AgateLib.Input
{
    /// <summary>
    /// Provides a mapping from keyboard keys to gamepad inputs.
    /// </summary>
    public class KeyboardGamepadMap : IDictionary<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>
    {
        private static KeyboardGamepadMap defaultValue;

        public static KeyboardGamepadMap Default
        {
            get
            {
                if (defaultValue == null)
                {
                    defaultValue = new KeyboardGamepadMap
                    {
                        { Microsoft.Xna.Framework.Input.Keys.W, KeyMapItem.ToLeftStick(PlayerIndex.One, Axis.Y, -1) },
                        { Microsoft.Xna.Framework.Input.Keys.A, KeyMapItem.ToLeftStick(PlayerIndex.One, Axis.X, -1) },
                        { Microsoft.Xna.Framework.Input.Keys.D, KeyMapItem.ToLeftStick(PlayerIndex.One, Axis.X, 1) },
                        { Microsoft.Xna.Framework.Input.Keys.S, KeyMapItem.ToLeftStick(PlayerIndex.One, Axis.Y, 1) },
                        { Microsoft.Xna.Framework.Input.Keys.F, KeyMapItem.ToButton(PlayerIndex.One,  Buttons.LeftStick) },

                        { Microsoft.Xna.Framework.Input.Keys.I, KeyMapItem.ToRightStick(PlayerIndex.One, Axis.Y, -1) },
                        { Microsoft.Xna.Framework.Input.Keys.J, KeyMapItem.ToRightStick(PlayerIndex.One, Axis.X, -1) },
                        { Microsoft.Xna.Framework.Input.Keys.L, KeyMapItem.ToRightStick(PlayerIndex.One, Axis.X, 1) },
                        { Microsoft.Xna.Framework.Input.Keys.K, KeyMapItem.ToRightStick(PlayerIndex.One, Axis.Y, 1) },
                        { Microsoft.Xna.Framework.Input.Keys.OemSemicolon, KeyMapItem.ToButton(PlayerIndex.One,  Buttons.RightStick) },

                        { Microsoft.Xna.Framework.Input.Keys.Up, KeyMapItem.ToButton(PlayerIndex.One, Buttons.DPadUp) },
                        { Microsoft.Xna.Framework.Input.Keys.Down, KeyMapItem.ToButton(PlayerIndex.One, Buttons.DPadDown) },
                        { Microsoft.Xna.Framework.Input.Keys.Left, KeyMapItem.ToButton(PlayerIndex.One, Buttons.DPadLeft) },
                        { Microsoft.Xna.Framework.Input.Keys.Right, KeyMapItem.ToButton(PlayerIndex.One, Buttons.DPadRight) },

                        { Microsoft.Xna.Framework.Input.Keys.Back, KeyMapItem.ToButton(PlayerIndex.One, Buttons.Back) },
                        { Microsoft.Xna.Framework.Input.Keys.Enter, KeyMapItem.ToButton(PlayerIndex.One, Buttons.Start) },

                        { Microsoft.Xna.Framework.Input.Keys.Space, KeyMapItem.ToButton(PlayerIndex.One, Buttons.A) },
                        { Microsoft.Xna.Framework.Input.Keys.LeftControl, KeyMapItem.ToButton(PlayerIndex.One, Buttons.X) },
                        { Microsoft.Xna.Framework.Input.Keys.LeftAlt, KeyMapItem.ToButton(PlayerIndex.One, Buttons.B) },
                        { Microsoft.Xna.Framework.Input.Keys.LeftShift, KeyMapItem.ToButton(PlayerIndex.One, Buttons.Y) },
                        { Microsoft.Xna.Framework.Input.Keys.Z, KeyMapItem.ToButton(PlayerIndex.One, Buttons.LeftShoulder) },
                        { Microsoft.Xna.Framework.Input.Keys.X, KeyMapItem.ToButton(PlayerIndex.One, Buttons.RightShoulder) },

                        { Microsoft.Xna.Framework.Input.Keys.Q, KeyMapItem.ToLeftTrigger(PlayerIndex.One) },
                        { Microsoft.Xna.Framework.Input.Keys.E, KeyMapItem.ToRightTrigger(PlayerIndex.One) },
                    };
                }

                return defaultValue;
            }
            set
            {
                Require.ArgumentNotNull(value, nameof(Default), "Default keymap must not be null.");

                defaultValue = value;
            }
        }

        private readonly Dictionary<Keys, KeyMapItem> keyMap = new Dictionary<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>();

        /// <summary>
        /// The number of items in the keymap.
        /// </summary>
        public int Count => keyMap.Count;

        /// <summary>
        /// Adds a new mapping.
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="keyMapItem"></param>
        public void Add(Microsoft.Xna.Framework.Input.Keys keyCode, KeyMapItem keyMapItem)
        {
            keyMap.Add(keyCode, keyMapItem);
        }

        /// <summary>
        /// Returns true if the key is mapped.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(Microsoft.Xna.Framework.Input.Keys key)
        {
            return keyMap.ContainsKey(key);
        }

        /// <summary>
        /// Removes a key mapping.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(Microsoft.Xna.Framework.Input.Keys key)
        {
            return keyMap.Remove(key);
        }

        /// <summary>
        /// Attemps to get a key mapping, returning true if successful.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(Microsoft.Xna.Framework.Input.Keys key, out KeyMapItem value)
        {
            return keyMap.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets or sets a key mapping.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyMapItem this[Microsoft.Xna.Framework.Input.Keys key]
        {
            get => keyMap[key];
            set => keyMap[key] = value;
        }

        /// <summary>
        /// Gets the collection of mapped keys.
        /// </summary>
        public ICollection<Microsoft.Xna.Framework.Input.Keys> Keys => keyMap.Keys;

        /// <summary>
        /// Get the collection of map values.
        /// </summary>
        public ICollection<KeyMapItem> Values => keyMap.Values;

        /// <summary>
        /// Enumerates the key-value pairs.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>> GetEnumerator()
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

        void ICollection<KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>>.Add(KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem> item)
        {
            keyMap.Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>>.Contains(KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem> item)
        {
            return ((IDictionary<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>)keyMap).Contains(item);
        }

        void ICollection<KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>>.CopyTo(KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>[] array, int arrayIndex)
        {
            ((IDictionary<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>)keyMap).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>>.Remove(KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem> item)
        {
            return ((IDictionary<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>)keyMap).Remove(item);
        }

        bool ICollection<KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>>.IsReadOnly => ((ICollection<KeyValuePair<Microsoft.Xna.Framework.Input.Keys, KeyMapItem>>)keyMap).IsReadOnly;
    }
}