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
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Gui
{
	/// <summary>
	/// A window is a general purpose low-level container for other widgets.
	/// </summary>
	public class Window : Container
	{
		/// <summary>
		/// Gets or sets the location of the window.
		/// </summary>
		public new Point Location
		{
			get { return base.Location; }
			set { base.Location = value; }
		}
		/// <summary>
		/// Gets or sets the size of the window.
		/// </summary>
		public new Size Size
		{
			get { return base.Size; }
			set { base.Size = value; }
		}

		/// <summary>
		/// Constructs a window object.
		/// </summary>
		public Window()
		{
			Name = "window";
			ShowTitleBar = true;

			Location = new Point(20, 20);
			Size = new Size(300, 250);
		}
		/// <summary>
		/// Constructs a window object.
		/// </summary>
		/// <param name="title">The title of the window to be displayed.</param>
		public Window(string title)
			: this()
		{
			Text = title;
			Name = title;
		}

		protected override void OnParentChanged()
		{
			if (Parent != null && Parent is GuiRoot == false)
			{
				Parent = null;
				throw new InvalidOperationException("Cannot add a window to anything other than a GuiRoot object.");
			}

			base.OnParentChanged();
		}

		/// <summary>
		/// The button which is clicked when the user presses enter.
		/// </summary>
		public Button AcceptButton { get; set; }
		/// <summary>
		/// The button which is clicked when the user presses escape.
		/// </summary>
		public Button CancelButton { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the window should allow the user
		/// to drag it around.
		/// </summary>
		public bool AllowDrag { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether the window should display a title bar.
		/// </summary>
		public bool ShowTitleBar { get; set; }

		bool dragging;
		Point mouseDiff;

		protected internal override void OnMouseDown(AgateLib.InputLib.InputEventArgs e)
		{
			if (AllowDrag)
			{
				dragging = true;
				mouseDiff = new Point(
					ScreenLocation.X - e.MousePosition.X,
					ScreenLocation.Y - e.MousePosition.Y);
			}
		}
		protected internal override void OnMouseUp(AgateLib.InputLib.InputEventArgs e)
		{
			dragging = false;
		}
		protected internal override void OnMouseMove(AgateLib.InputLib.InputEventArgs e)
		{
			if (dragging == false)
				return;

			Point newPos = new Point(e.MousePosition.X + mouseDiff.X, e.MousePosition.Y + mouseDiff.Y);

			if (Parent.Width - this.Width < newPos.X)
				newPos.X = Parent.Width - this.Width;
			if (Parent.Height - this.Height < newPos.Y)
				newPos.Y = Parent.Height - this.Height;
			if (newPos.X < 0) newPos.X = 0;
			if (newPos.Y < 0) newPos.Y = 0;

			Location = Parent.PointToClient(newPos);
		}
	}
}
