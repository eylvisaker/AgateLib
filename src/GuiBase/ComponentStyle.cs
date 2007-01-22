using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib.GuiBase
{
    public abstract class ComponentStyle
    {
        private Component mComponent;

        public ComponentStyle()
        {

        }
        public void Attach(Component c)
        {
            mComponent = c;

            mComponent.PaintBegin += new EventHandler(Component_PaintBegin);
            mComponent.PaintEnd += new EventHandler(Component_PaintEnd);
            mComponent.Paint += new EventHandler(Component_Paint);

            InitializeAfterConnect();
        }

        public abstract void InitializeAfterConnect();

        public abstract void Component_Paint(object sender, EventArgs e);
        public virtual void Component_PaintEnd(object sender, EventArgs e)
        { }
        public virtual void Component_PaintBegin(object sender, EventArgs e)
        { }

        public virtual bool IsPointInComponent(Point pt)
        {
            if (mComponent.Bounds.Contains(pt))
                return true;
            else
                return false;
        }

        public Component MyComponent
        {
            get { return mComponent; }
        }

        public abstract void DoAutoSize();

        /// <summary>
        /// Calculates and sets the client area for the control.
        /// This will only be called for controls deriving from the Container class.
        /// </summary>
        public virtual void UpdateClientArea()
        {
            if (MyComponent is Container == false)
                throw new Exception("Error: UpdateClientArea called on a non-Container class.");
        }
    }
}
