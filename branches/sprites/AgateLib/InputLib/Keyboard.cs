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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
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
    /// An enumeration of all possible key values.
    /// These values (mostly) correspond to the values used in System.Windows.Forms.Keys
    /// in .NET 2.0.
    /// </summary>
    [CLSCompliant(true)]
    public enum KeyCode
    {
        /// <summary>
        /// No key pressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// Backspace key.
        /// </summary>
        BackSpace = 8,
        /// <summary>
        /// Tab key.
        /// </summary>
        Tab = 9,
        /// <summary>
        /// Dunno what this is.
        /// </summary>
        LineFeed = 10,

        /// <summary>
        /// Dunno what this is.
        /// </summary>
        Clear = 12,

        /// <summary>
        /// Return key.
        /// </summary>
        Return = 13,
        /// <summary>
        /// Enter key.
        /// </summary>
        Enter = 13,
        /// <summary>
        /// Pause Key.
        /// </summary>
        Pause = 19,
        /// <summary>
        /// Caps Lock key.
        /// </summary>
        CapsLock = 20,
        /// <summary>
        /// Escape key.
        /// </summary>
        Escape = 27,
        /// <summary>
        /// Space bar.
        /// </summary>
        Space = 32,
        /// <summary>
        /// PageUp key.
        /// </summary>
        PageUp = 33,
        /// <summary>
        /// PageDown key.
        /// </summary>
        PageDown = 34,
        /// <summary>
        /// End key.
        /// </summary>
        End = 35,
        /// <summary>
        /// Home key.
        /// </summary>
        Home = 36,
        /// <summary>
        /// Left Arrow key.
        /// </summary>
        Left = 37,
        /// <summary>
        /// Up Arrow key.
        /// </summary>
        Up = 38,
        /// <summary>
        /// Right arrow key.
        /// </summary>
        Right = 39,
        /// <summary>
        /// Down arrow key.
        /// </summary>
        Down = 40,
        /// <summary>
        /// ???
        /// </summary>
        Select = 41,
        /// <summary>
        /// ???
        /// </summary>
        Print = 42,
        /// <summary>
        /// ???
        /// </summary>
        Execute = 43,
        /// <summary>
        /// PrintScreen key
        /// </summary>
        PrintScreen = 44,
        /// <summary>
        /// Insert key
        /// </summary>
        Insert = 45,
        /// <summary>
        /// Delete key
        /// </summary>
        Delete = 46,
        /// <summary>
        /// ???
        /// </summary>
        Help = 47,

        /// <summary>
        /// Zero key on main keyboard.
        /// </summary>
        D0 = 48,
        /// <summary>
        /// One key on main keyboard
        /// </summary>
        D1 = 49,
        /// <summary>
        /// Two key on main keyboard
        /// </summary>
        D2 = 50,
        /// <summary>
        /// Three key on main keyboard
        /// </summary>
        D3 = 51,
        /// <summary>
        /// Four key on main keyboard
        /// </summary>
        D4 = 52,
        /// <summary>
        /// Five key on main keyboard
        /// </summary>
        D5 = 53,
        /// <summary>
        /// Six key on main keyboard
        /// </summary>
        D6 = 54,
        /// <summary>
        /// Seven key on main keyboard
        /// </summary>
        D7 = 55,
        /// <summary>
        /// Eight key on main keyboard
        /// </summary>
        D8 = 56,
        /// <summary>
        /// Nine key on main keyboard
        /// </summary>
        D9 = 57,

        /// <summary>
        /// A key.
        /// </summary>
        A = 65,
        /// <summary>
        /// B key.
        /// </summary>
        B = 66,
        /// <summary>
        /// C key.
        /// </summary>
        C = 67,
        /// <summary>
        /// D key.
        /// </summary>
        D = 68,
        /// <summary>
        /// E key.
        /// </summary>
        E = 69,
        /// <summary>
        /// F key.
        /// </summary>
        F = 70,
        /// <summary>
        /// G key.
        /// </summary>
        G = 71,
        /// <summary>
        /// H key.
        /// </summary>
        H = 72,
        /// <summary>
        /// I key.
        /// </summary>
        I = 73,
        /// <summary>
        /// J key.
        /// </summary>
        J = 74,
        /// <summary>
        /// K key.
        /// </summary>
        K = 75,
        /// <summary>
        /// L key.
        /// </summary>
        L = 76,
        /// <summary>
        /// M key.
        /// </summary>
        M = 77,
        /// <summary>
        /// N key.
        /// </summary>
        N = 78,
        /// <summary>
        /// O key.
        /// </summary>
        O = 79,
        /// <summary>
        /// P key.
        /// </summary>
        P = 80,
        /// <summary>
        /// Q key.
        /// </summary>
        Q = 81,
        /// <summary>
        /// R key.
        /// </summary>
        R = 82,
        /// <summary>
        /// S key.
        /// </summary>
        S = 83,
        /// <summary>
        /// T key.
        /// </summary>
        T = 84,
        /// <summary>
        /// U key.
        /// </summary>
        U = 85,
        /// <summary>
        /// V key.
        /// </summary>
        V = 86,
        /// <summary>
        /// W key.
        /// </summary>
        W = 87,
        /// <summary>
        /// X key.
        /// </summary>
        X = 88,
        /// <summary>
        /// Y key.
        /// </summary>
        Y = 89,
        /// <summary>
        /// Z key.
        /// </summary>
        Z = 90,

        /// <summary>
        /// Left windows key 
        /// </summary>
        WinLeft = 91,
        /// <summary>
        /// Right windows key
        /// </summary>
        WinRight = 92,
        /// <summary>
        /// Menu key, usually between right windows key and right control key.
        /// </summary>
        Menu = 93,
        /// <summary>
        /// Left windows key
        /// </summary>
        [Obsolete("Use WinLeft instead.")]
        LWin = 91,
        /// <summary>
        /// Right windows key
        /// </summary>
        [Obsolete("Use WinRight instead.")]
        RWin = 92,
        /// <summary>
        /// Menu key, usually between right windows key and right control key.
        /// </summary>
        [Obsolete("Use Menu instead.")]
        Apps = 93,
        /// <summary>
        /// ???
        /// </summary>
        Sleep = 95,
        /// <summary>
        /// Numeric keypad key 0
        /// </summary>
        NumPad0 = 96,
        /// <summary>
        /// Numeric keypad key 1
        /// </summary>
        NumPad1 = 97,
        /// <summary>
        /// Numeric keypad key 2
        /// </summary>
        NumPad2 = 98,
        /// <summary>
        /// Numeric keypad key 3
        /// </summary>
        NumPad3 = 99,
        /// <summary>
        /// Numeric keypad key 4
        /// </summary>
        NumPad4 = 100,
        /// <summary>
        /// Numeric keypad key 5
        /// </summary>
        NumPad5 = 101,
        /// <summary>
        /// Numeric keypad key 6
        /// </summary>
        NumPad6 = 102,
        /// <summary>
        /// Numeric keypad key 7
        /// </summary>
        NumPad7 = 103,
        /// <summary>
        /// Numeric keypad key 8
        /// </summary>
        NumPad8 = 104,
        /// <summary>
        /// Numeric keypad key 9
        /// </summary>
        NumPad9 = 105,
        /// <summary>
        /// Numeric keypad key *
        /// </summary>
        NumPadMultiply = 106,
        /// <summary>
        /// Numeric keypad key +
        /// </summary>
        NumPadPlus = 107,
        /// <summary>
        /// ?
        /// </summary>
        Separator = 108,
        /// <summary>
        /// Numeric keypad key -
        /// </summary>
        NumPadMinus = 109,
        /// <summary>
        /// Numeric keypad key period
        /// </summary>
        NumPadPeriod = 110,
        /// <summary>
        /// Numeric keypad key /
        /// </summary>
        NumPadSlash = 111,
        /// <summary>
        /// Function key
        /// </summary>
        F1 = 112,
        /// <summary>
        /// Function key
        /// </summary>
        F2 = 113,
        /// <summary>
        /// Function key
        /// </summary>
        F3 = 114,
        /// <summary>
        /// Function key
        /// </summary>
        F4 = 115,
        /// <summary>
        /// Function key
        /// </summary>
        F5 = 116,
        /// <summary>
        /// Function key
        /// </summary>
        F6 = 117,
        /// <summary>
        /// Function key
        /// </summary>
        F7 = 118,
        /// <summary>
        /// Function key
        /// </summary>
        F8 = 119,
        /// <summary>
        /// Function key
        /// </summary>
        F9 = 120,
        /// <summary>
        /// Function key
        /// </summary>
        F10 = 121,
        /// <summary>
        /// Function key
        /// </summary>
        F11 = 122,
        /// <summary>
        /// Function key
        /// </summary>
        F12 = 123,
        /// <summary>
        /// Function key
        /// </summary>
        F13 = 124,
        /// <summary>
        /// Function key
        /// </summary>
        F14 = 125,
        /// <summary>
        /// Function key
        /// </summary>
        F15 = 126,
        /// <summary>
        /// Function key
        /// </summary>
        F16 = 127,
        /// <summary>
        /// Function key
        /// </summary>
        F17 = 128,
        /// <summary>
        /// Function key
        /// </summary>
        F18 = 129,
        /// <summary>
        /// Function key
        /// </summary>
        F19 = 130,
        /// <summary>
        /// Function key
        /// </summary>
        F20 = 131,
        /// <summary>
        /// Function key
        /// </summary>
        F21 = 132,
        /// <summary>
        /// Function key
        /// </summary>
        F22 = 133,
        /// <summary>
        /// Function key
        /// </summary>
        F23 = 134,
        /// <summary>
        /// Function key
        /// </summary>
        F24 = 135,
        /// <summary>
        /// NumLock key
        /// </summary>
        NumLock = 144,
        /// <summary>
        /// Scroll Lock key
        /// </summary>
        ScrollLock = 145,

        /// <summary>
        /// 
        /// </summary>
        ShiftLeft = 160,
        /// <summary>
        /// 
        /// </summary>
        ShiftRight = 161,
        /// <summary>
        /// 
        /// </summary>
        ControlLeft = 162,
        /// <summary>
        /// 
        /// </summary>
        ControlRight = 163,

        /// <summary>
        /// Scroll Lock key
        /// </summary>
        [Obsolete("Use ScrollLock instead.")]
        Scroll = 145,
        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        LShiftKey = 160,
        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        RShiftKey = 161,
        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        LControlKey = 162,
        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        RControlKey = 163,
        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        LMenu = 164,
        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        RMenu = 165,
        /// <summary>
        /// 
        /// </summary>
        BrowserBack = 166,
        /// <summary>
        /// 
        /// </summary>
        BrowserForward = 167,
        /// <summary>
        /// 
        /// </summary>
        BrowserRefresh = 168,
        /// <summary>
        /// 
        /// </summary>
        BrowserStop = 169,
        /// <summary>
        /// 
        /// </summary>
        BrowserSearch = 170,
        /// <summary>
        /// 
        /// </summary>
        BrowserFavorites = 171,
        /// <summary>
        /// 
        /// </summary>
        BrowserHome = 172,
        /// <summary>
        /// 
        /// </summary>
        VolumeMute = 173,
        /// <summary>
        /// 
        /// </summary>
        VolumeDown = 174,
        /// <summary>
        /// 
        /// </summary>
        VolumeUp = 175,
        /// <summary>
        /// 
        /// </summary>
        MediaNextTrack = 176,
        /// <summary>
        /// 
        /// </summary>
        MediaPreviousTrack = 177,
        /// <summary>
        /// 
        /// </summary>
        MediaStop = 178,
        /// <summary>
        /// 
        /// </summary>
        MediaPlayPause = 179,
        /// <summary>
        /// 
        /// </summary>
        LaunchMail = 180,
        /// <summary>
        /// 
        /// </summary>
        SelectMedia = 181,
        /// <summary>
        /// 
        /// </summary>
        LaunchApplication1 = 182,
        /// <summary>
        /// 
        /// </summary>
        LaunchApplication2 = 183,
        /// <summary>
        /// Semicolon key
        /// </summary>
        Semicolon = 186,
        /// <summary>
        /// 
        /// </summary>
        [Obsolete]
        Oem1 = 186,
        /// <summary>
        /// Plus and equals key
        /// </summary>
        Plus = 187,
        /// <summary>
        /// Comma and less-than key. 
        /// </summary>
        Comma = 188,
        /// <summary>
        /// Minus and underscore key.
        /// </summary>
        Minus = 189,
        /// <summary>
        /// Period and greater-than key.
        /// </summary>
        Period = 190,
        /// <summary>
        /// Slash and question mark key.
        /// </summary>
        Slash = 191,
        /// <summary>
        /// Left angled quote and tilde key.
        /// </summary>
        Tilde = 192,
        /// <summary>
        /// Open bracket and brace key.
        /// </summary>
        OpenBracket = 219,
        /// <summary>
        /// Backslash and pipe key.
        /// </summary>
        BackSlash = 220,
        /// <summary>
        /// Close bracket and brace key.
        /// </summary>
        CloseBracket = 221,
        /// <summary>
        /// Single and double quotes key.
        /// </summary>
        Quotes = 222,
        /// <summary>
        /// ???
        /// </summary>
        [Obsolete]
        Oem8 = 223,

        
        // These values are different than the .NET key values.

        /// <summary>
        /// Shift key
        /// </summary>
        Shift = 1,
        /// <summary>
        /// Control key
        /// </summary>
        Control = 2,
        /// <summary>
        /// Alt key
        /// </summary>
        Alt = 4, 

    }

    /// <summary>
    /// Event handler event type.
    /// </summary>
    /// <param name="e"></param>
    public delegate void InputEventHandler(InputEventArgs e);

    /// <summary>
    /// Structure which keeps track of modifier keys applied to
    /// other keys.
    /// </summary>
    public struct KeyModifiers
    {
        private bool mAlt;
        private bool mControl;
        private bool mShift;

        /// <summary>
        /// Constructs a KeyModifiers structure with the given
        /// state of the alt, control and shift keys.
        /// </summary>
        /// <param name="alt"></param>
        /// <param name="control"></param>
        /// <param name="shift"></param>
        public KeyModifiers(bool alt, bool control, bool shift)
        {
            mAlt = alt;
            mControl = control;
            mShift = shift;
        }
        /// <summary>
        /// Gets or sets the state of the Alt key.
        /// </summary>
        public bool Alt
        {
            get { return mAlt; }
            set { mAlt = value; }
        }
        /// <summary>
        /// Gets or sets the state of the Control key.
        /// </summary>
        public bool Control
        {
            get { return mControl; }
            set { mControl = value; }
        }
        /// <summary>
        /// Gets or sets the state of the Shift key.
        /// </summary>
        public bool Shift
        {
            get { return mShift; }
            set { mShift = value; }
        }
    }
    
    /// <summary>
    /// Class which describes details about an input event.
    /// </summary>
    public class InputEventArgs 
    {
        KeyCode mKeyId;
        KeyModifiers mModifiers;
        int mRepeatCount;
        string mKeyString;
        Point mMousePosition;
        Mouse.MouseButtons mButtons;

        internal InputEventArgs()
        {
            Initialize();
        }
        internal InputEventArgs(KeyCode keyID, KeyModifiers mods)
        {
            mKeyId = keyID;
            mKeyString = Keyboard.GetKeyString(keyID, mods);
            mModifiers = mods;

            Initialize();
        }
        internal InputEventArgs(KeyCode keyID, KeyModifiers mods, int repeatCount)
            : this(keyID, mods)
        {
            mRepeatCount = repeatCount;
        }

        internal InputEventArgs(Mouse.MouseButtons mouseButtons)
        {
            mButtons = mouseButtons;

            Initialize();
        }

        private void Initialize()
        {
            mMousePosition = Mouse.Position;

        }

        /// <summary>
        /// Gets which key was pressed.
        /// </summary>
        public KeyCode KeyID
        {
            get { return mKeyId; }
            internal set { mKeyId = value; }
        }
        /// <summary>
        /// Gets the text created by the key which was pressed.
        /// </summary>
        public string KeyString
        {
            get { return mKeyString; }
        }

        /// <summary>
        /// The mouse position during this event
        /// </summary>
        public Point MousePosition
        {
            get { return mMousePosition; }
        }

        /// <summary>
        /// Gets how many times the keypress has been repeated.
        /// This is zero for the first time a key is pressed, and increases
        /// as the key is held down and KeyDown events are generated after that.
        /// </summary>
        public int RepeatCount
        {
            get { return mRepeatCount; }
        }

        /// <summary>
        /// Gets which mouse buttons were pressed.
        /// </summary>
        public Mouse.MouseButtons MouseButtons
        {
            get { return mButtons; }
        }
    }

    /// <summary>
    /// Static class which represents Keyboard input.
    /// </summary>
    [CLSCompliant(true)]
    public static class Keyboard
    {
        static KeyState mKeyState = new KeyState();

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
                                mKeyState[(int)id]-1);
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
            /// <summary>
            /// Gets the state of a key using the System.Windows.Forms.Keys enum values.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            [Obsolete("Use AgateForms.dll::FormUtil.TransformWinFormsKey to convert key and call Keyboard.Keys[] with the value returned instead.")]
            public bool GetWinFormsKey(System.Windows.Forms.Keys id)
            {
                return this[TransformWinFormsKey(id)];
            }
            /// <summary>
            /// Sets the state of a key using the System.Windows.Forms.Keys enum values.
            /// Used by Forms to respond to key events.
            /// </summary>
            /// <param name="id"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            [Obsolete("Use AgateForms.dll::FormUtil.TransformWinFormsKey to convert key and call Keyboard.Keys[] with the value returned instead.")]
            public void SetWinFormsKey(System.Windows.Forms.Keys id, bool value)
            {
                this[TransformWinFormsKey(id)] = value;
            }

            /// <summary>
            /// Converts a System.Windows.Forms.Keys value to a KeyCode value.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            [Obsolete("Use AgateForms.dll::FormUtil.TransformWinFormsKey instead.")]
            public static KeyCode TransformWinFormsKey(System.Windows.Forms.Keys id)
            {
                KeyCode myvalue;

                // sometimes windows reports Shift and sometimes ShiftKey.. what gives?
                switch (id)
                {
                    case System.Windows.Forms.Keys.Alt:
                    case System.Windows.Forms.Keys.Menu:
                        myvalue = KeyCode.Alt;
                        break;

                    case System.Windows.Forms.Keys.Control:
                    case System.Windows.Forms.Keys.ControlKey:
                        myvalue = KeyCode.Control;
                        break;

                    case System.Windows.Forms.Keys.Shift:
                    case System.Windows.Forms.Keys.ShiftKey:
                        myvalue = KeyCode.Shift;
                        break;

                    default:
                        myvalue = (KeyCode)id;
                        break;
                }
                return myvalue;
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
        /// Resets all keys to being in the up state (not pushed).
        /// Does generate KeyUp events.
        /// 
        /// Deprecated.  Use ReleaseAllKeys(false) instead.
        /// </summary>
        [Obsolete("Use ReleaseAllKeys(false) instead.")]
        public static void ClearAllKeys()
        {
            ReleaseAllKeys(false);
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

    }
}