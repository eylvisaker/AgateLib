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
using AgateLib.InputLib;

namespace AgateLib.Gui
{
	/// <summary>
	/// Class for a button in the GUI.  A button is a very simple "command" interface, which
	/// the user can click on to activate.
	/// </summary>
	public class Button : Widget
	{
		public Button() { Name = "Button"; }
		public Button(string text) { Name = text; Text = text; }

		bool spaceDownFocus = false;

		public override bool CanHaveFocus
		{
			get
			{
				return true;
			}
		}
		internal bool DrawActivated
		{
			get { return MouseDownIn && MouseIn || spaceDownFocus; }
		}
		internal bool IsDefaultButton
		{
			get
			{
				Container p = this.Parent;

				while (p is Window == false)
					p = p.Parent;

				return ((Window)p).AcceptButton == this;
			}
		}

		protected internal override void OnKeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
			{
				spaceDownFocus = true;
			}

			base.OnKeyDown(e);
		}
		protected internal override void OnKeyUp(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
			{
				spaceDownFocus = false;
				OnClick();
			}

			base.OnKeyUp(e);
		}
		protected internal override void SendMouseUp(InputEventArgs e)
		{
			if (MouseIn && MouseDownIn)
				OnClick();

			base.SendMouseUp(e);
		}

		private void OnClick()
		{
			if (Click != null)
				Click(this, EventArgs.Empty);
		}
		public event EventHandler Click;
	}
}
