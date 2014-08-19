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
		public MenuItem()
		{
			Children.WidgetAdded += Children_WidgetAdded;
			Children.WidgetRemoved += Children_WidgetRemoved;
		}

		public MenuItem(params Widget[] children) : this()
		{
			Children.AddRange(children);
		}

		public static MenuItem OfLabel(string text, string name = "")
		{
			return new MenuItem(new Label(text)) { Name = name };
		}

		[Obsolete("Use static MenuItem.OfLabel method instead.")]
		public MenuItem(string name, string text) : this()
		{
			Name = name;

			Children.Add(new Label(text));
		}
		public MenuItem(Widget child) : this()
		{
			Children.Add(child);
		}
		public MenuItem(string name, Widget child) : this()
		{
			Name = name;

			Children.Add(child);
		}

		public override Container Parent
		{
			get { return base.Parent; }
			set
			{
				if (value is Menu == false)
					throw new InvalidOperationException("Cannot add a menu item to a container which is not a menu.");

				base.Parent = value;
			}
		}
		Menu ParentMenu
		{
			get { return (Menu)Parent; }
		}

		public bool Selected { get; set; }

		public event EventHandler PressAccept;
		public event EventHandler PressToggle;
		public event EventHandler PressMenu;
		public event EventHandler Select;

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
			widget.MouseUp -= widget_MouseUp;
		}

		bool mouseDown;
		void widget_MouseDown(object sender, MouseEventArgs e)
		{
			mouseDown = true;
		}
		void widget_MouseUp(object sender, MouseEventArgs e)
		{
			if (mouseDown)
			{
				OnPressAccept();
			}

			mouseDown = false;
		}
		void widget_MouseMove(object sender, MouseEventArgs e)
		{
			OnSelect();
		}


		internal void OnSelect()
		{
			if (Select != null)
				Select(this, EventArgs.Empty);
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
	}
}
