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
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform;
using AgateLib.Quality;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.StyleModel;

namespace AgateLib.UserInterface.Widgets
{
	public class Widget
	{
		static internal InputMode PreferredInputMode { get; set; }

		Widget mParentCoordinateSystem;
		private Point mClientWidgetOffset;
		private Size mWidgetSize;

		private Rectangle mClientRect;
		private IFont mFont;
		private Color? mFontColor;
		private bool mEnabled = true;
		string mStyle = string.Empty;
		bool mLayoutDirty;
		bool mVisible;

		public Widget()
		{
			WidgetStyle = new StyleModel.WidgetStyle(this);
			Visible = true;
			AutoSize = true;
			LayoutDirty = true;
		}

		protected internal virtual IEnumerable<Widget> RenderChildren => Enumerable.Empty<Widget>();
		protected internal virtual IEnumerable<Widget> LayoutChildren => Enumerable.Empty<Widget>();

		/// <summary>
		/// The rectangle in parent client coordinates that contains the client area of the widget.
		/// </summary>
		public Rectangle ClientRect
		{
			get { return mClientRect; }
			set { mClientRect = value; }
		}
		/// <summary>
		/// The rectangle in parent client coordinates that contains the widget, including its padding and border.
		/// This is the area in which the widget has mouse focus.
		/// </summary>
		public Rectangle WidgetRect
		{
			get
			{
				return new Rectangle(
					ClientRect.X - mClientWidgetOffset.X,
					ClientRect.Y - mClientWidgetOffset.Y,
					mWidgetSize.Width,
					mWidgetSize.Height);
			}
		}
		/// <summary>
		/// The amount the client area is shifted inside the widget rectangle.
		/// </summary>
		public Point ClientWidgetOffset
		{
			get { return mClientWidgetOffset; }
			set { mClientWidgetOffset = value; }
		}
		public Size WidgetSize
		{
			get { return mWidgetSize; }
			set
			{
				mWidgetSize = value;
			}
		}

		public override string ToString()
		{
			return this.GetType().Name + ": " + Name;
		}

		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the X position of the widget's client rect in its parent's coordinate system.
		/// </summary>
		public int X { get { return mClientRect.X; } set { mClientRect.X = value; } }
		/// <summary>
		/// Gets or sets the Y position of the widget's client rect in its parent's coordinate system.
		/// </summary>
		public int Y { get { return mClientRect.Y; } set { mClientRect.Y = value; } }
		/// <summary>
		/// Gets or sets the width of the client rect.
		/// </summary>
		public int Width { get { return mClientRect.Width; } set { mClientRect.Width = value; } }
		/// <summary>
		/// Gets or sets the height of the client rect.
		/// </summary>
		public int Height { get { return mClientRect.Height; } set { mClientRect.Height = value; } }
		public object Tag { get; set; }

		public bool Enabled
		{
			get { return mEnabled; }
			set { mEnabled = value; }
		}

		public bool AutoSize { get; set; }

		public virtual Widget Parent { get; set; }

		protected internal Widget ParentCoordinateSystem
		{
			get
			{
				if (mParentCoordinateSystem == null)
					return Parent;
				else
					return mParentCoordinateSystem;
			}
			internal set
			{
				mParentCoordinateSystem = value;
			}
		}

		public IFont Font
		{
			get
			{
				if (mFont != null)
					return mFont;

				return FindParentFont();
			}
			set
			{
				mFont = value;
			}
		}
		public virtual Color FontColor
		{
			get
			{
				Widget test = this;

				do
				{
					if (test.Enabled == false)
						return Color.Gray;

					test = test.Parent;

				} while (test != null);

				if (mFontColor.HasValue)
					return mFontColor.Value;

				return FindParentFontColor();
			}
			set
			{
				mFontColor = value;
			}
		}
		private IFont FindParentFont()
		{
			Widget parent = Parent;

			if (parent == null)
				return null;

			while (parent.Font == null && parent.Parent != null)
				parent = parent.Parent;

			return parent.Font;
		}

		private Color FindParentFontColor()
		{
			Widget parent = Parent;

			while (parent != null && parent is Window == false)
			{
				parent = parent.Parent;
			}

			if (parent is Window)
			{
				Window w = (Window)parent;

				return Color.Black;
			}
			else
				return Color.Magenta;
		}

		/// <summary>
		/// Converts the client rectangle into screen coordinates.
		/// </summary>
		/// <returns></returns>
		public Rectangle ClientToScreen()
		{
			Rectangle translated = ClientRect;

			translated.Location = ClientToScreen(Point.Zero);

			return translated;
		}
		public Rectangle ClientToScreen(Rectangle value)
		{
			Rectangle translated = value;

			translated.Location = ClientToScreen(value.Location);

			return translated;
		}
		public Point ClientToScreen(Point clientPoint)
		{
			if (Parent == null)
				return ClientToParent(clientPoint);

			Point translated = ClientToParent(clientPoint);

			return ParentCoordinateSystem.ClientToScreen(translated);
		}
		public Point ScreenToClient(Point screenPoint)
		{
			if (Parent == null)
				return screenPoint;

			Point translated = ParentToClient(screenPoint);

			return Parent.ScreenToClient(translated);
		}
		public Point ClientToParent(Point clientPoint)
		{
			Point translated = clientPoint;

			translated.X += X;
			translated.Y += Y;

			return translated;
		}
		public Point ParentToClient(Point parentClientPoint)
		{
			Point translated = parentClientPoint;

			translated.X -= X;
			translated.Y -= Y;

			return translated;
		}

