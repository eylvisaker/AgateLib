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
using AgateLib.InputLib;
using System.Diagnostics;

namespace AgateLib.Gui
{
	/// <summary>
	/// Base class for a widget.
	/// </summary>
	public abstract class Widget
	{
		Rectangle mRegion;
		Container mParent;
		string mText;
		LayoutExpand mLayoutExpand;
		bool mAutoCalcMinSize = true;
		bool mAutoCalcMaxSize = true;
		Size mMinSize, mMaxSize;
		bool mVisible = true;
		bool mEnabled = true;

		bool suspendLayout = false;
		bool mMouseDownIn = false;
		
		/// <summary>
		/// The default constructor for a Widget.
		/// </summary>
		public Widget()
		{
			mText = string.Empty;
			mMaxSize = new Size(9000, 9000);
		}

		/// <summary>
		/// Provides basic debugging information
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return base.ToString() + ": " + Name;
		}
		/// <summary>
		/// Gets the very lowest level control in the gui heirarchy.
		/// </summary>
		protected GuiRoot Root
		{
			get
			{
				if (Parent == null && this is GuiRoot)
					return (GuiRoot)this;
				else if (Parent == null)
					// this is for an object which has not been added to 
					// a container yet.
					return null;
				else
					return Parent.Root;
			}
		}

		/// <summary>
		/// Returns the WidgetCache object used by the theming engine.
		/// </summary>
		public Cache.WidgetCache Cache { get; set; }

		/// <summary>
		/// Gets whether or not this widget can have keyboard focus.
		/// </summary>
		public virtual bool CanHaveFocus { get { return false; } }
		/// <summary>
		/// Gets whether or not this widget has the keyboard focus.
		/// </summary>
		public bool HasFocus
		{
			get
			{
				if (Root == null)
					return false;
				else
					return Root.FocusControl == this;
			}
			set
			{
				Root.FocusControl = this;
			}
		}
		/// <summary>
		/// Gets or sets the text for this widget.
		/// When overriding, be sure to call base.Text to set the text.
		/// </summary>
		public virtual string Text
		{
			get { return mText; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("Text may not be null.");

				mText = value;
				RecalcSizeRange();
			}
		}

		/// <summary>
		/// Gets or sets the name of this widget.  This is mainly used
		/// for debugging information.
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets tool tip text for this widget.
		/// </summary>
		public string TooltipText { get; set; }
		/// <summary>
		/// Gets or sets a value indicating how this widget
		/// should share space with others when automatic layout is performed.
		/// </summary>
		public LayoutExpand LayoutExpand
		{
			get { return mLayoutExpand; }
			set
			{
				mLayoutExpand = value;

				if (Parent != null)
					Parent.RedoLayout();
			}
		}

		/// <summary>
		/// Gets the parent of this control.
		/// </summary>
		public Container Parent
		{
			get { return mParent; }
			internal set
			{
				mParent = value;
				OnParentChanged();
			}
		}

		/// <summary>
		/// Called when this control has a new parent.
		/// </summary>
		protected virtual void OnParentChanged()
		{
			RecalcSizeRange();
		}
		/// <summary>
		/// Called when this control should recalculate its minimum and maximum size values.
		/// </summary>
		protected internal virtual void RecalcSizeRange()
		{
			if (Root == null)
				return;
			if (suspendLayout)
				return;

			try
			{
				suspendLayout = true;

				if (AutoCalcMinSize)
					MinSize = Root.ThemeEngine.CalcMinSize(this);

				if (AutoCalcMaxSize)
					MaxSize = Root.ThemeEngine.CalcMaxSize(this);
			}
			finally
			{
				suspendLayout = false;

				Parent.RedoLayout();
			}
		}

		/// <summary>
		/// Gets or sets a boolean value indicating whether or not this widget should be visible
		/// at runtime.
		/// </summary>
		public bool Visible
		{
			get { return mVisible; }
			set
			{
				if (value == mVisible)
					return;

				mVisible = value;

				if (Parent != null)
					Parent.RedoLayout();
			}
		}
		/// <summary>
		/// Gets or sets a boolean value indicating whether or not the user should
		/// be allowed to interact with this widget.  Widgets which are disabled
		/// are displayed as "grayed-out." 
		/// </summary>
		public bool Enabled
		{
			get { return mEnabled; }
			set
			{
				mEnabled = value;
				OnEnabledChanged();
			}
		}

