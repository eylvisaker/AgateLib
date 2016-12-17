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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

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

}
