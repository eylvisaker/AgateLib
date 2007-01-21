using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.GuiBase
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
        private Rectangle mClientArea;
        private Rectangle mBounds;
        private Component mParent = null;
        private List<Component> mChildren = new List<Component>();
        private bool mEnabled = true;
        private bool mVisible = true;
        private Anchor mAnchor = Anchor.Top | Anchor.Left;
        private bool mHasFocus = false;
        private int mFocusChild = -1;

        // protected data
        protected bool mFocusable = true;
        protected ComponentStyle mStyle;
        protected StyleManager mStyleManager;
        protected string mComponentType;

        protected bool mMouseIn = false;

        #endregion

        #region --- Construction / Destruction ---

        /// <summary>
        /// Initialize a new Component object.
        /// </summary>
        public Component(string componentType)
        {
            mComponentType = componentType;
        }
        /// <summary>
        /// Initialize a new Component object.
        /// </summary>
        /// <param name="parent">The Component containing this Component.</param>
        public Component(string componentType, Component parent) 
            : this(componentType)
        {
            mParent = parent;
            mParent.AddChild(this);

            LoadStyle();
           
        }


        /// <summary>
        /// Initialize a new Component object.
        /// </summary>
        /// <param name="parent">The Component containing this Component.</param>
        /// <param name="bounds">The boundary rectangle for this component.</param>
        public Component(string componentType, Component parent, Rectangle bounds)
            : this(componentType, parent)
        {
            mBounds = bounds;

            mClientArea = new Rectangle(new Point(0, 0), mBounds.Size);

        }

        protected void LoadStyle()
        {
            mStyleManager = mParent.GetStyleManager();
            mStyleManager.ConnectStyle(mComponentType, this);

        }
        /// <summary>
        /// Disposes the control and all children.
        /// </summary>
        public virtual void Dispose()
        {
            for (int i = 0; i < mChildren.Count; i++)
            {
                mChildren[i].Dispose();
            }

            mParent.RemoveChild(this);
        }

        #endregion 

        #region --- Display and layout ---

        public virtual Rectangle ClientArea
        {
            get { return mClientArea; }
            set 
            {
                Rectangle old = mClientArea;
                mClientArea = value;

                if (ClientAreaSizeChanged != null)
                {
                    if (old.Width != value.Width || old.Height != value.Height)
                    {
                        ClientAreaSizeChanged(this, new ResizeEventArgs(old.Size, value.Size));
                    }
                }

                if (ClientAreaLocationChanged != null)
                {
                    if (old.X != value.X || old.Y != value.Y)
                    {
                        ClientAreaLocationChanged(this, new MoveEventArgs(old.Location, value.Location));
                    }
                }
            }
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

                mClientArea = mBounds;

                if (SizeChanged != null)
                {
                    if (old.Width != value.Width || old.Height != value.Height)
                    {
                        SizeChanged(this, new ResizeEventArgs(old, value));
                    }
                }

                ResizeChildren(old);

            }
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
            get { return mFocusable; }
        }

        /// <summary>
        /// Returns whether or not this control currently has user input focus.
        /// </summary>
        public bool HasFocus
        {
            get { return mHasFocus; }
        }


        /// <summary>
        /// Resizes all children after a Size change event.
        /// </summary>
        /// <param name="oldSize">The old size of the control.  Used to update child positions
        /// based on what their Anchor property is.</param>
        protected void ResizeChildren(Size oldSize)
        {
            Size change = new Size(Size.Width - oldSize.Width, Size.Height - oldSize.Height);

            foreach (Component c in Children)
            {
                Anchor a = c.Anchor;
                int leftSpacing = c.Location.X;
                int topSpacing = c.Location.Y;
                int rightSpacing = oldSize.Width - c.Bounds.Right;
                int bottomSpacing = oldSize.Height - c.Bounds.Bottom;
                
                Rectangle newBounds = new Rectangle(c.Location, c.Size);

                // test left/right
                Anchor lr = a & (Anchor.Left | Anchor.Right);

                if (lr == Anchor.Left)
                {
                    // well, don't do anything here, since the left spot is staying where it's at.
                }
                else if (lr == Anchor.Right)
                {
                    // move the control, but don't resize it.
                    newBounds.X += change.Width;
                }
                else if (lr == (Anchor.Left | Anchor.Right))
                {
                    // now we must resize the control to keep the left spacing and right spacing the same
                    newBounds.Width = Size.Width - rightSpacing - leftSpacing;
                }
                else
                {
                    // move by half the change
                    // mod the second part to make sure the control is still moved by the
                    // same amount even if the size is changed by one pixel at a time.
                    newBounds.X += (change.Width / 2) + Math.Sign(change.Width) * (Size.Width % 2);
                }

                // test top.bottom
                Anchor tb = a & (Anchor.Top | Anchor.Bottom);

                if (tb == Anchor.Top)
                {
                    // again, nothing here.
                }
                else if (tb == Anchor.Bottom)
                {
                    // move but don't resize
                    newBounds.Y += change.Height;
                }
                else if (tb == (Anchor.Top | Anchor.Bottom))
                {
                    // now resize the control to keep top and bottom spacing the same
                    newBounds.Height = Size.Height - topSpacing - bottomSpacing;
                }
                else
                {
                    // move by half the change
                    // mod the second part to make sure the control is still moved by the
                    // same amount even if the size is changed by one pixel at a time.
                    newBounds.Y += (change.Height / 2) + Math.Sign(change.Height) * (Size.Height % 2);
                }

                c.Bounds = newBounds;
            }
        }

        /// <summary>
        /// Sets focus on this control.
        /// </summary>
        public void SetFocus()
        {
            // if we can't set focus on this control, just exit
            // maybe this should throw an exception??
            if (!mFocusable)
                return;

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
        protected void LoseFocus()
        {
            mHasFocus = false;

            if (LostFocus != null)
                LostFocus(this, EventArgs.Empty);
        }
        /// <summary>
        /// This control responds 
        /// </summary>
        /// <param name="c"></param>
        protected void ChildHasFocus(Component c)
        {
            SetFocus();

            for (int i = 0; i < mChildren.Count; i++)
            {
                if (c != mChildren[i])
                {
                    mChildren[i].LoseFocus();
                }
            }
        }

        #endregion
        #region --- Coordinate transforms to the screen and back ---

        protected Point ClientToParent(Point localCoord)
        {
            return new Point(mBounds.X + mClientArea.X + localCoord.X,
                             mBounds.Y + mClientArea.Y + localCoord.Y);
        }
        protected Point ParentToClient(Point parentCoord)
        {
            return new Point(parentCoord.X - mClientArea.X - mBounds.X,
                             parentCoord.Y - mClientArea.Y - mBounds.Y);
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

        #endregion
        #region --- Finding and checking Parents and Children ---

        /// <summary>
        /// Returns the control directly above this one.
        /// </summary>
        public virtual Component Parent
        {
            get { return mParent; }
            set { mParent = value; }
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

        /// <summary>
        /// Returns the list of child components belonging to this control.
        /// Do not use this to add children.  Instead use the AddChild method,
        /// or the constructor of the child control.
        /// </summary>
        public List<Component> Children
        {
            get { return mChildren; }
        }

        public void AddChild(Component child)
        {
            // make sure we are not adding a component which is a parent somewhere of this control
            if (IsAnyParent(child))
                throw new Exception("Cannot add as a child a control which is a parent to this control.");
            
            
            // don't add a null reference
            if (child == null)
                throw new Exception("Cannot add a null reference as a child.");

            mChildren.Add(child);
            child.mParent = this;
        }
        public void RemoveChild(Component child)
        {
            mChildren.Remove(child);
        }
        /// <summary>
        /// Returns whether or not the given component is a child of this component.
        /// Does not test recursivly within the children.  Use IsAnyChild for that functionality.
        /// </summary>
        /// <param name="test">The component to test.</param>
        /// <returns>Returns true if the passed component is a child of this control.</returns>
        public bool IsChild(Component test)
        {
            if (test == null)
                return false;

            for (int i = 0; i < mChildren.Count; i++)
            {
                if (mChildren[i] == test)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks to see if the passed control is a child of this control, or any of its children.
        /// </summary>
        /// <param name="test">The component to test</param>
        /// <returns>True if the passed component is a child of this control or recursively any
        /// child of this control.  Returns false otherwise, or if test is null.</returns>
        public bool IsAnyChild(Component test)
        {
            if (test == null)
                return false;

            for (int i = 0; i < mChildren.Count; i++)
            {
                if (mChildren[i] == test)
                    return true;
                else if (mChildren[i].IsAnyChild(test))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Checks to see if the passed control is a parent of this control, or any of its parents.
        /// </summary>
        /// <param name="test">The component to test.</param>
        /// <returns>True if the passed component is the parent of this control or any of its parents.
        /// False otherwise.  Also returns false if test is null.</returns>
        public bool IsAnyParent(Component test)
        {
            if (test == null)
                return false;

            if (mParent == test) return true;
            else if (mParent != null) return mParent.IsAnyParent(test);
            else return false;
        }

        /// <summary>
        /// Returns the component at the specified point within the client rectangle.
        /// </summary>
        /// <param name="x">X position in the client rect</param>
        /// <param name="y">Y position in the client rect</param>
        /// <returns>This object if there is no other component there.</returns>
        public Component GetComponentAt(int x, int y)
        {
            return GetComponentAt(new Point(x, y));
        }
        /// <summary>
        /// Returns the component at the specified point within the client rectangle.
        /// </summary>
        /// <param name="pt">position in the client rect to look at.</param>
        /// <returns>This object if there is no other component there.</returns>
        public Component GetComponentAt(Point pt)
        {
            for (int i = 0; i < mChildren.Count; i++)
            {
                if (mChildren[i].Bounds.Contains(pt))
                {
                    return mChildren[i].GetComponentAt(pt);
                }
            }

            return this;
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

        private void RaiseChild(Component c)
        {
            for (int i = 0; i < mChildren.Count; i++)
            {
                if (mChildren[i] == c)
                {
                    int j = i + 1;

                    if (j < mChildren.Count)
                    {
                        mChildren.RemoveAt(i);
                        mChildren.Insert(j, c);
                    }

                    return;
                }
            }

            throw new Exception("BUG: The passed component is not a child of this control!");
        }
        private void LowerChild(Component c)
        {
            for (int i = 0; i < mChildren.Count; i++)
            {
                if (mChildren[i] == c)
                {
                    int j = i - 1;

                    if (j >= 0)
                    {
                        mChildren.RemoveAt(i);
                        mChildren.Insert(j, c);
                    }

                    return;
                }
            }

            throw new Exception("BUG: The passed component is not a child of this control!");
        }
        private void BringChildToFront(Component c)
        {
            for (int i = 0; i < mChildren.Count; i++)
            {
                if (mChildren[i] == c)
                {
                    mChildren.RemoveAt(i);
                    mChildren.Add(c);

                    return;
                }
            }

            throw new Exception("BUG: The passed component is not a child of this control!");
        }
        private void SendChildToBack(Component c)
        {
            for (int i = 0; i < mChildren.Count; i++)
            {
                if (mChildren[i] == c)
                {
                    mChildren.RemoveAt(i);
                    mChildren.Insert(0, c);
                
                    return;
                }
            }

            throw new Exception("BUG: The passed component is not a child of this control!");
        }

        #endregion

        #region --- Drawing ---

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

            for (int i = 0; i < mChildren.Count; i++)
                mChildren[i].Draw();
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

        public void OnMouseMove(InputEventArgs e)
        {
            if (MouseMove != null)
                MouseMove(this, e);
        }
        public void OnMouseDown(InputEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(this, e);
        }
        public void OnMouseUp(InputEventArgs e)
        {
            if (MouseUp != null)
                MouseUp(this, e);
        }
        public void OnMouseEnter(InputEventArgs e)
        {
            mMouseIn = true;

            if (MouseEnter != null)
                MouseEnter(this, e);
        }
        public void OnMouseLeave(InputEventArgs e)
        {
            mMouseIn = false;

            if (MouseLeave != null)
                MouseLeave(this, e);
        }
        public void OnMouseClick(InputEventArgs e)
        {
            if (MouseClick != null)
                MouseClick(this, e);
        }
        public void OnMouseHover(InputEventArgs e)
        {
            if (MouseHover != null)
                MouseHover(this, e);
        }

        public void OnKeyDown(InputEventArgs e)
        {
            if (KeyDown != null)
                KeyDown(this, e);
        }
        public void OnKeyUp(InputEventArgs e)
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
