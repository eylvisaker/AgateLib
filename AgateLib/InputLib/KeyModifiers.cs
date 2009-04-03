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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
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
}