		public virtual void Update(ClockTimeSpan elapsed, ref bool processInput)
		{

		}
		public void Draw()
		{
			if (Visible == false)
				return;

			DrawImpl(ClientToScreen(new Rectangle(Point.Zero, ClientRect.Size)));
		}
		public virtual void DrawImpl(Rectangle screenRect)
		{ }

		internal virtual Size ComputeSize(int? maxWidth, int? maxHeight)
		{
			return new Size(0, 0);
		}

		public virtual void Refresh()
		{ }

		public virtual bool Visible
		{
			get { return mVisible; }
			set
			{
				if (value == mVisible)
					return;

				mVisible = value;
				LayoutDirty = true;
			}
		}

		public string Style
		{
			get { return mStyle; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();
				if (value == mStyle)
					return;

				mStyle = value;
				LayoutDirty = true;
			}
		}

		public virtual bool LayoutDirty
		{
			get { return mLayoutDirty; }
			set
			{
				mLayoutDirty = value;
				if (value)
					StyleDirty = true;
			}
		}
		internal bool StyleDirty { get; set; }

		protected internal virtual FacetScene MyFacetScene
		{
			get
			{
				if (Parent != null)
					return Parent.MyFacetScene;
				else
					return null;
			}
		}

		#region --- Mouse Events ---

		bool mMouseIn;
		MouseButton mMouseButtons;

		public bool MouseIn
		{
			get { return mMouseIn; }
			internal set
			{
				if (value == mMouseIn)
					return;

				mMouseIn = value;

				if (value)
					OnMouseEnter();
				else
					OnMouseLeave();
			}
		}

		protected internal virtual void OnMouseMove(Point clientPoint)
		{
			//if (Parent == null)
			//	return;

			//Parent.OnMouseMove(ClientToParent(clientPoint));

			if (MouseMove != null)
				MouseMove(this, new MouseEventArgs(clientPoint, mMouseButtons));
		}

		protected internal virtual void OnMouseLeave()
		{
			if (MouseLeave != null)
				MouseLeave(this, EventArgs.Empty);
		}
		protected internal virtual void OnMouseEnter()
		{
			if (MouseEnter != null)
				MouseEnter(this, EventArgs.Empty);
		}

		protected internal virtual void OnMouseDown(MouseButton mouseButtons, Point point)
		{
			mMouseButtons = mouseButtons;

			if (MouseDown != null)
				MouseDown(this, new MouseEventArgs(point, mouseButtons));
		}
		protected internal virtual void OnMouseUp(MouseButton mouseButtons, Point point)
		{
			mMouseButtons = mouseButtons;

			if (MouseUp != null)
				MouseUp(this, new MouseEventArgs(point, mouseButtons));
		}

		public event EventHandler<MouseEventArgs> MouseDown;
		public event EventHandler<MouseEventArgs> MouseUp;
		public event EventHandler<MouseEventArgs> MouseMove;
		public event EventHandler MouseEnter;
		public event EventHandler MouseLeave;

		#endregion
		#region --- Keyboard Events ---

		protected internal virtual void OnKeyDown(KeyCode key, string keyString)
		{
			if (KeyDown != null)
				KeyDown(this, new KeyboardEventArgs(key, keyString));
		}
		protected internal virtual void OnKeyUp(KeyCode key, string keyString)
		{
			if (KeyUp != null)
				KeyUp(this, new KeyboardEventArgs(key, keyString));
		}

		protected internal virtual void OnGuiInput(GuiInput input, ref bool handled)
		{
		}

		public event EventHandler<KeyboardEventArgs> KeyDown;
		public event EventHandler<KeyboardEventArgs> KeyUp;

		#endregion

		public bool TabStop { get; set; }
		protected bool AcceptFocus { get; set; }

		protected internal virtual Widget FindFocusWidget()
		{
			if (AcceptFocus)
				return this;
			else
				return null;
		}

		public virtual void Focus()
		{
			MyFacetScene.FocusWidget = this;
		}
		public bool HasFocus { get { return MyFacetScene.FocusWidget == this; } }


		protected internal virtual void OnUpdate(ClockTimeSpan elapsed)
		{
		}

		protected internal virtual void OnGestureBegin(Gesture gesture)
		{
		}
		protected internal virtual void OnGestureComplete(Gesture gesture)
		{
		}
		protected internal virtual void OnGestureChange(Gesture gesture)
		{
		}

		protected internal virtual bool AcceptGestureInput { get { return false; } }
		/// <summary>
		/// Returns false by default. If this returns false, then gestures will be fixed to 
		/// either the vertical or horizontal axis.
		/// </summary>
		protected internal virtual bool AnyDirectionGestures { get { return false; } }

		internal WidgetStyle WidgetStyle { get; set; }

		public Point ClientLocationOf(Widget widget)
		{
			//Condition.Requires<ArgumentException>(Descendants.Contains(widget));

			var w = widget;
			Point pt = Point.Zero;

			while (w != null && w != this)
			{
				pt = w.ClientToParent(pt);
				w = w.Parent;
			}

			return pt;
		}
	}
}
