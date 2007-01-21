using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.GuiBase
{
   
    public class Container : Component
    {
        private List<Component> mMouseInControls = new List<Component>();
        private Point mLastMousePosition = new Point(0, 0);
        private Component mMouseDownControl = null;

        private List<Component> mChildren = new List<Component>();

        #region --- Construction / Destruction ---

        public Container()
        {
            this.SizeChanged += new ResizeEventHandler(Container_SizeChanged);
        }

        public override void Dispose()
        {
            for (int i = 0; i < mChildren.Count; i++)
            {
                mChildren[i].Dispose();
            }

            base.Dispose();
        }

        #endregion

        #region --- Event Handlers ---

        //void Container_MouseMove(object sender, InputEventArgs e)
        internal override void OnMouseMove(InputEventArgs e)
        {
            // If the mouse is already down in a control, then mouse motions 
            // are intercepted only by that control.
            if (mMouseDownControl == null)
            {
                // check for mouse leave events
                Component last = this.GetComponentAt(mLastMousePosition);

                // go through these for mouseLeave events
                while (last != this && !last.Bounds.Contains(e.MousePosition))
                {
                    last.OnMouseLeave(e);
                    last = last.Parent;
                }


                Component c = this.GetComponentAt(e.MousePosition);

                if (c == this)
                {
                    base.OnMouseMove(e);
                    return;
                }

                if (!c.MouseIn)
                {
                    c.OnMouseEnter(e);

                    // go through its parents to make sure they have a mouse in.
                    last = c.Parent;
                    while (last != this && !last.MouseIn)
                    {
                        last.OnMouseEnter(e);
                    }
                }

                mLastMousePosition = e.MousePosition;
            }
            else
            {
                Component last = this.GetComponentAt(e.MousePosition);

                if (last != mMouseDownControl)
                {
                    if (mMouseDownControl.MouseIn)
                        mMouseDownControl.OnMouseLeave(e);
                }
                else
                {
                    if (!mMouseDownControl.MouseIn)
                        mMouseDownControl.OnMouseEnter(e);

                    mMouseDownControl.OnMouseMove(e);
                }
            }
        }
        //void Container_MouseUp(object sender, InputEventArgs e)
        internal override void  OnMouseUp(InputEventArgs e)
        {
            if (mMouseDownControl == null)
            {
                base.OnMouseUp(e);
                return;
            }

            mMouseDownControl.OnMouseUp(e);
            mMouseDownControl = null;
        }
        internal override void  OnMouseDown(InputEventArgs e)
        {
            mMouseDownControl = this.GetComponentAt(e.MousePosition);

            if (mMouseDownControl == null || mMouseDownControl == this)
            {
                mMouseDownControl = null;
                base.OnMouseDown(e);

                return;
            }

            mMouseDownControl.OnMouseDown(e);
        }

        #endregion
        #region --- Drawing ---

        public override void Draw()
        {
            base.Draw();

            for (int i = 0; i < mChildren.Count; i++)
                mChildren[i].Draw();
        }

        #endregion

        #region --- Managing child components ---


        public void AddChild(Component child)
        {
            if (child == null)
                throw new NullReferenceException("Cannot add a null reference as a child.");
            
            // make sure we are not adding a component which is a parent somewhere of this control
            if (child is Container)
            {
                if (IsAnyParent(child as Container))
                    throw new Exception("Cannot add as a child a control which is a parent to this control.");
            }

            
            mChildren.Add(child);
            child.Parent = this;
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
                else if (mChildren[i] is Container)
                {
                    if ((mChildren[i] as Container).IsAnyChild(test))
                        return true;
                }
            }

            return false;
        }
        void Container_SizeChanged(object sender, ResizeEventArgs e)
        {
            ResizeChildren(e.OldSize);
        }

        protected internal void ChildHasFocus(Component c)
        {
            if (mChildren.Contains(c) == false)
                throw new Exception("ChildHasFocus called by a component that is not a child!");

            SetFocus();

            for (int i = 0; i < mChildren.Count; i++)
            {
                if (c != mChildren[i])
                {
                    mChildren[i].LoseFocus();
                }
            }
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
                    if (mChildren[i] is Container)
                        return (mChildren[i] as Container).GetComponentAt(pt);
                    else
                        return mChildren[i];
                }
            }

            return this;
        }

        public void RaiseChild(Component c)
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
        public void LowerChild(Component c)
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
        public void BringChildToFront(Component c)
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
        public void SendChildToBack(Component c)
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
        /// Returns the list of child components belonging to this control.
        /// </summary>
        public IEnumerable<Component> Children
        {
            get { return mChildren; }
        }

        #endregion

    }
}
