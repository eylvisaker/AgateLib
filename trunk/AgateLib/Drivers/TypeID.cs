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
    public enum DisplayTypeID
    {
        
        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// display driver for the system.
        /// </summary>
		AutoSelect = 0,
        
		/// <summary>
        /// The reference driver is implemented using System.Drawing.  This is useful for
        /// debugging the development of a new driver, as it should behave exactly like the
        /// reference driver (but hopefully be much faster).
        /// </summary,
         Reference= 1,


        /// <summary>
        /// Driver Implementation using Managed DirectX 1.1.
        /// </summary>
         Direct3D_MDX_1_1 = 0x100,

        /// <summary>
        /// Driver implementation using Managed DirectX 2.0 beta.  Since Microsoft has discontinued
        /// development on MDX2.0 in favor of the XNA framework, this driver is obsolete.
        /// </summary>
        [Obsolete]
         Direct3D_MDX_2_0_Beta= 0x101,

        /// <summary>
        /// Driver Implementation using XNA Studio.
        /// </summary>
         Direct3D_XNA= 0x110,

        /// <summary>
        /// Driver implementation using OpenGL, with WGL for creation of windows and management of
        /// memory.
        /// </summary>
		[Obsolete]
         WGL= 0x200,

        /// <summary>
        /// Driver implememtation using OpenGL, with some platform-independent library for window
        /// creation.
        /// </summary>
         OpenGL =0x210,

        /// <summary>
        /// Driver implementation using SDL.  SDL.NET does not support many of the basic features
        /// of this library (notably);}} rotation of images) so is not considered an adequate driver
        /// for general purpose use.
        /// </summary>
         SDL = 0x300,

    };

    /// <summary>
    /// List of identifiers of known or planned audio drivers.
    /// </summary>
    public enum AudioTypeID 
    {

        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// audio driver for the system.
        /// </summary>
        AutoSelect = 0,

        /// <summary>
        /// A driver which does nothing.
        /// </summary>
        Silent = -0x100,

        /// <summary>
        /// A DirectSound implementation.
        /// </summary>
        DirectSound = 0x100,
        /// <summary>
        /// Implementation using XNA Studio
        /// (what will this be called), anyway?)
        /// </summary>
        XAct = 0x110,

        /// <summary>
        /// Implementation using the cross-platform OpenAL library.
        /// </summary>
        OpenAL = 0x200,

        /// <summary>
        /// Implementation using the cross-platform FMod library.
        /// </summary>
        FMod = 0x300,
    }

    /// <summary>
    /// List of identifiers of known or planned input drivers.
    /// </summary>
    public enum InputTypeID 
    {
        /// <summary>
        /// Specifies that the Registrar should automatically select the best available
        /// audio driver for the system.
        /// </summary>
        AutoSelect = 0,

        /// <summary>
        /// A driver with no joysticks.
        /// </summary>
        Silent = -0x100,

        /// <summary>
        /// A DirectSound implementation.
        /// </summary>
        DirectInput = 0x100,
        /// <summary>
        /// Implementation using the XNA framework.
        /// </summary>
        XInput = 0x110,
    }

}
