using System;
using System.Collections.Generic;
using System.Text;
using ERY.AgateLib.GuiBase;

namespace ERY.AgateLib.Gui
{
    public class GuiManager : Container 
    {
        private bool mUserInterface = false;


         public GuiManager(StyleManager style)
            : base("GUIManager")
        {
            SetStyleManager(style);

            Mouse.MouseMove += new InputEventHandler(Mouse_MouseMove);
            Mouse.MouseUp += new InputEventHandler(Mouse_MouseUp);
            Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
            Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
            Keyboard.KeyUp += new InputEventHandler(Keyboard_KeyUp);

        }

        public override void Dispose()
        {
            Mouse.MouseMove -= Mouse_MouseMove;
            Mouse.MouseUp -= Mouse_MouseUp;
            Mouse.MouseDown -= Mouse_MouseDown;
            Keyboard.KeyDown -= Keyboard_KeyDown;
            Keyboard.KeyUp -= Keyboard_KeyUp;

            base.Dispose();
        }


        public bool UserInterface
        {
            get { return mUserInterface; }
            set { mUserInterface = value; }
        }

        void Keyboard_KeyUp(InputEventArgs e)
        {
            if (!mUserInterface) return;

            OnKeyUp(e);
        }

        void Keyboard_KeyDown(InputEventArgs e)
        {
            if (!mUserInterface) return;

            OnKeyDown(e);
        }

        void Mouse_MouseDown(InputEventArgs e)
        {
            if (!mUserInterface) return;

            OnMouseDown(e);
        }

        void Mouse_MouseUp(InputEventArgs e)
        {
            if (!mUserInterface) return;

            OnMouseUp(e);
        }

        void Mouse_MouseMove(InputEventArgs e)
        {
            if (!mUserInterface) return;

            OnMouseMove(e);
        }

    }
}
