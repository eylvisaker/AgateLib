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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.Gui
{

    public class ResizeEventArgs : EventArgs
    {
        private Size mOldSize;
        private Size mNewSize;

        public ResizeEventArgs(Size old_size, Size new_size)
        {
            mOldSize = old_size;
            mNewSize = new_size;
        }
        public Size OldSize
        {
            get { return mOldSize; }
        }
        public Size NewSize
        {
            get { return mNewSize; }
        }

    }
    public class MoveEventArgs : EventArgs
    {
        private Point mOldLocation;
        private Point mNewLocation;

        public MoveEventArgs(Point oldPoint, Point newPoint)
        {
            mOldLocation = oldPoint;
            mNewLocation = newPoint;
        }
        public Point OldLocation
        {
            get { return mOldLocation; }
        }
        public Point NewLocation
        {
            get { return mNewLocation; }
        }
    }
    /*
    class Swap
    {
        public static void DoSwap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
    }

    [Flags]
    public enum MouseButtons
    {
        None = 0,
        Left = 0x01,
        Middle = 0x02,
        Right = 0x04,

        XButton1 = 0x08,
        XButton2 = 0x10,
        XButton3 = 0x20,
        XButton4 = 0x40,
    }

    public class MouseEventArgs : EventArgs
    {
        private Point mLocation;
        private MouseButtons mButtons;
        private int mClicks;
        private int mWheelDelta;

        public MouseEventArgs(Point loc)
        {
            mLocation = loc;
        }
        public Point Location
        {
            get { return mLocation; }
        }
        public MouseButtons Buttons
        {
            get { return mButtons; }
            set { mButtons = value; }
        }
        public int Clicks
        {
            get { return mClicks; }
            set { mClicks = value; }
        }
        public int WheelDelta
        {
            get { return mWheelDelta; }
            set { mWheelDelta = value; }
        }
        
    }
    public class KeyEventArgs : EventArgs
    {
        private bool mAlt = false;
        private bool mControl = false;
        private bool mShift = false;
        private bool mHandled = false;
        private SDL.Key mKey;
        private char mKeyChar;

        public bool Alt { get { return mAlt; } set { mAlt = value; } }
        public bool Control { get { return mControl; } set { mControl = value; } }
        public bool Shift { get { return mShift; } set { mShift = value; } }
        public bool Handled { get { return mHandled; } set { mHandled = value; } }
        public char KeyChar
        {
            get { return mKeyChar; }
            set { mKeyChar = value; }
        }
        public SDL.Key Key 
        {
            get { return mKey; }
            set
            { 
                mKey = value;
            } 
        }
        

    }

    public delegate void MoveEventHandler(object sender, MoveEventArgs e);
    public delegate void MouseEventHandler(object sender, MouseEventArgs e);
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);
    */
    [Flags]
    public enum Anchor
    {
        Top = 0x01,
        Left = 0x02,
        Right = 0x04,
        Bottom = 0x08,

    }

    public abstract class Component : IDisposable
    {
        #region --- Component data ---

        // private data
        private Rectangle mBounds = new Rectangle(0, 0, 50, 40);
        private Container mParent = null;
        private bool mEnabled = true;
        private bool mVisible = true;
        private Anchor mAnchor = Anchor.Top | Anchor.Left;
        private bool mHasFocus = false;
        private int mFocusChild = -1;
        private Color mBackColor = Color.White;
        private Color mForeColor = Color.Black;
        private bool mAutoSize = true;

        // protected data
        protected bool mCanHaveFocus = true;
        protected ComponentStyle mStyle;
        protected StyleManager mStyleManager;

        protected bool mMouseIn = false;

        #endregion

        #region --- Construction / Destruction ---

        /// <summary>
        /// Initialize a new Component object.
        /// </summary>
        public Component()
        {
        }
        /// <summary>
        /// Initialize a new Component object.
        /// </summary>
        /// <param name="parent">The Component containing this Component.</param>
        public Component(Container parent, bool autoSize)             
        {
            mAutoSize = autoSize;
            parent.AddChild(this);
        }


        /// <summary>
        /// Initialize a new Component object.
        /// </summary>
        /// <param name="parent">The Component containing this Component.</param>
        /// <param name="bounds">The boundary rectangle for this component.</param>
        public Component(Container parent, Rectangle bounds)
            : this(parent, false)
        {
            Bounds = bounds;
        }

        protected void LoadStyle()
        {
            if (ConnectToStyle)
            {
                mStyleManager = mParent.GetStyleManager();
                mStyleManager.ConnectStyle(this.GetType(), this);

                if (mAutoSize)
                    mStyle.DoAutoSize();
            }

        }
        /// <summary>
        /// Disposes the control and all children.
        /// </summary>
        public virtual void Dispose()
        {
            mParent.RemoveChild(this);
        }

        #endregion 

        #region --- Display and layout properties ---

        public virtual bool AutoSize
        {
            get { return mAutoSize; }
            set { mAutoSize = value; }
        }
        
        public virtual Rectangle Bounds
        {
            get { return mBounds; }
            set
            {
                Size = value.Size;
                Location = value.Location;
            }
        }
        public virtual Size Size
        {
            get { return mBounds.Size; }
            set
            {
                Size old = mBounds.Size;
                
                mBounds.Width = value.Width;
                mBounds.Height = value.Height;

                if (SizeChanged != null)
                {
                    if (old.Width != value.Width || old.Height != value.Height)
                    {
                        SizeChanged(this, new ResizeEventArgs(old, value));
                    }
                }
            }
        }
        public int Height
        {
            get { return Size.Height; }
            set { Size = new Size(Width, value); }
        }
        public int Width
        {
            get { return Size.Width; }
            set { Size = new Size(value, Height); }
        }

        public virtual Point Location
        {
            get { return mBounds.Location; }
            set
            {
                Point old = mBounds.Location;

                mBounds.X = value.X;
                mBounds.Y = value.Y;

                if (LocationChanged != null)
                {
                    if (old.X != value.X || old.Y != value.Y)
                    {
                        LocationChanged(this, new MoveEventArgs(old, value));
                    }
                }
            }
        }
        public Color BackColor
        {
            get { return mBackColor; }
            set { mBackColor = value; }
        }
        public Color ForeColor
        {
            get { return mForeColor; }
            set { mForeColor = value; }
        }

        public bool MouseIn
        {
            get { return mMouseIn; }
        }

        /// <summary>
        /// True if the user can interact with this control.
        /// </summary>
        public bool Enabled
        {
            get { return mEnabled; }
            set { mEnabled = value; }
        }

        /// <summary>
        /// True if this control should be drawn on screen.  If false,
        /// this control will not fire Drawing events.
        /// </summary>
        public bool Visible
        {
            get { return mVisible; }
            set { mVisible = value; }
        }

        /// <summary>
        /// Indicates how this control should be resized/moved if the parent
        /// control is resized.
        /// </summary>
        public Anchor Anchor
        {
            get { return mAnchor; }
            set { mAnchor = value; }
        }

        /// <summary>
        /// Returns whether or not this control can have user input focus.
        /// </summary>
        public bool IsFocusable
        {
            get { return mCanHaveFocus; }
        }

        /// <summary>
        /// Returns whether or not this control currently has user input focus.
        /// </summary>
        public bool HasFocus
        {
            get { return mHasFocus; }
        }

        /// <summary>
        /// Gets whether or not to automatically connect to a style when the component is
        /// initialized.  This always returns true; it must be overriden to return false.
        /// </summary>
        protected internal virtual bool ConnectToStyle
        {
            get { return true; }
        }

       
        /// <summary>
        /// Sets focus on this control.
        /// </summary>
        public void SetFocus()
        {
            // if we can't set focus on this control, just exit
            // maybe this should throw an exception??
            if (!mCanHaveFocus)
                throw new Exception("Error: this component cannot have focus.");

            // tell parent to lose focus on all other children.
            mParent.ChildHasFocus(this);


            mHasFocus = true;


            // fire the event.
            if (GotFocus != null)
                GotFocus(this, EventArgs.Empty);
        }
        /// <summary>
        /// Causes this control to lose focus (and all its children).
        /// </summary>
        protected internal void LoseFocus()
        {
            mHasFocus = false;

            if (LostFocus != null)
                LostFocus(this, EventArgs.Empty);
        }
        

        #endregion
        #region --- Coordinate transforms to the screen and back ---

        protected virtual Point ClientToParent(Point localCoord)
        {
            return new Point(mBounds.X + localCoord.X,
                             mBounds.Y + localCoord.Y);
        }
        protected virtual Point ParentToClient(Point parentCoord)
        {
            return new Point(parentCoord.X - mBounds.X,
                             parentCoord.Y - mBounds.Y);
        }

        public Point ClientToScreen(int x, int y)
        {
            return ClientToScreen(new Point(x, y));
        }
        public Point ClientToScreen(Point localCoord)
        {
            Component p = this;
            Point retval = localCoord;

            while (p != null)
            {
                retval = p.ClientToParent(retval);
                p = p.Parent;
            }

            return retval;
        }
        public Rectangle ClientToScreen(Rectangle localRect)
        {
            Rectangle retval = new Rectangle(localRect.Location, localRect.Size);

            retval.Location = ClientToScreen(localRect.Location);

            return retval;
        }
        public Point ScreenToClient(int x, int y)
        {
            return ScreenToClient(new Point(x, y));
        }
        public Point ScreenToClient(Point screenCoord)
        {
            List<Component> parentTree = new List<Component>();

            Component p = this;
            Point retval = screenCoord;

            while (p != null)
            {
                parentTree.Add(p);
                p = p.Parent;
            }

            
            for (int i = 0; i < parentTree.Count; i++)
            {
                retval = parentTree[i].ParentToClient(retval);
            }

            return retval;
        }
        public Rectangle ScreenToClient(Rectangle screenRect)
        {
            Rectangle retval = new Rectangle(screenRect.Location, screenRect.Size);

            retval.Location = ScreenToClient(screenRect.Location);

            return retval;

        }

        public Point ScreenLocation
        {
            get { return Parent.ClientToScreen(Location); }
        }
        public Rectangle ScreenBounds
        {
            get { return Parent.ClientToScreen(Bounds); }
        }

        #endregion
        #region --- Finding and checking Parents and Children ---

        /// <summary>
        /// Returns the control directly above this one.
        /// </summary>
        internal virtual Container Parent
        {
            get { return mParent; }
            set
            {
                mParent = value;

                LoadStyle();
            }
        }

        /// <summary>
        /// Returns the top most control containing this one.  Usually
        /// it's the GUI manager.
        /// </summary>
        public virtual Component TopParent
        {
            get
            {
                Component p = this;

                while (p.Parent != null)
                    p = p.Parent;

                return p;
            }
        }


        protected void CheckAutoSize()
        {
            if (AutoSize)
                mStyle.DoAutoSize();
        }

        /// <summary>
        /// Checks to see if the passed control is a parent of this control, or any of its parents.
        /// </summary>
        /// <param name="test">The component to test.</param>
        /// <returns>True if the passed component is the parent of this control or any of its parents.
        /// False otherwise.  Also returns false if test is null.</returns>
        public bool IsAnyParent(Container test)
        {
            if (test == null)
                return false;

            if (mParent == test) return true;
            else if (mParent != null) return mParent.IsAnyParent(test);
            else return false;
        }


        #endregion

        #region --- Methods for affecting draw order --

        /// <summary>
        /// Raises this control in the parent's draw order, bringing it closer to
        /// the front of the display
        /// </summary>
        public void Raise()
        {
            mParent.RaiseChild(this);
        }

        /// <summary>
        /// Lowers this control in the parent's draw order, pushing it back.
        /// </summary>
        public void Lower()
        {
            mParent.LowerChild(this);
        }

        /// <summary>
        /// Brings this control to the front of its parent's draw order, so it
        /// is drawn on top of other controls.
        /// </summary>
        public void BringToFront()
        {
            mParent.BringChildToFront(this);
        }
        /// <summary>
        /// Sends this control to the back of its parent's draw order, so other
        /// controls are drawn on top of it.
        /// </summary>
        public void SendToBack()
        {
            mParent.SendChildToBack(this);
        }

        #endregion

        #region --- Drawing ---

        /// <summary>
        /// Updates the control.  Called once each frame.
        /// </summary>
        protected internal virtual void Update()
        {
        }

        /// <summary>
        /// Draws this component and all its children.
        /// </summary>
        public virtual void Draw()
        {
            if (Paint != null)
            {
                PaintBegin(this, EventArgs.Empty);
                Paint(this, EventArgs.Empty);
                PaintEnd(this, EventArgs.Empty);
            }

        }

        public StyleManager GetStyleManager()
        {
            if (mStyleManager == null && Parent != null)
            {
                mStyleManager = Parent.GetStyleManager();
                return mStyleManager;
            }
            else
                return mStyleManager;
        }
        public void SetStyleManager(StyleManager man)
        {
            mStyleManager = man;
        }

        public void AttachStyle(ComponentStyle style)
        {
            mStyle = style;
            mStyle.Attach(this);
        }

        #endregion

        #region --- Event handlers ---

        internal virtual void OnMouseMove(InputEventArgs e)
        {
            if (MouseMove != null)
                MouseMove(this, e);
        }
        internal virtual void OnMouseDown(InputEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(this, e);
        }
        internal virtual void OnMouseUp(InputEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(this, e);
        }
        internal virtual void OnMouseEnter(InputEventArgs e)
        {
            mMouseIn = true;

            if (MouseEnter != null)
                MouseEnter(this, e);
        }
        internal virtual void OnMouseLeave(InputEventArgs e)
        {
            mMouseIn = false;

            if (MouseLeave != null)
                MouseLeave(this, e);
        }
        internal virtual void OnMouseClick(InputEventArgs e)
        {
            if (MouseClick != null)
                MouseClick(this, e);
        }
        internal virtual void OnMouseHover(InputEventArgs e)
        {
            if (MouseHover != null)
                MouseHover(this, e);
        }

        internal virtual void OnKeyDown(InputEventArgs e)
        {
            if (KeyDown != null)
                KeyDown(this, e);
        }
        internal virtual void OnKeyUp(InputEventArgs e)
        {
            if (KeyUp != null)
                KeyUp(this, e);
        }

        #endregion
        #region --- Component events ---


        public delegate void ResizeEventHandler(object sender, ResizeEventArgs e);
        public delegate void MoveEventHandler(object sender, MoveEventArgs e);
        public delegate void GuiInputEventHandler(object sender, InputEventArgs e);

        /// <summary>
        /// This event occurs when the size of the control is changed.  The
        /// display style should hook to this in order to resize the client area.
        /// </summary>
        public virtual event ResizeEventHandler SizeChanged;
        /// <summary>
        /// Occurs when the location of the control is changed.
        /// </summary>
        public virtual event MoveEventHandler LocationChanged;

        public virtual event ResizeEventHandler ClientAreaSizeChanged;
        public virtual event MoveEventHandler ClientAreaLocationChanged;

        public virtual event EventHandler PaintBegin;
        public virtual event EventHandler Paint;
        public virtual event EventHandler PaintEnd;

        public virtual event EventHandler GotFocus;
        public virtual event EventHandler LostFocus;

        public virtual event GuiInputEventHandler MouseEnter;
        public virtual event GuiInputEventHandler MouseLeave;
        public virtual event GuiInputEventHandler MouseMove;
        public virtual event GuiInputEventHandler MouseDown;
        public virtual event GuiInputEventHandler MouseUp;
        public virtual event GuiInputEventHandler MouseClick;
        public virtual event GuiInputEventHandler MouseHover;

        public virtual event GuiInputEventHandler KeyDown;
        public virtual event GuiInputEventHandler KeyUp;

        public virtual event EventHandler Close;

        #endregion
    }

}
