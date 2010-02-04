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
using AgateLib.Geometry;
using AgateLib.InputLib;
using System.Diagnostics;

namespace AgateLib.Gui
{
	/// <summary>
	/// Abstract base class for a GUI component that can contain other components.
	/// </summary>
	public abstract class Container : Widget
	{
		WidgetList mChildren;
		Rectangle mClientArea;
		ILayoutPerformer mLayout = new Layout.VerticalBox();
		bool mLayoutSuspended;

		/// <summary>
		/// Constructs a container object.
		/// </summary>
		public Container()
		{
			mChildren = new WidgetList(this);
			mChildren.ListUpdated += new EventHandler(mChildren_ListUpdated);
		}

		internal bool AnyChildCanHaveFocus
		{
			get
			{
				for (int i = 0; i < mChildren.Count; i++)
				{
					if (mChildren[i].CanHaveFocus)
						return true;

					if (mChildren[i] is Container)
					{
						if (((Container)mChildren[i]).AnyChildCanHaveFocus)
							return true;
					}
				}

				return false;
			}
		}
		/// <summary>
		/// Gets the client area of the container.
		/// </summary>
		public Rectangle ClientArea { get { return mClientArea; } }

		/// <summary>
		/// Gets the size of the client area of the container.
		/// </summary>
		public Size ClientSize
		{
			get { return mClientArea.Size; }
			set
			{
				Size = Root.ThemeEngine.RequestClientAreaSize(this, value);
			}
		}
		/// <summary>
		/// Gets or sets the object which handles the layout for the container.
		/// </summary>
		public virtual ILayoutPerformer Layout
		{
			get { return mLayout; }
			set
			{
				mLayout = value;
				RedoLayout();
			}
		}

		public override Point PointToScreen(Point p)
		{
			return base.PointToScreen(
				new Point(p.X + ClientArea.X, p.Y + ClientArea.Y));
		}
		public override Point PointToClient(Point p)
		{
			Point retval = base.PointToClient(p);

			retval.X -= ClientArea.X;
			retval.Y -= ClientArea.Y;

			return retval;
		}
		protected override void OnResize()
		{
			if (Root == null)
				return;

			mClientArea = Root.ThemeEngine.GetClientArea(this);
			DoLayoutPrivate();

			base.OnResize();
		}
		protected override void OnParentChanged()
		{
			if (Parent == null)
				return;

			OnResize();
			base.OnParentChanged();
		}

		public void SuspendLayout()
		{
			mLayoutSuspended = true;

			foreach (Container child in mChildren)
			{
				child.SuspendLayout();
			}
		}
		public void ResumeLayout()
		{
			mLayoutSuspended = false;

			RedoLayout();

			foreach (Widget child in mChildren)
			{
				if (child is Container == false)
					continue;

				((Container)child).ResumeLayout();
			}

		}
		protected internal override void RecalcSizeRange()
		{
			if (mLayoutSuspended)
				return;
			if (Root == null)
				return;

			try
			{
				InLayout = true;

				MinSize = Layout.RecalcMinSize(this);
			}
			finally
			{
				InLayout = false;
			}
		}

		void mChildren_ListUpdated(object sender, EventArgs e)
		{
			DoLayoutPrivate();
		}

		public WidgetList Children
		{
			get { return mChildren; }
		}

		protected internal override void UpdateGui()
		{
			for (int i = 0; i < Children.Count; i++)
				Children[i].UpdateGui();

			base.UpdateGui();
		}

		void DoLayoutPrivate()
		{
			RedoLayout();
			Invalidate();
		}

		internal bool InLayout { get; private set; }

		public virtual void RedoLayout()
		{
			if (mLayoutSuspended)
				return;
			if (Root == null)
				return;
			if (InLayout)
				return;

			try
			{
				InLayout = true;

				RecalcSizeRange();
				Layout.DoLayout(this);
			}
			finally
			{
				InLayout = false;
			}
		}
		protected override void DoDraw()
		{
			base.DoDraw();

			foreach (var child in Children.VisibleItems)
				child.Draw();
		}

		protected internal override bool AcceptFocusOnMouseDown
		{
			get { return false; }
		}

		protected internal override bool AcceptInputKey(KeyCode keyCode)
		{
			return Layout.AcceptInputKey(keyCode);
		}

		//Widget FindMouseInControl(Point screenMousePoint)
		//{
		//    foreach (Widget child in mChildren)
		//    {
		//        if (child.ContainsScreenPoint(screenMousePoint) == true)
		//            return child;
		//    }

		//    return null;
		//}


		internal Widget NearestChildTo(Point pt, bool skipDisabled)
		{
			int distance = int.MaxValue;
			Widget retval = null;

			foreach (Widget child in Children)
			{
				if (child.Enabled == false && skipDisabled)
					continue;

				Point center = new Point(
					child.Location.X + child.Width / 2,
					child.Location.Y + child.Height / 2);

				int dist = Math.Abs(center.X - pt.X) + Math.Abs(center.Y - pt.Y);

				if (dist < distance)
				{
					retval = child;
					distance = dist;
				}
			}

			return retval;
		}

		internal Widget CanMoveFocus(Widget currentFocus, Direction direction)
		{
			if (this.Children.Contains(currentFocus) == false)
				throw new ArgumentException("currentFocus does not belong to this container.");

			return Layout.CanMoveFocus(this, currentFocus, direction);
		}
	}

	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
	}

}