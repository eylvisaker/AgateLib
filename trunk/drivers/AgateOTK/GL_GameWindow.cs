using System;
using System.Collections.Generic;
using System.Text;

using ERY.AgateLib.ImplBase;
using ERY.AgateLib.Geometry;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ERY.AgateLib.OpenGL
{
    class GL_GameWindow : DisplayWindowImpl, GL_IRenderTarget
    {
        GL_Display mDisplay;
        System.Drawing.Icon mIcon;
        GameWindow mWindow;
        string mTitle;
        int mWidth;
        int mHeight;
        bool mAllowResize;
        bool mHasFrame;
        WindowPosition mCreatePosition;

        public GL_GameWindow(CreateWindowParams windowParams)
        {
            mCreatePosition = windowParams.WindowPosition;
            
            if (string.IsNullOrEmpty(windowParams.IconFile) == false)
                mIcon = new System.Drawing.Icon(windowParams.IconFile);
            else
                mIcon = AgateLib.WinForms.FormUtil.AgateLibIcon;

            mTitle = windowParams.Title;
            mWidth = windowParams.Width;
            mHeight = windowParams.Height;
            mAllowResize = windowParams.IsResizable;
            mHasFrame = windowParams.HasFrame;

            if (windowParams.IsFullScreen)
                CreateFullScreenDisplay();
            else
                CreateWindowedDisplay();

            mDisplay = Display.Impl as GL_Display;
            mDisplay.InitializeGL();

            mDisplay.ProcessEventsEvent += new EventHandler(mDisplay_ProcessEventsEvent);
        }

        void mWindow_CloseWindow(object sender, EventArgs e)
        {
            mWindow.Dispose();
            mWindow = null;
        }

        void Keyboard_KeyDown(OpenTK.Input.KeyboardDevice sender, OpenTK.Input.Key key)
        {
            KeyCode code = TransformKey(key);

            Keyboard.Keys[code] = true;
        }

        void Keyboard_KeyUp(OpenTK.Input.KeyboardDevice sender, OpenTK.Input.Key key)
        {
            KeyCode code = TransformKey(key);

            Keyboard.Keys[code] = false;
        }

        void Mouse_ButtonUp(OpenTK.Input.MouseDevice sender, OpenTK.Input.MouseButton button)
        {
            Mouse.MouseButtons agatebutton = TransformButton(button);
            Mouse.Buttons[agatebutton] = false;
        }

        void Mouse_ButtonDown(OpenTK.Input.MouseDevice sender, OpenTK.Input.MouseButton button)
        {
            Mouse.MouseButtons agatebutton = TransformButton(button);
            Mouse.Buttons[agatebutton] = true;
        }

        private static KeyCode TransformKey(OpenTK.Input.Key key)
        {
            KeyCode code = KeyCode.None;

            switch (key)
            {
                case OpenTK.Input.Key.Down: code = KeyCode.Down; break;
                case OpenTK.Input.Key.Up: code = KeyCode.Up; break;
                case OpenTK.Input.Key.Left: code = KeyCode.Left; break;
                case OpenTK.Input.Key.Right: code = KeyCode.Right; break;
                case OpenTK.Input.Key.Escape: code = KeyCode.Escape; break;
            }
            return code;
        }
        private static Mouse.MouseButtons TransformButton(OpenTK.Input.MouseButton button)
        {
            Mouse.MouseButtons agatebutton = Mouse.MouseButtons.None;

            switch (button)
            {
                case OpenTK.Input.MouseButton.Left: agatebutton = Mouse.MouseButtons.Primary; break;
                case OpenTK.Input.MouseButton.Middle: agatebutton = Mouse.MouseButtons.Middle; break;
                case OpenTK.Input.MouseButton.Right: agatebutton = Mouse.MouseButtons.Secondary; break;
                case OpenTK.Input.MouseButton.Button1: agatebutton = Mouse.MouseButtons.ExtraButton1; break;
                case OpenTK.Input.MouseButton.Button2: agatebutton = Mouse.MouseButtons.ExtraButton2; break;
                case OpenTK.Input.MouseButton.Button3: agatebutton = Mouse.MouseButtons.ExtraButton3; break;
            }
            return agatebutton;
        }
        
        private void AttachEvents()
        {
            mWindow.CloseWindow += new EventHandler(mWindow_CloseWindow);
            mWindow.Resize += new OpenTK.Platform.ResizeEvent(mWindow_Resize);
            
            mWindow.Keyboard.KeyDown += new OpenTK.Input.KeyDownEvent(Keyboard_KeyDown);
            mWindow.Keyboard.KeyUp += new OpenTK.Input.KeyUpEvent(Keyboard_KeyUp);

            mWindow.Mouse.ButtonDown += new OpenTK.Input.MouseButtonDownEvent(Mouse_ButtonDown);
            mWindow.Mouse.ButtonUp += new OpenTK.Input.MouseButtonUpEvent(Mouse_ButtonUp);
        }


        
        private void DetachEvents()
        {
            mWindow.CloseWindow -= mWindow_CloseWindow;
            mWindow.Resize -= mWindow_Resize;

            mWindow.Keyboard.KeyDown -= Keyboard_KeyDown;
            mWindow.Keyboard.KeyUp -= Keyboard_KeyUp;

            mWindow.Mouse.ButtonDown -= Mouse_ButtonDown;
            mWindow.Mouse.ButtonUp -= Mouse_ButtonUp;
        }

        void mWindow_Resize(object sender, OpenTK.Platform.ResizeEventArgs e)
        {
            GL.Viewport(0, 0, mWindow.Width, mWindow.Height);
           
        }

        private void CreateWindowedDisplay()
        {
            mWindow = new GameWindow(mWidth, mHeight,
                new OpenTK.Graphics.GraphicsMode(), mTitle);

            AttachEvents();
        }


        private void CreateFullScreenDisplay()
        {
            mWindow = new GameWindow(mWidth, mHeight,
                new OpenTK.Graphics.GraphicsMode(), mTitle, GameWindowFlags.Fullscreen);

            AttachEvents();
        }

        void mDisplay_ProcessEventsEvent(object sender, EventArgs e)
        {
            try
            {
                mWindow.ProcessEvents();
            }
            catch (ApplicationException)
            {
                Dispose();
            }
        }

        public override void Dispose()
        {
            if (mWindow != null)
            {
                mWindow.Dispose();
                mWindow = null;
            }

            mDisplay.ProcessEventsEvent -= mDisplay_ProcessEventsEvent;
        }

        public override bool IsClosed
        {
            get
            {
                if (mWindow == null)
                    return true;


                return false;
            }
        }

        public override bool IsFullScreen
        {
            get { return false; }
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

        public override Size Size
        {
            get
            {
                return new Size(mWindow.Width, mWindow.Height);
            }
            set
            {
                mWindow.Width = value.Width;
            }
        }

        public override string Title
        {
            get
            {
                return mWindow.Title;
            }
            set
            {
                mWindow.Title = value;
            }
        }

        public override Point MousePosition
        {
            get
            {
                return new Point(mWindow.Mouse.Position.X, mWindow.Mouse.Position.Y);
            }
            set
            {
                //mWindow.Mouse.Position = new System.Drawing.Point(value.X, value.Y);
            }
        }

        public override void BeginRender()
        {
            if (mWindow.Context.VSync != mDisplay.VSync)
                mWindow.Context.VSync = mDisplay.VSync;

            MakeCurrent();

            mDisplay.SetupGLOrtho(Rectangle.FromLTRB(0, 0, Width, Height));

        }

        public override void EndRender()
        {
            mWindow.SwapBuffers();
        }

        #region GL_IRenderTarget Members

        public void MakeCurrent()
        {
            mWindow.Context.MakeCurrent(mWindow.WindowInfo);
        }

        #endregion

    }
}
