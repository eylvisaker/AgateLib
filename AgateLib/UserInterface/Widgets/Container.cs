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
using AgateLib.Platform;
using AgateLib.Quality;

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
		
		protected internal override IEnumerable<Widget> RenderChildren => Children;

		protected internal override IEnumerable<Widget> LayoutChildren => Children;

		/// <summary>
		/// Enumerates all descendants of this container.
		/// </summary>
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

		public override void Update(ClockTimeSpan elapsed, ref bool processInput)
		{
			foreach (var child in Children)
			{
				child.Update(elapsed, ref processInput);
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

		internal override Size ComputeSize(int? maxWidth, int? maxHeight)
		{
			if (AutoSize == false)
				return new Size(Width, Height);

			Size result = new Size();

			foreach (var child in Children)
			{
				var sz = child.ComputeSize(maxWidth, maxHeight);

				result.Width = Math.Max(sz.Width + child.X, result.Width);
				result.Height = Math.Max(sz.Height + child.Y, result.Height);
			}

			return result;
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
				var result = ctrl.FindFocusWidget();

				if (result != null)
					return result;
			}

			if (AcceptFocus)
				return this;

			return null;
		}
		
		[Obsolete("Use WidgetStyle.ScrollToWidget instead.")]
		public void ScrollToWidget(Widget widget)
		{
			WidgetStyle.ScrollToWidget(widget);
		}

		public bool IsDescendant(Widget widget)
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
