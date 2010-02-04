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
	/// Class which represents a radio button.  Radio buttons within the 
	/// same container are constrained so that only one of them can be
	/// checked at once.
	/// </summary>
	public class RadioButton : Widget
	{
		bool mouseDownIn;

		/// <summary>
		/// Constructs a radio button.
		/// </summary>
		public RadioButton()
		{
			Name = "RadioButton";
		}
		/// <summary>
		/// Constructs a radio button with the specified text.
		/// </summary>
		/// <param name="text"></param>
		public RadioButton(string text)
		{
			Name = text;
			Text = text;
		}
		private bool mChecked;

		/// <summary>
		/// Gets or sets whether or not this radio button object is checked.
		/// </summary>
		public bool Checked
		{
			get { return mChecked; }
			set
			{
				if (value == mChecked)
					return;

				mChecked = value;

				if (value == true && Parent != null)
				{
					foreach (Widget w in Parent.Children)
					{
						RadioButton rad = w as RadioButton;

						if (rad == null || rad == this)
							continue;

						rad.Checked = false;
					}
				}

				OnCheckChanged();
			}
		}

		/// <summary>
		/// Always returns true.
		/// </summary>
		public override bool CanHaveFocus
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Processes a keydown event for the radio button.
		/// </summary>
		/// <param name="e"></param>
		protected internal override void SendKeyDown(AgateLib.InputLib.InputEventArgs e)
		{
			if (e.KeyCode == AgateLib.InputLib.KeyCode.Space)
			{
				Checked = true;
			}
		}

		/// <summary>
		/// Processes the mouse down event.  If overriding, be sure to call base.OnMouseDown.
		/// </summary>
		/// <param name="e"></param>
		protected internal override void OnMouseDown(AgateLib.InputLib.InputEventArgs e)
		{
			if (Enabled == false)
				return;

			mouseDownIn = true;
		}
		/// <summary>
		/// Processes the mouse down event.  If overriding, be sure to call base.OnMouseDown.
		/// </summary>
		/// <param name="e"></param>
		protected internal override void OnMouseUp(AgateLib.InputLib.InputEventArgs e)
		{
			if (MouseIn && mouseDownIn)
				Checked = true;

			mouseDownIn = false;
		}

		private void OnCheckChanged()
		{
			if (CheckChanged != null)
				CheckChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// Event which is raised when the check state is changed.
		/// </summary>
		public event EventHandler CheckChanged;
	}
}
