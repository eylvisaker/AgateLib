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

using AgateLib.Core;
using AgateLib.Input;

namespace AgateLib.Gui
{
    public class GuiManager : Container 
    {
        private bool mUserInterface = true;


        public GuiManager(StyleManager style)
        {
            SetStyleManager(style);

            Mouse.MouseMove += new InputEventHandler(Mouse_MouseMove);
            Mouse.MouseUp += new InputEventHandler(Mouse_MouseUp);
            Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
            Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
            Keyboard.KeyUp += new InputEventHandler(Keyboard_KeyUp);

            AgateCore.KeepAliveEvent += new AgateCore.KeepAliveDelegate(Update);
        }


        public override void Dispose()
        {
            Mouse.MouseMove -= Mouse_MouseMove;
            Mouse.MouseUp -= Mouse_MouseUp;
            Mouse.MouseDown -= Mouse_MouseDown;
            Keyboard.KeyDown -= Keyboard_KeyDown;
            Keyboard.KeyUp -= Keyboard_KeyUp;

            AgateCore.KeepAliveEvent -= Update;

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
