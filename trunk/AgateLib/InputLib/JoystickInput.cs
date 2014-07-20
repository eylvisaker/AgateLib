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
using System.Linq;
using System.Text;
using AgateLib.Drivers;
using AgateLib.InputLib.ImplementationBase;

namespace AgateLib.InputLib
{
	/// <summary>
	/// Static class which contains a list of the joystick input devices attached
	/// to the computer.
	/// </summary>
	public static class JoystickInput
	{
		static InputImpl impl;
		static List<Joystick> mRawJoysticks = new List<Joystick>();

		/// <summary>
		/// Initializes the input system by instantiating the driver with the given
		/// InputTypeID.  The input driver must be registered with the Registrar
		/// class.
		/// </summary>
		/// <param name="inputType"></param>
		public static void Initialize(InputTypeID inputType)
		{
			Core.Initialize();

			impl = Registrar.CreateInputDriver(inputType);
			impl.Initialize();

			InitializeJoysticks();
		}

		private static void InitializeJoysticks()
		{
			mRawJoysticks.Clear();
			mRawJoysticks.AddRange(impl.CreateJoysticks().Select(x => new Joystick(x)));
		}

		/// <summary>
		/// Gets the list of joysticks.
		/// </summary>
		public static IList<Joystick> Joysticks
		{
			get { return mRawJoysticks; }
		}

		internal static void PollTimer()
		{
			for (int i = 0; i < mRawJoysticks.Count; i++)
				mRawJoysticks[i].Poll();
		}

		internal static void Dispose()
		{
			if (impl != null)
				impl.Dispose();
		}
	}
}