		/// <summary>
		/// Gets or sets the location of this widget.  Do not set this manually
		/// if using automatic layout.
		/// </summary>
		public Point Location
		{
			get { return mRegion.Location; }
			set { mRegion.Location = value; }
		}
		/// <summary>
		/// Gets or sets the size of this widget.  Do not set this manually if
		/// using automatic layout.
		/// </summary>
		public Size Size
		{
			get { return mRegion.Size; }
			set
			{
				if (mRegion.Size.Width == value.Width &&
					mRegion.Size.Height == value.Height)
					return;

				if (value.Width < MinSize.Width ||
					value.Height < MinSize.Height)
					throw new AgateGuiException("Cannot make widget smaller than its MinSize.");
				if (value.Width > MaxSize.Width ||
					value.Height > MaxSize.Height)
					throw new AgateGuiException("Cannot make widget larget than its MaxSize.");

				mRegion.Size = value;
				OnResizePrivate();
			}
		}

		/// <summary>
		/// Gets the rectangle containing this widget.
		/// </summary>
		protected internal Rectangle Region
		{
			get { return mRegion; }
		}

		/// <summary>
		/// Gets the width of this widget.
		/// </summary>
		public int Width
		{
			get { return Size.Width; }
		}
		/// <summary>
		/// Gets the height of this widget.
		/// </summary>
		public int Height
		{
			get { return Size.Height; }
		}

		/// <summary>
		/// Gets or sets the minimum size of this widget.
		/// </summary>
		public Size MinSize
		{
			get { return mMinSize; }
			set
			{
				mMinSize = value;

				if (this == Root) return;
				if (suspendLayout == false && Root != null)
					Parent.RedoLayout();
			}
		}
		/// <summary>
		/// Gets or sets the maximum size of this widget.
		/// </summary>
		public Size MaxSize
		{
			get { return mMaxSize; }
			set
			{
				if (value.Width < 1 || value.Height < 1)
					throw new ArgumentException();

				mMaxSize = value;

				if (this == Root) return;
				if (suspendLayout == false && Root != null)
					Parent.RedoLayout();
			}
		}
		/// <summary>
		/// Gets or sets a bool indicating whether or not the theming engine 
		/// should determine the minimum size of the widget.
		/// </summary>
		public bool AutoCalcMinSize
		{
			get { return mAutoCalcMinSize; }
			set
			{
				mAutoCalcMinSize = value;

				if (value == true)
					RecalcSizeRange();

				Parent.RedoLayout();
			}
		}
		/// <summary>
		/// Gets or sets a bool indicating whether or not the theming engine 
		/// should determine the maximum size of the widget.
		/// </summary>
		public bool AutoCalcMaxSize
		{
			get { return mAutoCalcMinSize; }
			set
			{
				mAutoCalcMaxSize = value;

				if (value == true)
					RecalcSizeRange();

				Parent.RedoLayout();
			}
		}

		bool IsDirty { get { return mDirtyRects.Count != 0; } }
		List<Rectangle> mDirtyRects = new List<Rectangle>();

		/// <summary>
		/// Tells the widget it should redraw itself.
		/// </summary>
		public virtual void Invalidate()
		{
			mDirtyRects.Clear();
			mDirtyRects.Add(Region);

			if (Parent != null)
				Parent.Invalidate(mRegion);
		}
		/// <summary>
		/// Tells the widget it should redraw itself.
		/// </summary>
		/// <param name="region">The region which should be redrawn</param>
		public void Invalidate(Rectangle region)
		{
			for (int i = 0; i < mDirtyRects.Count; i++)
			{
				if (Rectangle.Intersect(mDirtyRects[i], region) == region)
					return;
			}

			mDirtyRects.Add(region);

			if (Parent != null)
				Parent.Invalidate(mRegion);
		}

		private void OnResizePrivate()
		{
			mDirtyRects.Add(mRegion);
			OnResize();
		}
		/// <summary>
		/// Function called to raise the Resize event.
		/// </summary>
		protected virtual void OnResize()
		{
			if (Resize != null)
				Resize(this, EventArgs.Empty);

		}
		/// <summary>
		/// Indicates whether or not this widget should accept keyboard
		/// focus when the mouse button is pressed in it.
		/// </summary>
		protected internal virtual bool AcceptFocusOnMouseDown
		{
			get { return true; }
		}

		/// <summary>
		/// Event which is raised when the widget is resized.
		/// </summary>
		public event EventHandler Resize;

		/// <summary>
		/// Function called to draw the widget.
		/// </summary>
		public void Draw()
		{
			DoDraw();
		}
		/// <summary>
		/// Override to do actual drawing commands.  BeginFrame will already be set.
		/// </summary>
		protected virtual void DoDraw()
		{
			Root.ThemeEngine.DrawWidget(this);
		}

