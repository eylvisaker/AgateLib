using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.Geometry;
using ERY.AgateLib.GuiBase;

namespace ERY.AgateLib.Gui
{
    public class Window : Container
    {
        private string mTitle;
        private Size mTargetSize;
        private SizeF mInternalSize;
        private const double sizeVelocity = 200;

        public Window()
        {
            Location = new Point(10,10);

            mInternalSize = new SizeF(Width, Height);
            mTargetSize = Size;
        }

        public Window(Container parent, Point location, string title)
            : base(parent, new Rectangle(location, new Size(100, 100)))
        {
            mTitle = title;

            mStyle.UpdateClientArea();

            mInternalSize = new SizeF(Width, Height);
            mTargetSize = Size;
        }


        public string Title
        {
            get { return mTitle; }
            set { mTitle = value; }
        }

        public void SkipSizeTransition()
        {
            base.Size = mTargetSize;
            mInternalSize = new SizeF(Width, Height);
        }

        protected internal override void Update()
        {
            if (Size != mTargetSize)
            {
                double dW = (mTargetSize.Width - mInternalSize.Width) * Display.DeltaTime;
                double dH = (mTargetSize.Height - mInternalSize.Height) * Display.DeltaTime;

                mInternalSize.Width += (float)dW;
                mInternalSize.Height += (float)dH;

                Size = (Size)mInternalSize;
            }
        }
        public override Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                mTargetSize = value;
            }
        }

        /// <summary>
        /// Sets the region inside the window which is the client area.
        /// This should only be called by the style which is attached to the window.
        /// </summary>
        /// <param name="client"></param>
        public new void SetClientArea(Rectangle client)
        {
            base.SetClientArea(client);
        }
    }
}
