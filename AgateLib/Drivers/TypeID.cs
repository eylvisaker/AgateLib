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

namespace AgateLib.Drivers
{

    /// <summary>
    /// List of identifiers of known or planned display drivers.
    /// </summary>
    /// <remarks>
    /// This really seems like an overengineered piece of garbage
    /// designed just to use constraints in generic collections.
    /// I'd get rid of the whole thing and replace it with enums, but
    /// it's currently working, and I don't have any need to change it.
    /// But if the need ever arises, I will redo this whole thing.
    /// </remarks>
    public class DisplayTypeID : DriverTypeIDBase, IEquatable<DisplayTypeID>
    {
        private int value;

        private DisplayTypeID()
        { }
        private DisplayTypeID(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Implementation of IEquatable&lt;int&gt; from base class.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(int other)
        {
            return value == other;
        }
        /// <summary>
        /// Implementation of IEquatable&lt;DisplayTypeID&gt;.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DisplayTypeID other)
        {
            return Equals(other.value);
        }

        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// display driver for the system.
        /// </summary>
        public static DisplayTypeID AutoSelect
        {
            get { return new DisplayTypeID(0); }
        }
        /// <summary>
        /// The reference driver is implemented using System.Drawing.  This is useful for
        /// debugging the development of a new driver, as it should behave exactly like the
        /// reference driver (but hopefully be much faster).
        /// </summary>
        public static DisplayTypeID Reference
        {
            get { return new DisplayTypeID(1); }
        }


        /// <summary>
        /// Driver Implementation using Managed DirectX 1.1.
        /// </summary>
        public static DisplayTypeID Direct3D_MDX_1_1
        {
            get
            {
                return new DisplayTypeID(0x100);
            }
        }

        /// <summary>
        /// Driver implementation using Managed DirectX 2.0 beta.  Since Microsoft has discontinued
        /// development on MDX2.0 in favor of the XNA framework);}} this driver is obsolete.
        /// </summary>
        [Obsolete]
        public static DisplayTypeID Direct3D_MDX_2_0_Beta
        {
            get { return new DisplayTypeID(0x101); }
        }

        /// <summary>
        /// Driver Implementation using XNA Studio.
        /// </summary>
        public static DisplayTypeID Direct3D_XNA
        {
            get { return new DisplayTypeID(0x110); }
        }

        /// <summary>
        /// Driver implementation using OpenGL);}} with WGL for creation of windows and management of
        /// memory.
        /// </summary>
        public static DisplayTypeID WGL
        {
            get { return new DisplayTypeID(0x200); }
        }

        /// <summary>
        /// Driver implememtation using OpenGL);}} with some platform-independent library for window
        /// creation.
        /// </summary>
        public static DisplayTypeID OpenGL { get { return new DisplayTypeID(0x210); } }

        /// <summary>
        /// Driver implementation using SDL.  SDL.NET does not support many of the basic features
        /// of this library (notably);}} rotation of images) so is not considered an adequate driver
        /// for general purpose use.
        /// </summary>
        [Obsolete]
        public static DisplayTypeID SDL { get { return new DisplayTypeID(0x300); } }

    };

    /// <summary>
    /// List of identifiers of known or planned audio drivers.
    /// </summary>
    public class AudioTypeID : DriverTypeIDBase, IEquatable<AudioTypeID>
    {

        private int value;

        private AudioTypeID()
        { }
        private AudioTypeID(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Implementation of IEquatable&lt;int&gt; from base class.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(int other)
        {
            return value == other;
        }
        /// <summary>
        /// Implementation of IEquatable&lt;AudioTypeID&gt;.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(AudioTypeID other)
        {
            return Equals(other.value);
        }
        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// audio driver for the system.
        /// </summary>
        public static AudioTypeID AutoSelect { get { return new AudioTypeID(0); } }

        /// <summary>
        /// A driver which does nothing.
        /// </summary>
        public static AudioTypeID Silent { get { return new AudioTypeID(-0x100); } }

        /// <summary>
        /// A DirectSound implementation.
        /// </summary>
        public static AudioTypeID DirectSound { get { return new AudioTypeID(0x100); } }
        /// <summary>
        /// Implementation using XNA Studio
        /// (what will this be called), anyway?)
        /// </summary>
        public static AudioTypeID XAct { get { return new AudioTypeID(0x110); } }

        /// <summary>
        /// Implementation using the cross-platform OpenAL library.
        /// </summary>
        public static AudioTypeID OpenAL { get { return new AudioTypeID(0x200); } }

        /// <summary>
        /// Implementation using the cross-platform FMod library.
        /// </summary>
        public static AudioTypeID FMod { get { return new AudioTypeID(0x300); } }
    }

    /// <summary>
    /// List of identifiers of known or planned input drivers.
    /// </summary>
    public class InputTypeID : DriverTypeIDBase, IEquatable<InputTypeID>
    {

        private int value;

        private InputTypeID()
        { }
        private InputTypeID(int value)
        {
            this.value = value;
        }

        /// <summary>
        /// Implementation of IEquatable&lt;int&gt; from base class.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(int other)
        {
            return value == other;
        }
        /// <summary>
        /// Implementation of IEquatable&lt;InputTypeID&gt;.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(InputTypeID other)
        {
            return Equals(other.value);
        }

        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// audio driver for the system.
        /// </summary>
        public static InputTypeID AutoSelect { get { return new InputTypeID(0); } }

        /// <summary>
        /// A driver with no joysticks.
        /// </summary>
        public static InputTypeID Silent { get { return new InputTypeID(-0x100); } }

        /// <summary>
        /// A DirectSound implementation.
        /// </summary>
        public static InputTypeID DirectInput { get { return new InputTypeID(0x100); } }
        /// <summary>
        /// Implementation using the XNA framework.
        /// </summary>
        public static InputTypeID XInput { get { return new InputTypeID(0x110); } }
    }

}
