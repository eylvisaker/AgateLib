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
using System.Diagnostics.Contracts;

namespace AgateLib.UserInterface.Widgets
{
	public abstract class Container : Widget
	{
		public Container()
		{
			Children = new WidgetList(this);
		}
		public Container(params Widget[] contents)
			: this()
		{
			Children.AddRange(contents);
		}

		public WidgetList Children { get; protected set; }

		/// <summary>
		/// Enumerates all descendants of this container.
		/// </summary>
		[Pure]
		public IEnumerable<Widget> Descendants
		{
			get
			{
				foreach (var w in Children)
				{
					yield return w;

					if (w is Container)
					{
						foreach (var ww in ((Container)w).Descendants)
							yield return ww;
					}
				}
			}
		}

		public void BringToFront(Widget item)
		{
			if (Children.Contains(item) == false)
				throw new InvalidOperationException("Selected child is not part of this container!");

			Children.Remove(item);
			Children.Add(item);
		}

		public override void Update(double delta_t, ref bool processInput)
		{
			foreach (var child in Children)
			{
				child.Update(delta_t, ref processInput);
			}
		}


		/// <summary>
		/// Handles the pressing of an input button.
		/// </summary>
		/// <remarks>When overriding this function, the 
		/// base method should be called if the input is not handled.
		/// </remarks>
		/// <param name="button"></param>
		/// <param name="handled">Set to true to indicate the the input was dealt with
		/// and should not be passed to a lower control</param>
		public virtual void OnInputButtonDown(AgateLib.InputLib.KeyCode button, ref bool handled)
		{
			foreach (var child in Children)
			{
				if (child is Container)
				{
					Container w = (Container)child;

					w.OnInputButtonDown(button, ref handled);

					if (handled)
						return;
				}
			}
		}

		/// <summary>
		/// Handles the release of an input button.
		/// </summary>
		/// <remarks>When overriding this function, the 
		/// base method should be called if the input is not handled.
		/// </remarks>
		/// <param name="button"></param>
		/// <param name="handled">Set to true to indicate the the input was dealt with
		/// and should not be passed to a lower control</param>
		public virtual void OnInputButtonUp(AgateLib.InputLib.KeyCode button, ref bool handled)
		{
			foreach (var child in Children)
			{
				if (child is Container)
				{
					Container w = (Container)child;

					w.OnInputButtonUp(button, ref handled);

					if (handled)
						return;
				}
			}
		}

		internal override Size ComputeSize(int? minWidth, int? minHeight, int? maxWidth, int? maxHeight)
		{
			if (AutoSize == false)
				return new Size(Width, Height);

			Size retval = new Size();

			foreach (var child in Children)
			{
				var sz = child.ComputeSize(minWidth, minHeight, maxWidth, maxHeight);

				retval.Width = Math.Max(sz.Width + child.X, retval.Width);
				retval.Height = Math.Max(sz.Height + child.Y, retval.Height);
			}

			return retval;
		}

		internal override void DoAutoSize()
		{
			if (AutoSize == false)
				return;

			foreach (var child in Children)
				child.DoAutoSize();

			base.DoAutoSize();
		}

		public override void Refresh()
		{
			foreach (var child in Children)
				child.Refresh();
		}

		public override void Focus()
		{
			base.Focus();
		}

		protected internal override Widget FindFocusWidget()
		{
			foreach (var ctrl in Children)
			{
				var retval = ctrl.FindFocusWidget();

				if (retval != null)
					return retval;
			}

			if (AcceptFocus)
				return this;

			return null;
		}

		internal void OnBeforeDraw()
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// If this is set to true, then the layout engine will not determine the
		/// size of this control, or the size or positions of any child controls.
		/// Grandchildren of this control will have layout performed as normal.
		/// </summary>
		public bool ManualLayout { get; set; }


		public ScrollAxes AllowScroll { get; set; }
		public Point ScrollOffset { get; set; }

		public void ScrollToWidget(Widget widget)
		{
			Contract.Ensures(InOverflow(widget) == false);

			Contract.Assert(IsDescendant(widget));

			Point location = ClientLocationOf(widget);
			var newOffset = ScrollOffset;

			if ((AllowScroll & ScrollAxes.Vertical) != 0)
			{
				int bottom = widget.WidgetRect.Bottom;

				if (bottom - newOffset.Y > ClientRect.Height)
				{
					newOffset.Y = bottom - ClientRect.Height;
				}
				if (widget.WidgetRect.Top < newOffset.Y)
				{
					newOffset.Y = widget.WidgetRect.Top;
				}
			}
			if ((AllowScroll & ScrollAxes.Horizontal) != 0)
			{
				int right = widget.WidgetRect.Right;

				if (right - newOffset.X > ClientRect.Width)
				{
					int diff = right - ClientRect.Width;
					newOffset.X = diff;
				}

				if (widget.WidgetRect.Left < newOffset.X)
				{
					newOffset.X = widget.WidgetRect.Left;
				}
			}

			ScrollOffset = newOffset;
		}

		[Pure]
		protected bool IsDescendant(Widget widget)
		{
			foreach(var child in Children)
			{
				if (child == widget)
					return true;

				if (child is Container)
				{
					var result = ((Container)child).IsDescendant(widget);
					if (result)
						return true;
				}
			}

			return false;
		}

		[Pure]
		private bool InOverflow(Widget widget)
		{
			var bounds = ClientRect;
			var wr = widget.WidgetRect;
			bounds.Location = ScrollOffset;

			if (wr.Left < bounds.Left) return true;
			if (wr.Top < bounds.Top) return true;
			if (wr.Right > bounds.Right) return true;
			if (wr.Bottom > bounds.Bottom) return true;

			return false;
		}

		public Point ClientLocationOf(Widget widget)
		{
			Contract.Requires<ArgumentException>(Descendants.Contains(widget));

			var w = widget;
			Point pt = Point.Empty;

			while (w != null && w != this)
			{
				pt = w.ClientToParent(pt);
				w = w.Parent;
			}

			return pt;
		}
	}

	[Flags]
	public enum ScrollAxes
	{
		None = 0,
		Horizontal = 1,
		Vertical = 2,
		Both = 3,
	}

}
