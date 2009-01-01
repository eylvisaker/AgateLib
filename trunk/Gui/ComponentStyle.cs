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
using AgateLib.Geometry;

namespace AgateLib.Gui
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
