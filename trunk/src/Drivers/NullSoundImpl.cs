//     ``The contents of this file are subject to the Mozilla Public License
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

using ERY.AgateLib.Drivers;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
{
    /// <summary>
    /// Class which provides a silent Audio implementation.
    /// </summary>
    public class NullSoundImpl : AudioImpl 
    {
        /// <summary>
        /// 
        /// </summary>
        public class NullSoundBufferImpl : SoundBufferImpl
        {
            /// <summary>
            /// 
            /// </summary>
            public override void Dispose()
            {
                
            }
            /// <summary>
            /// 
            /// </summary>
            public override double Volume
            {
                get
                {
                    return 0;
                }
                set
                {
                    
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public class NullSoundBufferSessionImpl : SoundBufferSessionImpl
        {
            /// <summary>
            /// 
            /// </summary>
            public override void Dispose()
            {
                
            }
            /// <summary>
            /// 
            /// </summary>
            public override void Play()
            {
                
            }

            /// <summary>
            /// 
            /// </summary>
            public override void Stop()
            {
                
            }

            /// <summary>
            /// 
            /// </summary>
            public override double Volume
            {
                get
                {
                    return 0;
                }
                set
                {
                    
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override double Pan
            {
                get
                {
                    return 0;
                }
                set
                {
                    
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override bool IsPlaying
            {
                get { return false; }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class NullMusicImpl : MusicImpl
        {

            /// <summary>
            /// 
            /// </summary>
            protected override void OnSetLoop(bool value)
            {
                
            }

            /// <summary>
            /// 
            /// </summary>
            public override void Dispose()
            {
                
            }

            /// <summary>
            /// 
            /// </summary>
            public override void Play()
            {
                
            }

            /// <summary>
            /// 
            /// </summary>
            public override void Stop()
            {
                
            }

            /// <summary>
            /// 
            /// </summary>
            public override double Volume
            {
                get
                {
                    return 0;
                }
                set
                {
                    
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override double Pan
            {
                get
                {
                    return 0;
                }
                set
                {
                    
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public override bool IsPlaying
            {
                get { return false; }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {
            Registrar.RegisterAudioDriver(new DriverInfo<AudioTypeID>(
                typeof(NullSoundImpl), AudioTypeID.Silent, "Silent", -100));
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override SoundBufferImpl CreateSoundBuffer(string filename)
        {
            return new NullSoundBufferImpl();
        }

        /// <summary>
        /// 
        /// </summary>
        public override MusicImpl CreateMusic(string filename)
        {
            return new NullMusicImpl();
        }

        /// <summary>
        /// 
        /// </summary>
        public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
        {
            return new NullSoundBufferSessionImpl();
        }
    }
}
