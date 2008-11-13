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

using AgateLib.ImplementationBase;
using AgateLib.InputLib;
using AgateLib.WinForms;

namespace AgateLib.DisplayLib.SystemDrawing
{
    class Drawing_DisplayWindow : DisplayWindowImpl, Drawing_IRenderTarget 
    {
        Form frm;
        Control mRenderTarget;
        bool mIsClosed = false;

        Icon mIcon;
        Bitmap mBackBuffer;

        public Drawing_DisplayWindow(CreateWindowParams windowParams)
        {
            if (windowParams.RenderToControl == true)
            {
                if (typeof(Control).IsAssignableFrom(windowParams.RenderTarget.GetType()) == false)
                    throw new ArgumentException("The specified render target does not derive from System.Windows.Forms.Control");

                mRenderTarget = (Control)windowParams.RenderTarget;

                AttachEvents();

                OnResize();
            }
            else
            {
                WinForms.FormUtil.InitializeWindowsForm(out frm, out mRenderTarget, windowParams.WindowPosition, windowParams.Title,
                    windowParams.Width, windowParams.Height, windowParams.IsFullScreen, windowParams.IsResizable, 
                    windowParams.HasFrame);

                if (string.IsNullOrEmpty(windowParams.IconFile) == false)
                {
                    mIcon = new Icon(windowParams.IconFile);
                    frm.Icon = mIcon;
                }

                // finally, show the form
                frm.Show();

                AttachEvents();

                // and create the back buffer
                OnResize();
            }
        }
        public Drawing_DisplayWindow(Control renderTarget)
        {
            
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
                retval = Mouse.MouseButtons.Primary;
            if ((buttons & MouseButtons.Right) != 0)
                retval = Mouse.MouseButtons.Secondary;
            if ((buttons & MouseButtons.Middle) != 0)
                retval = Mouse.MouseButtons.Middle;
            if ((buttons & MouseButtons.XButton1) != 0)
                retval = Mouse.MouseButtons.ExtraButton1;
            if ((buttons & MouseButtons.XButton2) != 0)
                retval = Mouse.MouseButtons.ExtraButton2;

            return retval;
        }

        public void form_FormClosed(object sender, EventArgs e)
        {
            mIsClosed = true;
        }
        public void form_FormClosing(object sender, EventArgs e)
        {

        }
        void form_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = false;
        }
        void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Keyboard.Keys[FormUtil.TransformWinFormsKey(e.KeyCode)] = true;
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
            mIsClosed = true;
        }

        public override void Dispose()
        {
            if (frm != null)
                frm.Dispose();

            mIsClosed = true;
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
            if (mRenderTarget.ClientSize.Width == 0 || mRenderTarget.ClientSize.Height == 0)
                return;

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

        public override bool IsClosed
        {
            get { return mIsClosed; }
        }

        public override Geometry.Size Size
        {
            get
            {
                return Interop.Convert(mRenderTarget.ClientSize);
            }
            set
            {
                if (frm != null)
                    frm.ClientSize = Interop.Convert(value);
            }
        }

        public override Geometry.Point MousePosition
        {
            get
            {
                return Interop.Convert(mRenderTarget.PointToClient(Cursor.Position));
            }
            set
            {
                Cursor.Position = mRenderTarget.PointToScreen(Interop.Convert(value));
            }
        }

        public override bool IsFullScreen
        {
            get { return false; }
        }

        public override void BeginRender()
        {
        }

        public override void EndRender()
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

        public override void SetWindowed()
        {
        }

        public override void SetFullScreen()
        {
        }

        public override void SetFullScreen(int width, int height, int bpp)
        {
        }
    }
}
