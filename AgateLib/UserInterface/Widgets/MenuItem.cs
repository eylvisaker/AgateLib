//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Mathematics.Geometry;

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

		public override Widget Parent
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

		internal void OnSelect()
		{
			var parentContainer = Parent as Menu;

			if (parentContainer != null)
			{
				parentContainer.WidgetStyle.ScrollToWidget(this);
			}

			Select?.Invoke(this, EventArgs.Empty);
		}

		void OnDeselect()
		{
			Deselect?.Invoke(this, EventArgs.Empty);
		}

		internal void OnPressAccept()
		{
			PressAccept?.Invoke(this, EventArgs.Empty);
		}

		internal void OnPressToggle()
		{
			PressToggle?.Invoke(this, EventArgs.Empty);
		}

		internal void OnPressMenu()
		{
			PressMenu?.Invoke(this, EventArgs.Empty);
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
