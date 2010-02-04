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
		/// <summary>
		/// Constructs a button object.
		/// </summary>
		public Button() { Name = "Button"; }
		/// <summary>
		/// Constructs a button object.
		/// </summary>
		/// <param name="text"></param>
		public Button(string text) { Name = text; Text = text; }

		bool spaceDownFocus = false;

		/// <summary>
		/// Returns true
		/// </summary>
		public override bool CanHaveFocus
		{
			get			{				return true;			}
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

		/// <summary>
		/// Handles the key down event
		/// </summary>
		/// <param name="e"></param>
		protected internal override void OnKeyDown(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
			{
				spaceDownFocus = true;
			}

			base.OnKeyDown(e);
		}
		/// <summary>
		/// Handles the key up event
		/// </summary>
		/// <param name="e"></param>
		protected internal override void OnKeyUp(InputEventArgs e)
		{
			if (e.KeyCode == KeyCode.Space)
			{
				spaceDownFocus = false;
				OnClick();
			}

			base.OnKeyUp(e);
		}
		/// <summary>
		/// Handles the release of the mouse button in the widget.
		/// </summary>
		/// <param name="e"></param>
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
		
		/// <summary>
		/// Event raised when the button is clicked.
		/// </summary>
		public event EventHandler Click;
	}
}
