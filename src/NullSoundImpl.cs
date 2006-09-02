using System;
using System.Collections.Generic;
using System.Text;
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
            Registrar.RegisterAudioDriver(new AudioDriverInfo(
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
