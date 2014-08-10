using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class Container : Widget
	{
		public Container()
		{
			Children = new WidgetList(this);
		}
		public Container(params Widget[] contents) : this()
		{
			Children.AddRange(contents);
		}

		public WidgetList Children { get; protected set; }

		public bool ChildHasMouseIn
		{
			get
			{
				return Children.Any(x => x.MouseIn) ||
					Children.OfType<Container>().Any(x => x.ChildHasMouseIn);
			}
		}

		public void BringToFront(Widget item)
		{
			if (Children.Contains(item) == false)
				throw new InvalidOperationException("Selected child is not part of this container!");

			Children.Remove(item);
			Children.Insert(0, item);
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
			foreach(var ctrl in Children)
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
	}
}
