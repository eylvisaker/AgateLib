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
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib.SystemDrawing
{
    class Drawing_DisplayWindow : DisplayWindowImpl, Drawing_IRenderTarget 
    {
        Form frm;
        Control mRenderTarget;
        bool mClosed = false;

        Icon mIcon;
        Bitmap mBackBuffer;

        public Drawing_DisplayWindow(string title, int clientWidth, int clientHeight, string iconFile,
            bool startFullscreen, bool allowResize)
        {
            InitializeWindowsForm(out frm, out mRenderTarget, title, clientWidth, clientHeight, startFullscreen, allowResize);

            if (string.IsNullOrEmpty(iconFile) == false)
            {
                mIcon = new Icon(iconFile);
                frm.Icon = mIcon;
            }
       
            // finally, show the form
            frm.Show();

            AttachEvents();

            // and create the back buffer
            OnResize();
        }
        public Drawing_DisplayWindow(Control renderTarget)
        {
            mRenderTarget = renderTarget;

            AttachEvents();

            OnResize();
        }

        private void AttachEvents()
        {
            mRenderTarget.Resize += new EventHandler(frm_Resize);
            mRenderTarget.Paint += new PaintEventHandler(frm_Paint);

            mRenderTarget.MouseMove += new MouseEventHandler(pct_MouseMove);
            mRenderTarget.MouseDown += new MouseEventHandler(pct_MouseDown);
            mRenderTarget.MouseUp += new MouseEventHandler(pct_MouseUp);
            //mRenderTarget.MouseDoubleClick += new MouseEventHandler(pct_MouseDoubleClick);

            mRenderTarget.Disposed += new EventHandler(renderTarget_Disposed);


            System.Windows.Forms.Form form = (mRenderTarget.TopLevelControl as System.Windows.Forms.Form);

            form.KeyPreview = true;
            form.KeyDown += new System.Windows.Forms.KeyEventHandler(form_KeyDown);
            form.KeyUp += new System.Windows.Forms.KeyEventHandler(form_KeyUp);

            // fuck, it seems that FormClosing had a different name in .NET 1.1, which is
            // the version of windows that Mono implements.  
            // So here's an ugly System.Reflection hack around it.
            {
                EventInfo formClosing = GetFormEvent("FormClosing", "Closing");
                MethodInfo method = this.GetType().GetMethod("form_FormClosing");

                Delegate d = Delegate.CreateDelegate(formClosing.EventHandlerType, this, method);

                formClosing.AddEventHandler(form,d );
            }
            {
                EventInfo formClosed = GetFormEvent("FormClosed", "Closed");
                MethodInfo method = this.GetType().GetMethod("form_FormClosed");

                Delegate d = Delegate.CreateDelegate(formClosed.EventHandlerType, this, method);

                formClosed.AddEventHandler(form, d);
            }
        }

        private EventInfo GetFormEvent(params string[] eventNames)
        {
            Type formType = typeof(System.Windows.Forms.Form);

            foreach (string name in eventNames)
            {
                EventInfo evt = formType.GetEvent(name);

                if (evt != null)
                    return evt;
            }

            return null;
        }


        Mouse.MouseButtons GetButtons(MouseButtons buttons)
        {
            Mouse.MouseButtons retval = Mouse.MouseButtons.None;

            if ((buttons & MouseButtons.Left) != 0)
                retval |= Mouse.MouseButtons.Primary;
            if ((buttons & MouseButtons.Right) != 0)
                retval |= Mouse.MouseButtons.Secondary;
            if ((buttons & MouseButtons.Middle) != 0)
                retval |= Mouse.MouseButtons.Middle;
            if ((buttons & MouseButtons.XButton1) != 0)
                retval |= Mouse.MouseButtons.ExtraButton1;
            if ((buttons & MouseButtons.XButton2) != 0)
                retval |= Mouse.MouseButtons.ExtraButton2;

            return retval;
        }

        public void form_FormClosed(object sender, EventArgs e)
        {
            mClosed = true;
        }
        public void form_FormClosing(object sender, EventArgs e)
        {

        }
        void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, false);
        }
        void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys.SetWinFormsKey(e.KeyCode, true);
        }

        void pct_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Mouse.MouseButtons btn = GetButtons(e.Button);

            Mouse.OnMouseDoubleClick(btn);
        }

        void pct_MouseUp(object sender, MouseEventArgs e)
        {
            Mouse.MouseButtons btn = GetButtons(e.Button);

            Mouse.Buttons[btn] = false;
        }

        void pct_MouseDown(object sender, MouseEventArgs e)
        {
            Mouse.MouseButtons btn = GetButtons(e.Button);

            Mouse.Buttons[btn] = true;
        }

        void pct_MouseMove(object sender, MouseEventArgs e)
        {
            Mouse.OnMouseMove();
        }

        void renderTarget_Disposed(object sender, EventArgs e)
        {
            mClosed = true;
        }

        public override void Dispose()
        {
            if (frm != null)
                frm.Dispose();

            mClosed = true;
        }

        void frm_Paint(object sender, PaintEventArgs e)
        {

        }
        void frm_Resize(object sender, EventArgs e)
        {
            OnResize();
        }

        private void OnResize()
        {
            mBackBuffer = new Bitmap(mRenderTarget.ClientSize.Width, mRenderTarget.ClientSize.Height);
        }

        public Control RenderTarget
        {
            get
            {
                return mRenderTarget;
            }
        }
        public Bitmap BackBuffer
        {
            get { return mBackBuffer; }
        }

        public override bool Closed
        {
            get { return mClosed; }
        }

        public override Geometry.Size Size
        {
            get
            {
                return new Geometry.Size(mRenderTarget.ClientSize);
            }
            set
            {
                if (frm != null)
                    frm.ClientSize = (Size)value;
            }
        }

        public override Geometry.Point MousePosition
        {
            get
            {
                return new Geometry.Point(mRenderTarget.PointToClient(Cursor.Position));
            }
            set
            {
                Cursor.Position = mRenderTarget.PointToScreen((Point)value);
            }
        }

        public override bool IsFullScreen
        {
            get { return false; }
        }

        public override void ToggleFullScreen()
        {
        }
        public override void ToggleFullScreen(int width, int height, int bpp)
        {
        }

        public override void BeginRender()
        {
        }

        public override void EndRender(bool waitVSync)
        {
            Graphics g = RenderTarget.CreateGraphics();

            g.DrawImage(BackBuffer, new Rectangle(new Point(0, 0), BackBuffer.Size));
            g.Dispose();
        }


        public override string Title
        {
            get
            {
                if (frm != null)
                    return frm.Text;
                else
                    return "";
            }
            set
            {
                if (frm != null)
                    frm.Text = value;                    
            }
        }
    }
}
