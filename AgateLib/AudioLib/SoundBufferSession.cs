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

namespace AgateLib.AudioLib
{
    using Drivers;
    using ImplementationBase;

    /// <summary>
    /// A class which represents a playback instance of a SoundBuffer object.
    /// 
    /// After a SoundBufferSession is done playing, it may be recycled if its
    /// parent SoundBuffer object's Play or CreateSession methods are called.
    /// This behavior can be suppressed by setting the Recycle property to false.
    /// If you do this, you are responsible for freeing the unmanaged resources
    /// on the SoundBufferSession by calling its Dispose method.
    /// </summary>
    public sealed class SoundBufferSession
    {
        private SoundBuffer mSource;
        private SoundBufferSessionImpl impl;
        private bool mRecycle = true;

        private SoundBufferSession()
        { }
        internal SoundBufferSession(SoundBuffer source)
        {
            impl = Audio.Impl.CreateSoundBufferSession(source.Impl);

            mSource = source;
            mSource.StopEvent += new Audio.AudioCoreEventDelegate(Stop);

            Initialize();

        }

        internal void Initialize()
        {
            Volume = mSource.Volume;
            Pan = mSource.Pan;
        }

        /// <summary>
        /// Destroys the unmanaged resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            impl.Dispose();
            mSource.RemoveSession(this);
        }


        /// <summary>
        /// Returns the implemented object.
        /// </summary>
        public SoundBufferSessionImpl Impl
        {
            get { return impl; }
        }
        /// <summary>
        /// Returns the SoundBuffer object which created this SoundBufferSession.
        /// </summary>
        public SoundBuffer Source
        {
            get { return mSource; }
        }
        /// <summary>
        /// Begins playback of the SoundBufferSession object.
        /// </summary>
        public void Play()
        {
            impl.Play();
        }
        /// <summary>
        /// Stops playback.
        /// </summary>
        public void Stop()
        {
            impl.Stop();
        }

        /// <summary>
        /// Gets or sets the volume. Range is:
        /// 0.0 Quiet
        /// 0.5 Sounds half volume
        /// 1.0 Full volume
        /// </summary>
        public double Volume
        {
            get
            {
                return impl.Volume;
            }
            set
            {
                impl.Volume = value;
            }
        }
        /// <summary>
        /// Gets or sets the left-right balance.  
        /// -1 is entirely in the left speaker,
        ///  0 is equally in both and,
        ///  1 is entirely in the right speaker.
        /// </summary>
        public double Pan
        {
            get { return impl.Pan; }
            set { impl.Pan = value; }
        }
        /// <summary>
        /// Returns true if this Session is playing.
        /// </summary>
        public bool IsPlaying
        {
            get { return impl.IsPlaying; }
        }
        /// <summary>
        /// Gets or sets a bool value which indicates whether or not this
        /// SoundBufferSession object should be recycled when it is done playing.
        /// 
        /// If you set this to false, you should Dispose the SoundBufferSession
        /// object yourself when you're done with it.
        /// </summary>
        public bool Recycle
        {
            get { return mRecycle; }
            set
            {
                if (value != mRecycle)
                {
                    mRecycle = value;

                    if (value)
                        mSource.AddSession(this);
                    else
                        mSource.RemoveSession(this);
                }
            }
        }
    }

}
