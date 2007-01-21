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

        #region --- Construction / Destruction ---

        public Container()
        {
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

        public override void Draw()
        {
            // tell all the children to paint themselves.
            List<Component> children = Children;

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Draw();
            }

        }


        #endregion

    }
}
