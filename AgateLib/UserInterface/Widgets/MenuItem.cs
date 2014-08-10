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
		public MenuItem(string name, string text)
		{
			Name = name;

			Children.Add(new Label(text));
		}
		public MenuItem(Widget child)
		{
			Children.Add(child);
		}
		public MenuItem(string name, Widget child)
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
