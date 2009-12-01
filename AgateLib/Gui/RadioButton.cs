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
	public class RadioButton : Widget
	{
		public RadioButton()
		{
			Name = "RadioButton";
		}
		public RadioButton(string text)
		{
			Name = text;
			Text = text;
		}
		private bool mChecked;

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

		public override bool CanHaveFocus
		{
			get
			{
				return true;
			}
		}

		protected internal override void SendKeyDown(AgateLib.InputLib.InputEventArgs e)
		{
			if (e.KeyCode == AgateLib.InputLib.KeyCode.Space)
			{
				Checked = true;
			}
		}

		bool mouseDownIn;
		protected internal override void OnMouseDown(AgateLib.InputLib.InputEventArgs e)
		{
			if (Enabled == false)
				return;

			mouseDownIn = true;
		}
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

		public event EventHandler CheckChanged;
	}
}
