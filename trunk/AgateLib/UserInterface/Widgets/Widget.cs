using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System.ComponentModel;

namespace AgateLib.UserInterface.Widgets
{
	public class Widget
	{
		private Point mClientWidgetOffset;
		private Size mWidgetSize;

		private Rectangle mClientRect;
		private Font mFont;
		private Color? mFontColor;
		private bool mEnabled = true;
		string mStyle = string.Empty;

		public Widget()
		{
			Visible = true;
			AutoSize = true;
			LayoutDirty = true;
		}

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
			//set
			//{
			//	mClientRect.X = value.X + mClientWidgetOffset.X;
			//	mClientRect.Y = value.Y + mClientWidgetOffset.Y;
			//	mWidgetRect = value; 
			//}
		}
		public Point ClientWidgetOffset
		{
			get { return mClientWidgetOffset; }
			set { mClientWidgetOffset = value; }
		}
		public Size WidgetSize
		{
			get { return mWidgetSize; }
			set { mWidgetSize = value; }
		}

		public override string ToString()
		{
			return this.GetType().Name + ": " + Name;
		}

		public string Name { get; set; }
		public int X { get { return mClientRect.X; } set { mClientRect.X = value; } }
		public int Y { get { return mClientRect.Y; } set { mClientRect.Y = value; } }
		public int Width { get { return mClientRect.Width; } set { mClientRect.Width = value; } }
		public int Height { get { return mClientRect.Height; } set { mClientRect.Height = value; } }
		public object Tag { get; set; }

		public bool Enabled
		{
			get { return mEnabled; }
			set { mEnabled = value; }
		}

		public bool AutoSize { get; set; }

		public virtual Container Parent { get; set; }
		public Font Font
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
		public Color FontColor
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
		private AgateLib.DisplayLib.Font FindParentFont()
		{
			Widget parent = Parent;

			if (parent == null)
				return null;

			while (parent.Font == null && parent.Parent != null)
				parent = parent.Parent;

			return parent.Font;
		}

		private AgateLib.Geometry.Color FindParentFontColor()
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
				return AgateLib.Geometry.Color.Magenta;
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
				return clientPoint;

			Point translated = ClientToParent(clientPoint);

			return Parent.ClientToScreen(translated);
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

		public virtual void Update(double delta_t, ref bool processInput)
		{

		}
		public void Draw()
		{
			if (Visible == false)
				return;

			DrawImpl();
		}
		public void Draw(Rectangle parentClient)
		{
			if (Visible == false)
				return;

			DrawImpl();
		}
		public virtual void DrawImpl()
		{ }

		internal virtual Size ComputeSize(int? minWidth, int? minHeight, int? maxWidth, int? maxHeight)
		{
			return new Size(0, 0);
		}
		internal virtual void DoAutoSize()
		{
			if (AutoSize == false)
				return;

			Size sz = ComputeSize(null, null, null, null);

			Width = sz.Width;
			Height = sz.Height;
		}

		public virtual void Refresh()
		{ }

		public virtual bool Visible { get; set; }

		public string Style
		{
			get { return mStyle; }
			set
			{
				if (value == null)
					throw new ArgumentNullException();

				mStyle = value;
			}
		}

		public virtual bool LayoutDirty { get; set; }

		protected virtual Gui MyGui
		{
			get
			{
				if (Parent != null)
					return Parent.MyGui;
				else
					return null;
			}
		}

		#region --- Mouse Events ---

		bool mMouseIn;
		MouseButton mMouseButtons;

		public bool MouseIn { get { return mMouseIn; } internal set { mMouseIn = value; } }

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
			MyGui.FocusWidget = this;
		}
		public bool HasFocus { get { return MyGui.FocusWidget == this; } }


		protected internal virtual void OnUpdate(double deltaTime)
		{
		}
	}
}
