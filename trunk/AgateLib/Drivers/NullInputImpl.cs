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

using AgateLib.Drivers;
using AgateLib.ImplementationBase;

namespace AgateLib.Drivers
{
    class NullInputImpl : InputImpl 
    {

        public override void Initialize()
        {
            Report("No input driver found.  Joysticks will not work.");            
        }

        public override void Dispose()
        {
            
        }

        public override int JoystickCount
        {
            get { return 0; }
        }

        public override IEnumerable<JoystickImpl> CreateJoysticks()
        {
            return new List<JoystickImpl>();
        }
    }
}
