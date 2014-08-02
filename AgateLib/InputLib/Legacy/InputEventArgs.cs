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
using AgateLib.Geometry;

namespace AgateLib.InputLib.Legacy
{
	/// <summary>
	/// Class which describes details about an input event.
	/// </summary>
	[Obsolete("Use AgateInputEventArgs instead.")]
	public class InputEventArgs
	{
		private AgateInputEventArgs args;
		int mRepeatCount;

		internal InputEventArgs(AgateInputEventArgs args)
		{
			this.args = args;
			Initialize();
		}
		private void Initialize()
		{
			if (args == null)
				args = new AgateInputEventArgs();

			MousePosition = Mouse.Position;
		}
		internal InputEventArgs()
		{
			Initialize();
		}
		internal InputEventArgs(KeyCode keyID, KeyModifiers mods)
		{
			KeyCode = keyID;
			KeyString = Keyboard.GetKeyString(keyID, mods);
			Modifiers = mods;

			Initialize();
		}
		[Obsolete("Don't use this one.", true)]
		internal InputEventArgs(KeyCode keyID, KeyModifiers mods, int repeatCount)
			: this(keyID, mods)
		{
			mRepeatCount = repeatCount;
		}

		internal InputEventArgs(MouseButton mouseButtons)
		{
			args = new AgateInputEventArgs();
			MouseButtons = mouseButtons;

			Initialize();
		}
		internal InputEventArgs(int wheelDelta)
		{
			args = new AgateInputEventArgs();
			WheelDelta = wheelDelta;

			Initialize();
		}



		/// <summary>
		/// Gets which key was pressed.
		/// </summary>
		public KeyCode KeyCode
		{
			get { return args.KeyCode; }
			internal set { args.KeyCode = value; }
		}
		/// <summary>
		/// Gets the text created by the key which was pressed.
		/// </summary>
		public string KeyString
		{
			get { return args.KeyString; }
			internal set { args.KeyString = value; }
		}

		/// <summary>
		/// The mouse position during this event
		/// </summary>
		public Point MousePosition
		{
			get { return args.MousePosition; }
			internal set { args.MousePosition = value; }
		}

		/// <summary>
		/// Gets how many times the keypress has been repeated.
		/// This is zero for the first time a key is pressed, and increases
		/// as the key is held down and KeyDown events are generated after that.
		/// </summary>
		[Obsolete("Use KeyPress event instead.", true)]
		public int RepeatCount
		{
			get { return mRepeatCount; }
		}

		/// <summary>
		/// Gets which mouse buttons were pressed.
		/// </summary>
		public MouseButton MouseButtons
		{
			get { return args.MouseButton; }
			internal set { args.MouseButton = value; }
		}

		/// <summary>
		/// Gets the amount the mouse wheel moved in this event.
		/// </summary>
		public int WheelDelta
		{
			get { return args.MouseWheelDelta; }
			internal set { args.MouseWheelDelta = value; }
		}

		public KeyModifiers Modifiers
		{
			get { return args.KeyModifiers; }
			set { args.KeyModifiers = value; }
		}
	}
}
