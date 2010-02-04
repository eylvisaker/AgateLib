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
using System.Linq;
using System.Text;

namespace AgateLib.Gui
{
	/// <summary>
	/// A checkbox object.
	/// </summary>
	public class CheckBox : Widget
	{
		/// <summary>
		/// Constructs a checkbox object.
		/// </summary>
		public CheckBox()
		{
			Name = "Checkbox";
		}
		/// <summary>
		/// Constructs a checkbox object.
		/// </summary>
		/// <param name="text"></param>
		public CheckBox(string text)
		{
			Name = text;
			Text = text;
		}
		private bool mChecked;

		/// <summary>
		/// Gets or sets whether the checkbox is checked.
		/// </summary>
		public bool Checked
		{
			get { return mChecked; }
			set
			{
				mChecked = value;

				OnCheckChanged();
			}
		}

		/// <summary>
		/// Returns true.
		/// </summary>
		public override bool CanHaveFocus
		{
			get
			{
				return true;
			}
		}

		bool mouseDownIn;
		/// <summary>
		/// Handles the mouse down event.
		/// </summary>
		/// <param name="e"></param>
		protected internal override void OnMouseDown(AgateLib.InputLib.InputEventArgs e)
		{
			if (Enabled == false)
				return;

			mouseDownIn = true;
		}
		/// <summary>
		///  Handles the mouse up event.
		/// </summary>
		/// <param name="e"></param>
		protected internal override void OnMouseUp(AgateLib.InputLib.InputEventArgs e)
		{
			if (MouseIn && mouseDownIn)
				Checked = !Checked;

			mouseDownIn = false;

		}
		/// <summary>
		/// Handles the key down event.
		/// </summary>
		/// <param name="e"></param>
		protected internal override void SendKeyDown(AgateLib.InputLib.InputEventArgs e)
		{
			if (e.KeyCode == AgateLib.InputLib.KeyCode.Space)
			{
				Checked = !Checked;
			}
		}

		private void OnCheckChanged()
		{
			if (CheckChanged != null)
				CheckChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Event raised when the check state is changed.
		/// </summary>
		public event EventHandler CheckChanged;
	}
}
