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
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class MenuItem : Container
	{
		bool mSelected;

		public MenuItem()
		{
			Children.WidgetAdded += Children_WidgetAdded;
			Children.WidgetRemoved += Children_WidgetRemoved;
		}

		public MenuItem(params Widget[] children)
			: this()
		{
			Children.AddRange(children);
		}

		public static MenuItem OfLabel(string text, string name = "")
		{
			return new MenuItem(new Label(text)) { Name = name };
		}

		[Obsolete("Use static MenuItem.OfLabel method instead.")]
		public MenuItem(string name, string text)
			: this()
		{
			Name = name;

			Children.Add(new Label(text));
		}
		public MenuItem(Widget child)
			: this()
		{
			Children.Add(child);
		}
		public MenuItem(string name, Widget child)
			: this()
		{
			Name = name;

			Children.Add(child);
		}

		public override Container Parent
		{
			get { return base.Parent; }
			set
			{
				if (value != null && value is Menu == false)
					throw new InvalidOperationException("Cannot add a menu item to a container which is not a menu.");

				base.Parent = value;
			}
		}
		Menu ParentMenu
		{
			get { return (Menu)Parent; }
		}

		public bool Selected
		{
			get { return mSelected; }
			set
			{
				if (value == mSelected)
					return;

				mSelected = value;

				if (mSelected)
					OnSelect();
				else
					OnDeselect();

				StyleDirty = true;
			}
		}

        /// <summary>
        /// Event raised when an accept event (button, mouse click) occurs on this menu item.
        /// </summary>
		public event EventHandler PressAccept;
		public event EventHandler PressToggle;
		public event EventHandler PressMenu;
        /// <summary>
        /// Event raised when the menu item is selected (gains focus).
        /// </summary>
		public event EventHandler Select;
		public event EventHandler Deselect;
		public event EventHandler Discard;

		void Children_WidgetAdded(object sender, WidgetEventArgs e)
		{
			var widget = e.Widget;
			ListenToEventsIn(widget);
		}
		void Children_WidgetRemoved(object sender, WidgetEventArgs e)
		{
			var widget = e.Widget;
			IgnoreEventsIn(widget);
		}

		private void ListenToEventsIn(Widget widget)
		{
			var container = widget as Container;

			if (container != null)
			{
				container.Children.WidgetAdded += Children_WidgetAdded;
				container.Children.WidgetRemoved += Children_WidgetRemoved;

				foreach (var c in container.Children)
					ListenToEventsIn(c);
			}

			widget.MouseDown += widget_MouseDown;
			widget.MouseMove += widget_MouseMove;
			widget.MouseUp += widget_MouseUp;
		}

		private void IgnoreEventsIn(Widget widget)
		{
			var container = widget as Container;

			if (container != null)
			{
				container.Children.WidgetAdded -= Children_WidgetAdded;
				container.Children.WidgetRemoved -= Children_WidgetRemoved;

				foreach (var c in container.Children)
					IgnoreEventsIn(c);
			}

			widget.MouseDown -= widget_MouseDown;
			widget.MouseMove -= widget_MouseMove;
			widget.MouseUp -= widget_MouseUp;
		}

		bool mouseDown;

		void widget_MouseDown(object sender, MouseEventArgs e)
		{
			OnMouseDown(e.Buttons, e.Location);
		}
		void widget_MouseUp(object sender, MouseEventArgs e)
		{
			OnMouseUp(e.Buttons, e.Location);
		}
		void widget_MouseMove(object sender, MouseEventArgs e)
		{
			OnMouseMove(e.Location);
		}

		protected internal override void OnMouseDown(InputLib.MouseButton mouseButtons, Point point)
		{
			mouseDown = true;

			base.OnMouseDown(mouseButtons, point);
		}
		protected internal override void OnMouseUp(InputLib.MouseButton mouseButtons, Point point)
		{
			base.OnMouseUp(mouseButtons, point);

			if (mouseDown && MouseIn && ParentMenu.SelectedItem == this)
			{
				//OnPressAccept();
			}

			mouseDown = false;
		}

		protected internal override void OnMouseEnter()
		{
			base.OnMouseEnter();

			ParentMenu.SelectedItem = this;
		}
		protected internal override void OnMouseLeave()
		{
			if (ParentMenu.SelectedItem == this)
				ParentMenu.SelectedItem = null;

			base.OnMouseLeave();
		}

		internal void OnSelect()
		{
            if (Parent != null)
            {
                Parent.ScrollToWidget(this);
            }

			if (Select != null)
				Select(this, EventArgs.Empty);
		}
		void OnDeselect()
		{
			if (Deselect != null)
				Deselect(this, EventArgs.Empty);
		}

		internal void OnPressAccept()
		{
			if (PressAccept != null)
				PressAccept(this, EventArgs.Empty);
			else
				System.Diagnostics.Debug.Assert(false, "A menu item was selected, but it has no event handler attached.");
		}
		internal void OnPressToggle()
		{
			if (PressToggle != null)
				PressToggle(this, EventArgs.Empty);
		}
		internal void OnPressMenu()
		{
			if (PressMenu != null)
				PressMenu(this, EventArgs.Empty);
		}

		public Point Pointer { get; set; }
		public bool ManualSetPointer { get; set; }

		public bool AllowDiscard { get; set; }

		internal void OnDiscard()
		{
			if (Discard != null)
				Discard(this, EventArgs.Empty);
		}
	}
}