		protected internal virtual void UpdateGui()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p">A point in client coordinates</param>
		/// <returns></returns>
		public virtual Point PointToScreen(Point p)
		{
			if (Parent == null && this is GuiRoot)
				return p;
			else if (Parent == null)
				throw new InvalidOperationException();

			return Parent.PointToScreen(
				new Point(
					p.X + Location.X,
					p.Y + Location.Y));
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="p">A point in window coordinates</param>
		/// <returns></returns>
		public virtual Point PointToClient(Point p)
		{
			if (Parent == null && this is GuiRoot)
				return p;
			else if (Parent == null)
				throw new InvalidOperationException();

			Point parentPoint = Parent.PointToClient(p);

			return new Point(parentPoint.X - Location.X, parentPoint.Y - Location.Y);
		}

		internal bool HitTest(Point screenLocation)
		{
			return Root.ThemeEngine.HitTest(this, screenLocation);
		}

		internal Point ScreenLocation
		{
			get
			{
				if (this is GuiRoot)
					return Location;

				return Parent.PointToScreen(Location);
			}
		}

		internal int ThemeMargin
		{
			get { return Root.ThemeEngine.ThemeMargin(this); }
		}

		/// <summary>
		/// Gets a value indicating whether or not the mouse pointer is inside this widget.
		/// </summary>
		public bool MouseIn { get; protected set; }
		/// <summary>
		/// Gets a value indicating whether or not the mouse button was pressed inside this widget.
		/// </summary>
		public bool MouseDownIn { get { return mMouseDownIn; } }

		protected internal virtual void SendMouseDown(InputEventArgs e)
		{
			if (Enabled == false)
				return;

			mMouseDownIn = true;
			OnMouseDown(e);

			if (Root != null)
			{
				Root.ThemeEngine.MouseDownInWidget(this, PointToClient(e.MousePosition));
			}
		}
		protected internal virtual void SendMouseUp(InputEventArgs e) 
		{
			if (Enabled == false)
				return;

			mMouseDownIn = false;
			OnMouseUp(e);

			if (Root != null)
			{
				Root.ThemeEngine.MouseUpInWidget(this, PointToClient(e.MousePosition));
			}
		}
		protected internal virtual void SendMouseMove(InputEventArgs e)
		{
			if (Enabled == false)
				return;

			OnMouseMove(e);

			if (Root != null)
			{
				Root.ThemeEngine.MouseMoveInWidget(this, PointToClient(e.MousePosition));
			}
		}
		protected internal virtual void SendMouseDoubleClick(InputEventArgs e)
		{
			if (Enabled == false)
				return;
			OnMouseDoubleClick(e);
		}
		protected internal virtual void SendKeyDown(InputEventArgs e) { OnKeyDown(e); }
		protected internal virtual void SendKeyUp(InputEventArgs e) { OnKeyUp(e); }
		/// <summary>
		/// Sends the MouseEnter event to this control.
		/// If overriding this, be sure to set MouseIn to true, and provide
		/// some logic for calling OnMouseEnter on the control.
		/// </summary>
		protected internal virtual void SendMouseEnter()
		{
			MouseIn = true; 
			if (Enabled == false)
				return; 
			OnMouseEnter();
		}
		/// <summary>
		/// Sends the MouseLeave event to this control.
		/// If overriding this, be sure to set MouseIn to false, and provide
		/// some logic for calling OnMouseLeave on the control.
		/// 
		/// </summary>
		protected internal virtual void SendMouseLeave()
		{
			MouseIn = false;
			if (Enabled == false)
				return;
			OnMouseLeave();
		}

		/// <summary>
		/// Sets the dirty flag in the cache so that it is updated on the next
		/// frame.
		/// </summary>
		protected void SetDirty()
		{
			if (Cache == null)
				return;

			Cache.Dirty = true;
		}

		protected internal virtual void OnMouseDown(InputEventArgs e) { }
		protected internal virtual void OnMouseUp(InputEventArgs e) { }
		protected internal virtual void OnMouseMove(InputEventArgs e) { }
		protected internal virtual void OnMouseDoubleClick(InputEventArgs e) { }
		protected internal virtual void OnKeyDown(InputEventArgs e) { }
		protected internal virtual void OnKeyUp(InputEventArgs e) { }
		protected internal virtual void OnMouseEnter() { }
		protected internal virtual void OnMouseLeave() { }

		protected internal virtual void OnEnabledChanged() { SetDirty(); }

		public bool ContainsScreenPoint(Point screenMousePoint)
		{
			return new Rectangle(ScreenLocation, Size).Contains(screenMousePoint);
		}

		protected internal virtual void OnGainFocus()
		{
		}
		protected internal virtual void OnLoseFocus()
		{
		}

		protected internal virtual bool AcceptInputKey(KeyCode keyCode)
		{
			return false;
		}

	}
}