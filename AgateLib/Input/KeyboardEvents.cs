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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Input
{
    /// <summary>
    /// Provides text input and event functionality for keyboards.
    /// Generates garbage, so use with care.
    /// </summary>
    public class KeyboardEvents
    {
        private List<Keys> pressedKeys = new List<Keys>();
        private KeyModifiers modifiers = new KeyModifiers();

        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;
        public event EventHandler<KeyPressEventArgs> KeyPress;

        private long lastKeyPress;
        private long currentTime;
        private Keys? repeatKey;
        private bool repeating;
        private Dictionary<Keys, string> lowerCase = new Dictionary<Keys, string>();
        private Dictionary<Keys, string> upperCase = new Dictionary<Keys, string>();

        public KeyboardEvents()
        {
            AddRange(upperCase, Keys.A, Keys.Z, key => key.ToString());
            AddRange(lowerCase, Keys.A, Keys.Z, key => key.ToString().ToLower());
            AddRange(lowerCase, Keys.D0, Keys.D9, key => (key - Keys.D0).ToString());
            AddRange(lowerCase, Keys.NumPad0, Keys.NumPad9, key => (key - Keys.NumPad0).ToString());

            AddSameCase(Keys.Space, " ");
            AddSameCase(Keys.Enter, "\n");
            AddSameCase(Keys.Tab, "\t");

            upperCase[Keys.D1] = "!";
            upperCase[Keys.D2] = "@";
            upperCase[Keys.D3] = "#";
            upperCase[Keys.D4] = "$";
            upperCase[Keys.D5] = "%";
            upperCase[Keys.D6] = "^";
            upperCase[Keys.D7] = "&";
            upperCase[Keys.D8] = "*";
            upperCase[Keys.D9] = "(";
            upperCase[Keys.D0] = ")";

            AddKey(Keys.OemMinus, "-", "_");
            AddKey(Keys.OemPlus, "=", "+");
            AddKey(Keys.OemSemicolon, ";", ":");
            AddKey(Keys.OemPeriod, ".", ">");
            AddKey(Keys.OemComma, ",", "<");
            AddKey(Keys.OemQuestion, "/", "?");
            AddKey(Keys.OemQuotes, "'", "\"");
            AddKey(Keys.OemTilde, "`", "~");
        }

        private void AddKey(Keys key, string lower, string upper)
        {
            lowerCase[key] = lower;
            upperCase[key] = upper;
        }

        private void AddSameCase(Keys key, string stringValue)
        {
            lowerCase[key] = stringValue;
            upperCase[key] = stringValue;
        }

        private void AddRange(Dictionary<Keys, string> dest, Keys start, Keys end,
            Func<Keys, string> stringCoercer)
        {
            for (var i = start; i <= end; i++)
            {
                dest.Add(i, stringCoercer(i));
            }
        }

        /// <summary>
        /// Number of milliseconds before first key repeat.
        /// </summary>
        public int InitialRepeatTime { get; set; } = 500;

        /// <summary>
        /// Number of milliseconds between repeating keystrokes.
        /// </summary>
        public int RepeatTime { get; set; } = 25;

        public void Update(GameTime time)
        {
            KeyboardState state = Keyboard.GetState();
            currentTime = (long)time.TotalGameTime.TotalMilliseconds;

            var newKeys = state.GetPressedKeys();

            foreach (var key in newKeys)
            {
                if (pressedKeys.Contains(key))
                    continue;

                OnKeyDown(key);

                if (CanRepeat(key))
                {
                    repeatKey = key;
                    repeating = false;
                }
            }

            foreach (var key in pressedKeys)
            {
                if (newKeys.Contains(key))
                    continue;

                OnKeyUp(key);

                if (repeatKey == key)
                {
                    repeatKey = null;
                    repeating = false;
                }
            }

            pressedKeys.Clear();
            pressedKeys.AddRange(newKeys);

            if (repeatKey != null)
            {
                var dt = currentTime - lastKeyPress;

                if (repeating && dt > RepeatTime)
                {
                    OnKeyPress(repeatKey.Value);
                }
                else if (!repeating && dt > InitialRepeatTime)
                {
                    OnKeyPress(repeatKey.Value);
                    repeating = true;
                }
            }
        }

        private void OnKeyUp(Keys key)
        {
            modifiers.KeyUp(key);

            KeyUp?.Invoke(this, new KeyEventArgs(key));
        }

        private void OnKeyDown(Keys key)
        {
            modifiers.KeyDown(key);

            KeyDown?.Invoke(this, new KeyEventArgs(key));

            OnKeyPress(key);
        }

        private void OnKeyPress(Keys key)
        {
            if (KeyModifiers.ModifierKeys.Contains(key))
                return;

            var str = KeyString(key);

            lastKeyPress = currentTime;
            KeyPress?.Invoke(this, new KeyPressEventArgs(key, str, modifiers));
        }

        private bool CanRepeat(Keys key)
        {
            return !KeyModifiers.ModifierKeys.Contains(key);
        }

        private string KeyString(Keys key)
        {
            var table = modifiers.Shift ? upperCase : lowerCase;

            if (table.TryGetValue(key, out var result))
                return result;

            return null;
        }
    }

    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(Keys key)
        {
            Key = key;
        }

        public Keys Key { get; }
    }

    public class KeyPressEventArgs : EventArgs
    {
        public KeyPressEventArgs(Keys key, string keyString, IKeyModifiers modifiers)
        {
            Key = key;
            KeyString = keyString;
            Modifiers = modifiers;
        }

        public Keys Key { get; }
        public string KeyString { get; }
        public IKeyModifiers Modifiers { get; }
    }

    public interface IKeyModifiers
    {
        bool Shift { get; }
        bool Control { get; }
        bool Alt { get; }
    }

    public class KeyModifiers : IKeyModifiers
    {
        public static readonly IKeyModifiers NoModifierKeyPressed
            = new KeyModifiers();

        public static readonly IReadOnlyList<Keys> ModifierKeys =
            new List<Keys>
            {
                Keys.LeftShift, Keys.RightShift,
                Keys.LeftControl, Keys.RightControl,
                Keys.LeftAlt, Keys.RightAlt,
            };

        public bool Shift => LeftShift || RightShift;
        public bool Control => LeftControl || RightControl;
        public bool Alt => LeftAlt || RightAlt;

        public bool LeftShift { get; set; }
        public bool RightShift { get; set; }

        public bool LeftControl { get; set; }
        public bool RightControl { get; set; }

        public bool LeftAlt { get; set; }
        public bool RightAlt { get; set; }

        public void KeyDown(Keys key)
        {
            KeyPress(key, true);
        }

        public void KeyUp(Keys key)
        {
            KeyPress(key, false);
        }

        private void KeyPress(Keys key, bool value)
        {
            switch (key)
            {
                case Keys.LeftShift: LeftShift = value; break;
                case Keys.RightShift: RightShift = value; break;

                case Keys.LeftControl: LeftControl = value; break;
                case Keys.RightControl: RightControl = value; break;

                case Keys.LeftAlt: LeftAlt = value; break;
                case Keys.RightAlt: RightAlt = value; break;
            }
        }
    }
}
