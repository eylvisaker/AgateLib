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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using AgateLib;
using AgateLib.Geometry;

using OpenTK.Graphics;

namespace AgateOTK
{
    class GLState
    {
        #region --- Private variables for state management ---

        private GLDrawBuffer mDrawBuffer;

        #endregion

        public GLState()
        {
             mDrawBuffer = new GLDrawBuffer(this);
        }


        public GLDrawBuffer DrawBuffer
        {
            get { return mDrawBuffer; }
        }

        public void SetGLColor(Color color)
        {
            GL.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }


    }
}
